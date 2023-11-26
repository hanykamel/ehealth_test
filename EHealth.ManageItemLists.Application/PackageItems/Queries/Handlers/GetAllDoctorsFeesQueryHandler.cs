using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.PackageItems.DTOs;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
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
    public class GetAllDoctorsFeesQueryHandler : IRequestHandler<GetAllDoctorsFeesQuery, PagedResponse<GetAllDoctorsFeesDTO>>
    {
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        private readonly IPackageHeaderRepository _packageHeaderRepository;

        public GetAllDoctorsFeesQueryHandler(IDoctorFeesUHIARepository doctorFeesUHIARepository, IPackageHeaderRepository packageHeaderRepository)
        {
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
            _packageHeaderRepository = packageHeaderRepository;
        }

        public async Task<PagedResponse<GetAllDoctorsFeesDTO>> Handle(GetAllDoctorsFeesQuery request, CancellationToken cancellationToken)
        {
            var packageDate = await PackageHeader.Get(request.PackageHeaderId, _packageHeaderRepository);
            var DateRange = new DateRangeDto()
            {
                Start = packageDate.ActivationDateFrom.Date,

                End = packageDate.ActivationDateTo.HasValue ? packageDate.ActivationDateTo.Value.Date : null
            };
            var result = await DoctorFeesUHIA.Search(_doctorFeesUHIARepository, f=> f.IsDeleted != null &&
            
            (!string.IsNullOrEmpty(request.ItemEn) ? f.DescriptorEn.ToLower().Contains(request.ItemEn.ToLower()) : true) &&
            (!string.IsNullOrEmpty(request.ItemAr) ? f.DescriptorAr.ToLower().Contains(request.ItemAr.ToLower()) : true) &&
            (!string.IsNullOrEmpty(request.Code) ? f.Code.ToLower().Contains(request.Code.ToLower()) : true)&&
            (request.ComplexityClassificationCode.HasValue ? f.PackageComplexityClassificationId == request.ComplexityClassificationCode : true)
            , request.PageNo, request.PageSize, request.EnablePagination, request.OrderBy, request.Ascending);


            result.Data = result.Data.
                Where(f => (!DateAndTimeOperations.DoesNotOverlap(DateRange, new DateRangeDto()
                {
                    Start = f.DataEffectiveDateFrom.Date,

                    End = f.DataEffectiveDateTo.HasValue ? f.DataEffectiveDateTo.Value.Date : null
                }))).ToList();

            var data = result.Data.Select(s=> GetAllDoctorsFeesDTO.FromGetAllDoctorsFees(s)).ToList();

            return new PagedResponse<GetAllDoctorsFeesDTO>
            {
                PageNumber = result.PageNumber,
                TotalCount = result.TotalCount,
                PageSize = result.PageSize,
                Data = data
            };
        }
    }
}
