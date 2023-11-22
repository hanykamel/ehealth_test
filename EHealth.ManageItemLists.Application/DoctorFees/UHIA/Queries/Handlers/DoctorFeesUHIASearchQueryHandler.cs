using EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Queries.Handlers
{
    public class DoctorFeesUHIASearchQueryHandler : IRequestHandler<DoctorFeesUHIASearchQuery, PagedResponse<DoctorFeesUHIADto>>
    {
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        public DoctorFeesUHIASearchQueryHandler(IDoctorFeesUHIARepository doctorFeesUHIARepository)
        {
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
        }
        public async Task<PagedResponse<DoctorFeesUHIADto>> Handle(DoctorFeesUHIASearchQuery request, CancellationToken cancellationToken)
        {
            var res = await DoctorFeesUHIA.Search(_doctorFeesUHIARepository, f =>
            f.ItemListId == request.ItemListId &&
            (!string.IsNullOrEmpty(request.EHealthCode) ? f.Code.ToLower().Contains(request.EHealthCode.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.DescriptorEn) ? f.DescriptorEn.ToLower().Contains(request.DescriptorEn.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.DescriptorAr) ? f.DescriptorAr.ToLower().Contains(request.DescriptorAr.ToLower()) : true)
            //&& (!string.IsNullOrEmpty(request.DescriptorAr) && f.DescriptorAr != null ? f.DescriptorAr.ToLower().Contains(request.DescriptorAr.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.ComplexityClassificationCode) && f.PackageComplexityClassification != null ? f.PackageComplexityClassification.Code.ToLower().Contains(request.ComplexityClassificationCode.ToLower()) : true)
            //
            //&& f.IsDeleted != true
            , request.PageNo, request.PageSize,request.EnablePagination, request.OrderBy, request.Ascending);

            var data = res.Data.Select(s => DoctorFeesUHIADto.FromDoctorFeesUHIA(s)).ToList();

            return new PagedResponse<DoctorFeesUHIADto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = data
            };
        }
    }
}
