using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using MediatR;

namespace EHealth.ManageItemLists.Application.ItemLists.Queries
{

    public record GetItemListByIdQuery(int Id) : IRequest<ItemListDto>;

}
