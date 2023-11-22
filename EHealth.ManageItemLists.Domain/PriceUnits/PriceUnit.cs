using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.PriceUnits
{
    public class PriceUnit : ItemManagmentBaseClass, IEntity<int>, IValidationModel<PriceUnit>
    {
        private PriceUnit()
        {
            //default value is Active
            this.Active = true;
        }
        public string NameAr { get; private set; }
        public string NameEN { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEN { get; private set; }

        public int ResourceUnitOfCostValue { get; set; }

        public AbstractValidator<PriceUnit> Validator => new PriceUnitValidator();

        public async Task<int> Create(IPriceUnitRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.CreatePriceUnit(this);
        }

        public async Task<bool> Update(IPriceUnitRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.UpdatePriceUnit(this);
        }

        public async Task<bool> Delete(IPriceUnitRepository repository)
        {
            return await repository.DeletePriceUnit(this);
        }

        public static async Task<PagedResponse<PriceUnit>> Search(IPriceUnitRepository repository, Expression<Func<PriceUnit, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }

        public static async Task<PriceUnit> Get(int id, IPriceUnitRepository repository)
        {
            var dbPriceUnit = await repository.Get(id);

            if (dbPriceUnit is null)
            {
                throw new DataNotFoundException();
            }

            return dbPriceUnit;
        }

        public static PriceUnit Create(int? id, string code, string nameAr, string nameEN, string DefinitionAr, string DefinitionEN, int resourceUnitOfCostValue, string createdBy)
        {
            return new PriceUnit
            {
                Id = id ?? 0,
                Code = code,
                NameAr = nameAr,
                NameEN = nameEN,
                DefinitionAr = DefinitionAr,
                DefinitionEN = DefinitionEN,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                ResourceUnitOfCostValue = resourceUnitOfCostValue
            };
        }

        private async Task<bool> EnsureNoDuplicates(IPriceUnitRepository repository, bool throwException = true)
        {
            var dbPriceUnit = await repository.Search( x => x.Code == Code && x.NameAr == NameAr && x.NameEN == NameEN
            && x.DefinitionAr == DefinitionAr && x.DefinitionEN == DefinitionEN , 1, 1, false);
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
