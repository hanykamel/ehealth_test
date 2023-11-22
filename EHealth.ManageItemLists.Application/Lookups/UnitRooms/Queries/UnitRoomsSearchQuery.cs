using EHealth.ManageItemLists.Application.Lookups.UnitRooms.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;

namespace EHealth.ManageItemLists.Application.Lookups.UnitRooms.Queries
{
    public class UnitRoomsSearchQuery : LookupPagedRequerst, IRequest<PagedResponse<UnitRoomDto>>
    {

    }
}
