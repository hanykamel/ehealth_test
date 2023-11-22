using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Queries;
using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Queries.Handlers
{
    public class ProcedureICHISearchQueryHandler : IRequestHandler<ProcedureICHISearchQuery, PagedResponse<ProcedureICHIDto>>
    {
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        public ProcedureICHISearchQueryHandler(IProcedureICHIRepository procedureICHIRepository)
        {
            _procedureICHIRepository = procedureICHIRepository;
        }
        public async Task<PagedResponse<ProcedureICHIDto>> Handle(ProcedureICHISearchQuery request, CancellationToken cancellationToken)
        {
            //var res = await ProcedureICHI.Search(_procedureICHIRepository, f => f.ItemListId == request.ItemListId &&
            var res = await ProcedureICHI.Search(_procedureICHIRepository, f => f.ItemListId == request.ItemListId && 
            //f.IsDeleted != true &&
            (!string.IsNullOrEmpty(request.EHealthCode) ? f.EHealthCode.ToLower().Contains(request.EHealthCode.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.UHIAId) ? f.UHIAId.ToLower().Contains(request.UHIAId.ToLower()) : true)
            //&& (!string.IsNullOrEmpty(request.TitleAr) && !string.IsNullOrEmpty(f.TitleAr) ? f.TitleAr.ToLower().Contains(request.TitleAr.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.TitleAr) ? f.TitleAr.ToLower().Contains(request.TitleAr.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.TitleEn) ? f.TitleEn.ToLower().Contains(request.TitleEn.ToLower()) : true)
            , request.PageNo, request.PageSize,request.EnablePagination, request.OrderBy, request.Ascending);

            var data = res.Data.Select(s => ProcedureICHIDto.FromProcedureICHI(s)).ToList();

            return new PagedResponse<ProcedureICHIDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = data
            };
        }
    }
}
