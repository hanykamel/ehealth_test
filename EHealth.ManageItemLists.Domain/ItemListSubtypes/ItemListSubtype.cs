using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.ItemListTypes;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.ItemListSubtypes
{
    public class ItemListSubtype : ItemManagmentBaseClass, IEntity<int>, IValidationModel<ItemListSubtype>
    {
        private ItemListSubtype()
        {
            
        }
        public string NameAr { get; private set; }
        public string NameEN { get; private set; }
        public int ItemListTypeId { get; private set; }

        public ItemListType ItemListType { get; private set; }
        public IList<ItemList> ItemLists { get; private set; } = new List<ItemList>();
        public AbstractValidator<ItemListSubtype> Validator => new ItemListSubtypeValidator();
        public static async Task<ItemListSubtype> Get(int id, IItemListSubtypeRepository repository)
        {
            var dbItemListSubtype = await repository.GetById(id);

            if (dbItemListSubtype is null)
            {
                throw new DataNotFoundException();
            }

            return dbItemListSubtype;
        }

        public static async Task<PagedResponse<ItemListSubtype>> Search(IItemListSubtypeRepository repository, Expression<Func<ItemListSubtype, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber,  pageSize,enablePagination);
        }
    }
}
