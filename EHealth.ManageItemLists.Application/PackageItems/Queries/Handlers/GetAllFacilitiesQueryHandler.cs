using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using NPOI.Util;

namespace EHealth.ManageItemLists.Application.PackageItems.Queries.Handlers
{
    public class GetAllFacilitiesQueryHandler : IRequestHandler<GetAllFacilitiesQuery, PagedResponse<GetAllFacilitiesDTO>>
    {
        private readonly IFacilityUHIARepository _facilityUHIARepository;
        private readonly IPackageHeaderRepository _packageHeaderRepository;

        public GetAllFacilitiesQueryHandler(IFacilityUHIARepository facilityUHIARepository,IPackageHeaderRepository packageHeaderRepository)
        {
            _facilityUHIARepository = facilityUHIARepository;
            _packageHeaderRepository = packageHeaderRepository;
        }
        public async Task<PagedResponse<GetAllFacilitiesDTO>> Handle(GetAllFacilitiesQuery request, CancellationToken cancellationToken)
        {
            var packageDate = await PackageHeader.Get(request.PackageHeaderId, _packageHeaderRepository);
            var DateRange = new DateRangeDto()
            {
                Start = packageDate.ActivationDateFrom.Date,

                End = packageDate.ActivationDateTo.HasValue ? packageDate.ActivationDateTo.Value.Date : null
            };

            var res =( await FacilityUHIA.Search(_facilityUHIARepository, f =>
             f.IsDeleted != true
            && (!string.IsNullOrEmpty(request.DescriptorAr) ? f.DescriptorAr.ToLower().Contains(request.DescriptorAr.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.DescriptorEn) ? f.DescriptorEn.ToLower().Contains(request.DescriptorEn.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.Code) ? f.Code.ToLower() == request.Code.ToLower() : true)
            , request.PageNo, request.PageSize, request.EnablePagination, request.OrderBy, request.Ascending));


            res.Data = res.Data.
            Where(f => (!DateAndTimeOperations.DoesNotOverlap(DateRange, new DateRangeDto()
            {
                Start = f.DataEffectiveDateFrom.Date,

                End = f.DataEffectiveDateTo.HasValue ? f.DataEffectiveDateTo.Value.Date : null
            }))).ToList();

            var data = res.Data.Select(s => GetAllFacilitiesDTO.FromFacilityUHIA(s)).ToList();

            
            return new PagedResponse<GetAllFacilitiesDTO>
            {

                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = data ,


            };
        }
    }
}
