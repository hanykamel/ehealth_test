using EHealth.ManageItemLists.Domain.PackageSubTypes;
using EHealth.ManageItemLists.Domain.PackageTypes;
using EHealth.ManageItemLists.Domain.PriceUnits;
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

namespace EHealth.ManageItemLists.Domain.Locations
{
    public class Location : ItemManagmentBaseClass, IEntity<int>, IValidationModel<Location>
    {
        private Location()
        {

        }
        public string LocationAr { get; private set; }
        public string LocationEn { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEn { get; private set; }

        public AbstractValidator<Location> Validator => new LocationValidator();

        public async Task<int> Create(ILocationsRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(ILocationsRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(ILocationsRepository repository)
        {
            return await repository.Delete(this);
        }


        public static async Task<Location> Get(int id, ILocationsRepository repository)
        {
            var dbLocation = await repository.GetById(id);

            if (dbLocation is null)
            {
                throw new DataNotFoundException();
            }

            return dbLocation;
        }
        public static async Task<PagedResponse<Location>> Search(ILocationsRepository repository, Expression<Func<Location, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }

        public static Location Create(int? id, string code, string locationAr, string locationEn, string DefinitionAr, string DefinitionEN, int resourceUnitOfCostValue, string createdBy)
        {
            return new Location
            {
                Id = id ?? 0,
                Code = code,
                LocationAr = locationAr,
                LocationEn = locationEn,
                DefinitionAr = DefinitionAr,
                DefinitionEn = DefinitionEN,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now
            };
        }

        private async Task<bool> EnsureNoDuplicates(ILocationsRepository repository, bool throwException = true)
        {
            var dbPriceUnit = await repository.Search(x => x.Code == Code || x.LocationAr == LocationAr || x.LocationEn == LocationEn
            && x.DefinitionAr == DefinitionAr && x.DefinitionEn == DefinitionEn, 1, 1, false);
            if (Id == default)
            {
                if (dbPriceUnit.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbPriceUnit.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
