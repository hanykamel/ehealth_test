using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.UnitsTypes
{
    public class UnitsType : ItemManagmentBaseClass, IEntity<int>, IValidationModel<UnitsType>
    {
        private UnitsType()
        {
            Active = true;
        }
        public string UnitAr { get; private set; }
        public string UnitEn { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEn { get; private set; }
        public AbstractValidator<UnitsType> Validator => new UnitsTypeValidator();
        public async Task<int> Create(IUnitsTypeRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IUnitsTypeRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IUnitsTypeRepository repository)
        {
            return await repository.Delete(this);
        }

        public static async Task<PagedResponse<UnitsType>> Search(IUnitsTypeRepository repository,Expression<Func<UnitsType,bool>>predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }

        public static async Task<UnitsType> Get(int id, IUnitsTypeRepository repository)
        {
            var dbUnitType = await repository.Get(id);

            if (dbUnitType is null)
            {
                throw new DataNotFoundException();
            }

            return dbUnitType;
        }

        public static UnitsType Create(int? id, string code, string unitAr, string unitEN, string DefinitionAr, string DefinitionEn, string createdBy)
        {
            return new UnitsType
            {
                Id = id ?? 0,
                Code = code,
                UnitAr = unitAr,
                UnitEn = unitEN,
                DefinitionAr = DefinitionAr,
                DefinitionEn = DefinitionEn,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,

            };
        }

        private async Task<bool> EnsureNoDuplicates(IUnitsTypeRepository repository, bool throwException = true)
        {
            var dbUnitType = await repository.Search(x => x.Id == Id || x.Code == Code, 1, 1, false);
            if (Id == default)
            {
                if (dbUnitType.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbUnitType.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
