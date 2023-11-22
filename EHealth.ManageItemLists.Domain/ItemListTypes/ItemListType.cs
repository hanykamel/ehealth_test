using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.ItemListTypes
{
    public class ItemListType : ItemManagmentBaseClass, IEntity<int>, IValidationModel<ItemListType>
    {
        private ItemListType()
        {
            
        }
        public string NameAr { get; private set; }
        public string NameEN { get; private set; }
        public IList<ItemListSubtype> ItemListSubtypes { get;private set; }=new List<ItemListSubtype>();
        public AbstractValidator<ItemListType> Validator =>  new ItemListTypeValidator();

        public static async Task<ItemListType> Get(int id, IItemListTypeRepository repository)
        {
            var dbItemListType = await repository.GetById(id);

            if (dbItemListType is null)
            {
                throw new DataNotFoundException();
            }

            return dbItemListType;
        }
        public static async Task<PagedResponse<ItemListType>> Search(IItemListTypeRepository repository, Expression<Func<ItemListType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }
    }
}
