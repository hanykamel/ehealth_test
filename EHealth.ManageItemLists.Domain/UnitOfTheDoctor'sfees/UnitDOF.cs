using EHealth.ManageItemLists.Domain.PriceUnits;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.UnitOfTheDoctor_sfees
{
    public class UnitDOF:ItemManagmentBaseClass,IEntity<int>,IValidationModel<UnitDOF>
    {
        public UnitDOF()
        {
            //default value is Active
            this.Active = true;
        }
        public string NameAr { get; private set; }
        public string NameEN { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEN { get; private set; }
        public AbstractValidator<UnitDOF> Validator => new UnitDOFValidator();
        public async Task<int> Create(IUnitDOFRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.CreateUnitDOF(this);
        }

        public async Task<bool> Update(IUnitDOFRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.UpdateUnitDOF(this);
        }

        public async Task<bool> Delete(IUnitDOFRepository repository)
        {
            return await repository.DeleteUnitDOF(this);
        }

        public static async Task<PagedResponse<UnitDOF>> Search(IUnitDOFRepository repository, Expression<Func<UnitDOF, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }

        public static async Task<UnitDOF> Get(int id, IUnitDOFRepository repository)
        {
            var dbUnitDOF = await repository.Get(id);

            if (dbUnitDOF is null)
            {
                throw new DataNotFoundException();
            }

            return dbUnitDOF;
        }

        public static UnitDOF Create(int? id, string code, string nameAr, string nameEN, string DefinitionAr, string DefinitionEN, string createdBy)
        {
            return new UnitDOF
            {
                Id = id ?? 0,
                Code = code ,
                NameAr = nameAr,
                NameEN = nameEN,
                DefinitionAr = DefinitionAr,
                DefinitionEN = DefinitionEN,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,

            };
        }

        private async Task<bool> EnsureNoDuplicates(IUnitDOFRepository repository, bool throwException = true)
        {
            var dbUnitDOF = await repository.Search(u => u.Id == Id && u.Code == Code && u.NameAr == NameAr && u.NameEN == NameEN && u.DefinitionAr == DefinitionAr && u.DefinitionEN == DefinitionEN && u.IsDeleted != true, 1,1,true);
            if (Id == default)
            {
                if (dbUnitDOF.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbUnitDOF.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
