using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.PackageItems.DTOs;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageItems.Queries.Handlers
{
    public class GetAllConsumablesAndDevicesQueryHandler : IRequestHandler<GetAllConsumablesAndDevicesQuery, PagedResponse<GetAllConsumablesAndDevicesDTO>>
    {
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        private readonly IPackageHeaderRepository _packageHeaderRepository;

        public GetAllConsumablesAndDevicesQueryHandler(IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository, IPackageHeaderRepository packageHeaderRepository)
        {
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;
            _packageHeaderRepository = packageHeaderRepository;
        }
        public async Task<PagedResponse<GetAllConsumablesAndDevicesDTO>> Handle(GetAllConsumablesAndDevicesQuery request, CancellationToken cancellationToken)
        {

            var packageDate = await PackageHeader.Get(request.PackageHeaderId, _packageHeaderRepository);
            var DateRange = new DateRangeDto()
            {
                Start = packageDate.ActivationDateFrom.Date,

                End = packageDate.ActivationDateTo.HasValue ? packageDate.ActivationDateTo.Value.Date : null
            };
            var result = await ConsumablesAndDevicesUHIA.Search(_consumablesAndDevicesUHIARepository , f=>
                        f.IsDeleted!= true&&
                (!string.IsNullOrEmpty(request.eHealthCode) ? f.EHealthCode.ToLower().Contains(request.eHealthCode.ToLower()) : true) &&
                (!string.IsNullOrEmpty(request.UHIAId) ? f.UHIAId.ToLower().Contains(request.UHIAId.ToLower()) : true)&&
                (!string.IsNullOrEmpty(request.ItemNameAr) ? f.ShortDescriptorAr.ToLower().Contains(request.ItemNameAr.ToLower()) : true)&&
                (!string.IsNullOrEmpty(request.ItemNameEn) ? f.ShortDescriptorEn.ToLower().Contains(request.ItemNameEn.ToLower()) : true),
                    request.PageNo, request.PageSize, request.EnablePagination, request.OrderBy, request.Ascending);


            result.Data = result.Data.
                Where(f => (!DateAndTimeOperations.DoesNotOverlap(DateRange, new DateRangeDto()
                {
                    Start = f.DataEffectiveDateFrom.Date,

                    End = f.DataEffectiveDateTo.HasValue ? f.DataEffectiveDateTo.Value.Date : null
                }))).ToList();


            var data = result.Data.Select(s=> GetAllConsumablesAndDevicesDTO.FromGetAllConsumablesAndDevicesDTO(s)).ToList();

            return new PagedResponse<GetAllConsumablesAndDevicesDTO>
            {
                PageNumber = result.PageNumber,
                TotalCount = result.TotalCount,
                PageSize = result.PageSize,
                Data = data
            };

                
        }
    }
}
