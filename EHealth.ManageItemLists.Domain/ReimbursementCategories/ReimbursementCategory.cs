using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Pre_authorizationProtocol;
using EHealth.ManageItemLists.Domain.ReimbursementCategories;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.ReimbursementCategories
{
    public class ReimbursementCategory : ItemManagmentBaseClass, IEntity<int>, IValidationModel<ReimbursementCategory>
    {
        private ReimbursementCategory()
        {
            //default value is Active
            this.Active = true;
        }
        public string NameAr { get; private set; }
        public string NameENG { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionENG { get; private set; }

        public AbstractValidator<ReimbursementCategory> Validator => new ReimbursementCategoryValidator();
        AbstractValidator<ReimbursementCategory> IValidationModel<ReimbursementCategory>.Validator => throw new NotImplementedException();
        public async Task<int> Create(IReimbursementCategoryRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.CreateReimbursementCategory(this);
        }

        public async Task<bool> Update(IReimbursementCategoryRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.UpdateReimbursementCategory(this);
        }

        public async Task<bool> Delete(IReimbursementCategoryRepository repository)
        {
            return await repository.DeleteReimbursementCategory(this);
        }

        public static async Task<PagedResponse<ReimbursementCategory>> Search(IReimbursementCategoryRepository repository, Expression<Func<ReimbursementCategory, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }

        public static async Task<ReimbursementCategory> Get(int id, IReimbursementCategoryRepository repository)
        {
            var dbReimbursementCategory = await repository.Get(id);

            if (dbReimbursementCategory is null)
            {
                throw new DataNotFoundException();
            }

            return dbReimbursementCategory;
        }

        public static ReimbursementCategory Create(int? id, string code, string nameAr, string nameENG, string? DefinitionAr, string? DefinitionENG, string createdBy)
        {
            return new ReimbursementCategory
            {
                Id = id ?? 0,
                Code = code ,
                NameAr = nameAr,
                NameENG = nameENG,
                DefinitionAr = DefinitionAr,
                DefinitionENG = DefinitionENG,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,

            };
        }

        private async Task<bool> EnsureNoDuplicates(IReimbursementCategoryRepository repository, bool throwException = true)
        {
            var dbReimbursementCategory = await repository.Search( x => x.Id == Id || x.Code == Code, 1, 1, false);
            if (Id == default)
            {
                if (dbReimbursementCategory.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbReimbursementCategory.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }

    }
}
