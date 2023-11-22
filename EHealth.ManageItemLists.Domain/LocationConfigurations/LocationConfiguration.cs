using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.ItemListTypes;
using EHealth.ManageItemLists.Domain.PackageSubTypes;
using EHealth.ManageItemLists.Domain.PackageTypes;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.LocationConfigurations
{
    public class LocationConfiguration : EHealthDomainObject, IEntity<int>, IValidationModel<LocationConfiguration>
    {
        private LocationConfiguration()
        {

        }
        public int Id { get; set; }
        public bool NeededLocation { get; private set; }
        public int PackageSubTypeId { get; private set; }
        public PackageSubType PackageSubType { get; private set; }
        public AbstractValidator<LocationConfiguration> Validator => new LocationConfigurationValidator();

        public async Task<int> Create(ILocationConfigurationsRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(ILocationConfigurationsRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(ILocationConfigurationsRepository repository)
        {
            return await repository.Delete(this);
        }

        public static async Task<LocationConfiguration> Get(int id, ILocationConfigurationsRepository repository)
        {
            var dbLocationConfiguration = await repository.Get(id);

            if (dbLocationConfiguration is null)
            {
                throw new DataNotFoundException();
            }

            return dbLocationConfiguration;
        }
        public static async Task<PagedResponse<LocationConfiguration>> Search(ILocationConfigurationsRepository repository, Expression<Func<LocationConfiguration, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }
        public static LocationConfiguration Create(int? id, bool neededLocation, int packageSubTypeId, string createdBy)
        {
            return new LocationConfiguration
            {
                Id = id ?? 0,
                NeededLocation = neededLocation,
                PackageSubTypeId = packageSubTypeId,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
            };
        }

        private async Task<bool> EnsureNoDuplicates(ILocationConfigurationsRepository LocationConfigurationsRepository, bool throwException = true)
        {
            var dbLocationConfiguration = await LocationConfigurationsRepository.Search(c => c.NeededLocation == NeededLocation && c.PackageSubTypeId == PackageSubTypeId && c.IsDeleted != true , 1, 1, false);
            if (Id == default)
            {
                if (dbLocationConfiguration.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbLocationConfiguration.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
