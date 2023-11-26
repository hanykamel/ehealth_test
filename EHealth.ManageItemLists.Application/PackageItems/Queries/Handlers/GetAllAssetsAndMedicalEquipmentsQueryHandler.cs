using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.PackageItems.DTOs;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
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
    public class GetAllAssetsAndMedicalEquipmentsQueryHandler : IRequestHandler<GetAllAssetsAndMedicalEquipmentsQuery, PagedResponse<GetAllAssetsAndMedicalEquipmentsDTO>>
    {
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        private readonly IPackageHeaderRepository _packageHeaderRepository;

        public GetAllAssetsAndMedicalEquipmentsQueryHandler(IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository,IPackageHeaderRepository packageHeaderRepository)
        {
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
            _packageHeaderRepository = packageHeaderRepository;
        }
        public async Task<PagedResponse<GetAllAssetsAndMedicalEquipmentsDTO>> Handle(GetAllAssetsAndMedicalEquipmentsQuery request, CancellationToken cancellationToken)
        {
            var packageDate = await PackageHeader.Get(request.PackageHeaderId, _packageHeaderRepository);
            var DateRange = new DateRangeDto()
            {
                Start = packageDate.ActivationDateFrom.Date,

                End = packageDate.ActivationDateTo.HasValue ? packageDate.ActivationDateTo.Value.Date : null
            };
            var res = await DevicesAndAssetsUHIA.Search(_devicesAndAssetsUHIARepository, f =>
            f.IsDeleted != true
            && (!string.IsNullOrEmpty(request.DescriptorAr) ? f.DescriptorAr.ToLower().Contains(request.DescriptorAr.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.DescriptorEn) ? f.DescriptorEn.ToLower().Contains(request.DescriptorEn.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.Code) ? f.Code.ToLower() == request.Code.ToLower() : true)
            , request.PageNo, request.PageSize, request.EnablePagination, request.OrderBy, request.Ascending);

            res.Data = res.Data.
                Where(f => (!DateAndTimeOperations.DoesNotOverlap(DateRange, new DateRangeDto()
                {
                    Start = f.DataEffectiveDateFrom.Date,

                    End = f.DataEffectiveDateTo.HasValue ? f.DataEffectiveDateTo.Value.Date : null
                }))).ToList();

            var data = res.Data.Select(s => GetAllAssetsAndMedicalEquipmentsDTO.FromAssetsAndMedicalEquipment(s)).ToList();


            return new PagedResponse<GetAllAssetsAndMedicalEquipmentsDTO>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = data
            };
        }
    }
}
