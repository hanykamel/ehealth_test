using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.DrugsPackageTypes
{
    public class DrugsPackageType : ItemManagmentBaseClass, IEntity<int>, IValidationModel<DrugsPackageType>
    {
        private DrugsPackageType()
        {
            //default value is Active
            this.Active = true;
        }
        public string NameAr { get; private set; }
        public string NameEN { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEN { get; private set; }
        public AbstractValidator<DrugsPackageType> Validator => new DrugsPackageTypeValidator();
        public async Task<int> Create(IDrugsPackageTypeRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IDrugsPackageTypeRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IDrugsPackageTypeRepository repository)
        {
            return await repository.Delete(this);
        }

        public static async Task<PagedResponse<DrugsPackageType>> Search(IDrugsPackageTypeRepository repository, Expression<Func<DrugsPackageType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }

        public static async Task<DrugsPackageType> Get(int id, IDrugsPackageTypeRepository repository)
        {
            var dbDrugsPackageType = await repository.Get(id);

            if (dbDrugsPackageType is null)
            {
                throw new DataNotFoundException();
            }

            return dbDrugsPackageType;
        }

        public static DrugsPackageType Create(int? id, string code, string nameAr, string nameEN, string DefinitionAr, string DefinitionEN, string createdBy)
        {
            return new DrugsPackageType
            {
                Id = id ?? 0,
                Code = code,
                NameAr = nameAr,
                NameEN = nameEN,
                DefinitionAr = DefinitionAr,
                DefinitionEN = DefinitionEN,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,

            };
        }

        private async Task<bool> EnsureNoDuplicates(IDrugsPackageTypeRepository repository, bool throwException = true)
        {
            var dbDrugsPackageType = await repository.Search(x => x.Id == Id || x.Code == Code, 1, 1, false);
            if (Id == default)
            {
                if (dbDrugsPackageType.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbDrugsPackageType.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
