using EHealth.ManageItemLists.Application.PackageHeaders.DTOs;
using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageHeaders.Queries.Handler
{
    public class PackageHeaderSearchQueryHandler : IRequestHandler<PackageHeaderSearchQuery, PagedResponse<PackageHeaderDTO>>
    {
        private readonly IPackageHeaderRepository _packageHeaderRepository;

        public PackageHeaderSearchQueryHandler(IPackageHeaderRepository packageHeaderRepository)
        {
            _packageHeaderRepository = packageHeaderRepository;
        }
        public async Task<PagedResponse<PackageHeaderDTO>> Handle(PackageHeaderSearchQuery request, CancellationToken cancellationToken)
        {
            var result = await PackageHeader.Search(_packageHeaderRepository,  p=>    

                (!string.IsNullOrEmpty(request.EHealthCode) ? p.EHealthCode.ToLower().Contains(request.EHealthCode.ToLower()) : true) &&
                (!string.IsNullOrEmpty(request.UHIACode) ? p.UHIACode.ToLower().Contains(request.UHIACode.ToLower()):true) &&
                (!string.IsNullOrEmpty(request.PackageNameAr)? p.NameAr.ToLower().Contains(request.PackageNameAr.ToLower()):true)&&
                (!string.IsNullOrEmpty(request.PackageNameEn) ? p.NameEn.ToLower().Contains(request.PackageNameEn.ToLower()) : true)&&
                (request.GlobalTypeId.HasValue?p.GlobelPackageType.Id == request.GlobalTypeId:true)&&
                (request.PackageTypeId.HasValue ? p.PackageType.Id == request.PackageTypeId : true)&&
                (request.PackageSubTypeId.HasValue ? p.PackageSubType.Id == request.PackageSubTypeId : true)&&
                (request.PackageSpecialityId.HasValue ? p.PackageSpecialty.Id == request.PackageSpecialityId : true)&&
                (request.packageComplexityClassificationId.HasValue ? p.PackageComplexityClassification.Id == request.packageComplexityClassificationId : true)
                , request.PageNo, request.PageSize,request.EnablePagination, request.OrderBy, request.Ascending
                
                );
            return new PagedResponse<PackageHeaderDTO>
            {
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                Data = result.Data.Select(d => PackageHeaderDTO.FromPackageHeader(d)).ToList()
            };
        }
    }
}
