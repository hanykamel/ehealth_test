using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.LocalUnitOfMeasure;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.LocalTypeOfMeasure
{
    public class UnitOfMeasure : ItemManagmentBaseClass, IEntity<int>, IValidationModel<UnitOfMeasure>
    {
        private UnitOfMeasure()
        {   //default value is Active
            this.Active = true;
        }
 
        public string? MeasureTypeAr { get;private set; }
        public string? MeasureTypeENG { get;private set; }
        public string? DefinitionAr   { get;private set; }
        public string? DefinitionENG { get;private set; }
    
        public AbstractValidator<UnitOfMeasure> Validator => new UnitOfMeasureValidator();
        AbstractValidator<UnitOfMeasure> IValidationModel<UnitOfMeasure>.Validator => throw new NotImplementedException();
        public async Task<int> Create(IUnitOfMeasureRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.CreateUnitOfMeasure(this);
        }

        public async Task<bool> Update(IUnitOfMeasureRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.UpdateUnitOfMeasure(this);
        }

        public async Task<bool> Delete(IUnitOfMeasureRepository repository)
        {
            return await repository.DeleteUnitOfMeasure(this);
        }

        public static async Task<PagedResponse<UnitOfMeasure>> Search(IUnitOfMeasureRepository repository, Expression<Func<UnitOfMeasure, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }


        public static async Task<UnitOfMeasure> Get(int id, IUnitOfMeasureRepository repository)
        {
            var dbUnitOfMeasure = await repository.Get(id);

            if (dbUnitOfMeasure is null)
            {
                throw new DataNotFoundException();
            }

            return dbUnitOfMeasure;
        }

        public static UnitOfMeasure Create(int? id, string code, string? MeasureTypeAr, string? MeasureTypeENG, string? DefinitionAr, string? DefinitionENG, string createdBy)
        {
            return new UnitOfMeasure
            {
                Id = id ?? 0,
                Code = code,
                MeasureTypeAr= MeasureTypeAr,
                MeasureTypeENG = MeasureTypeENG,
                DefinitionAr = DefinitionAr,
                DefinitionENG = DefinitionENG,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,

            };
        }

        private async Task<bool> EnsureNoDuplicates(IUnitOfMeasureRepository repository, bool throwException = true)
        {
            var dbUnitOfMeasure = await repository.Search(x => x.IsDeleted != true, 1, 1, false);
            if (Id == default)
            {
                if (dbUnitOfMeasure.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbUnitOfMeasure.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }

    }
}





 