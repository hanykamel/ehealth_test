using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Sub_Categories
{
    public class SubCategory : ItemManagmentBaseClass, IEntity<int>, IValidationModel<SubCategory>
    {
        private SubCategory()
        {
            //default value is Active
            this.Active = true;
        }

        public string SubCategoryAr { get; set; }
        public string SubCategoryEn { get; set; }
        public string? DefinitionAr { get; set; }
        public string? DefinitionEn { get; set; }
        public int CategoryId { get; private set; }
        public Category Category { get; private set; }
        public int ItemListSubtypeId { get; private set; }
        public ItemListSubtype ItemListSubtype { get; private set; }
        public AbstractValidator<SubCategory> Validator => new SubCategoryValidator();
 

        public async Task<int> Create(ISubCategoriesRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(ISubCategoriesRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(ISubCategoriesRepository repository)
        {
            return await repository.Delete(this);
        }

        public static async Task<PagedResponse<SubCategory>> Search(ISubCategoriesRepository repository, Expression<Func<SubCategory, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }

        public static async Task<SubCategory> Get(int id, ISubCategoriesRepository repository)
        {
            var dbSubCategory = await repository.Get(id);

            if (dbSubCategory is null)
            {
                throw new DataNotFoundException();
            }

            return dbSubCategory;
        }

        public static SubCategory Create(int? id, string code, string subCategoryAr, string subCategoryEn, int categoryId, string createdBy)
        {
            return new SubCategory
            {
                Id = id ?? 0,
                Code = code,
                SubCategoryAr = subCategoryAr,
                SubCategoryEn = subCategoryEn,
                CategoryId = categoryId,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
            };
        }

        private async Task<bool> EnsureNoDuplicates(ISubCategoriesRepository SubCategoriesRepository, bool throwException = true)
        {
            var dbCategory = await SubCategoriesRepository.Search(s => s.Id == Id && s.Code == Code && s.SubCategoryAr == SubCategoryAr && s.SubCategoryEn == SubCategoryEn, 1, 1, true);
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
