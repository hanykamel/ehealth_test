using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.ItemLists.Queries.Handlers
{
    public class GetItemListByIdHandler : IRequestHandler<GetItemListByIdQuery, ItemListDto>
    {
        private readonly IItemListRepository _itemListsRepository;
        public GetItemListByIdHandler(IItemListRepository itemListsRepository)
        {
            _itemListsRepository = itemListsRepository;
        }
        public async Task<ItemListDto> Handle(GetItemListByIdQuery request, CancellationToken cancellationToken)
        {
            var output = await ItemList.Get(request.Id, _itemListsRepository);

            return ItemListDto.FromItemList(output);

        }
    }
}
