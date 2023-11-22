using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using FluentValidation;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Categories
{
    public class Category :ItemManagmentBaseClass, IEntity<int>, IValidationModel<Category>
    {
        private Category()
        {
            //default value is Active
            this.Active = true;
        }
        
        public string? CategoryAr { get;private set; }
        public string? CategoryEn { get;private set; }
        public string? DefinitionAr { get;private set; }
        public string? DefinitionEn { get;private set; }
        public int ItemListSubtypeId { get; private set; }
        public ItemListSubtype ItemListSubtype { get; private set; }
        public IList<SubCategory> SubCategories { get; private set; } = new List<SubCategory>();
        public AbstractValidator<Category> Validator => new CategoryValidator();
     

        public async Task<int> Create(ICategoriesRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(ICategoriesRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(ICategoriesRepository repository)
        {
            return await repository.Delete(this);
        }

        public static async Task<PagedResponse<Category>> Search(ICategoriesRepository repository, Expression<Func<Category, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }

        public static async Task<Category> Get(int id, ICategoriesRepository repository)
        {
            var dbCategory = await repository.Get(id);

            if (dbCategory is null)
            {
                throw new DataNotFoundException();
            }

            return dbCategory;
        }

        public static Category Create(int? id, string code, string categoryAr, string categoryEn, string createdBy)
        {
            return new Category
            {
                Id = id ?? 0,
                Code = code,
                CategoryAr = categoryAr,
                CategoryEn = categoryEn,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
            };
        }

        private async Task<bool> EnsureNoDuplicates(ICategoriesRepository CategoriesRepository, bool throwException = true)
        {
            var dbCategory = await CategoriesRepository.Search(c => c.Id == Id && c.Code == Code && c.CategoryAr == CategoryAr && c.CategoryEn == CategoryEn, 1, 1, true);
            if (Id == default)
            {
                if (dbCategory.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbCategory.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
