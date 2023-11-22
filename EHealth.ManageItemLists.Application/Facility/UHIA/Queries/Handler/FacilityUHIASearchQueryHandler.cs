using EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.Queries.Handler
{
    public class FacilityUHIASearchQueryHandler : IRequestHandler<FacilityUHIASearchQuery, PagedResponse<FacilityUHIADto>>
    {
        private readonly IFacilityUHIARepository _facilityUHIARepository;
        public FacilityUHIASearchQueryHandler(IFacilityUHIARepository facilityUHIARepository)
        {
            _facilityUHIARepository = facilityUHIARepository;
        }
        public async Task<PagedResponse<FacilityUHIADto>> Handle(FacilityUHIASearchQuery request, CancellationToken cancellationToken)
        {
            var res = await FacilityUHIA.Search(_facilityUHIARepository,f =>
            f.ItemListId == request.ItemListId &&
            (!string.IsNullOrEmpty(request.Code) ? f.Code.ToLower().Contains(request.Code.ToLower()) : true)
            //&& (!string.IsNullOrEmpty(request.DescriptorAr) && !string.IsNullOrEmpty(f.DescriptorAr) ? f.DescriptorAr.ToLower().Contains(request.DescriptorAr.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.DescriptorAr) ? f.DescriptorAr.ToLower().Contains(request.DescriptorAr.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.DescriptorEn) ? f.DescriptorEn.ToLower().Contains(request.DescriptorEn.ToLower()) : true)
            //
            //&& f.IsDeleted != true
            , request.PageNo, request.PageSize,request.EnablePagination, request.OrderBy, request.Ascending);

            var data = res.Data.Select(s => FacilityUHIADto.FromFacilityUHIA(s)).ToList();

            return new PagedResponse<FacilityUHIADto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = data
            };
        }
    }
}
