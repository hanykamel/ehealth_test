using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.Queries;
using EHealth.ManageItemLists.Application.Lookups.UnitRooms.DTOs;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.UnitRooms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.UnitRooms.Queries.Handler
{
    public class UnitRoomsSearchQueryHandler : IRequestHandler<UnitRoomsSearchQuery, PagedResponse<UnitRoomDto>>
    {
        private readonly IUnitRoomRepository _unitRoomRepository;
        public UnitRoomsSearchQueryHandler(IUnitRoomRepository unitRoomRepository)
        {
            _unitRoomRepository = unitRoomRepository;
        }
        public async Task<PagedResponse<UnitRoomDto>> Handle(UnitRoomsSearchQuery request, CancellationToken cancellationToken)
        {
            var res = await UnitRoom.Search(_unitRoomRepository, f => f.IsDeleted == false, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<UnitRoomDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => UnitRoomDto.FromUnitRoom(s)).ToList()
            };
        }
    }
}
