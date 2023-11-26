using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.PackageItems.DTOs;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageItems.Queries.Handlers
{
    internal class GetAllResourceItemsQueryHandler : IRequestHandler<GetAllResourceItemsQuery, PagedResponse<GetAllResourceItemsDTO>>
    {
        private readonly IResourceUHIARepository _resourceUHIARepository;
        private readonly IPackageHeaderRepository _packageHeaderRepository;

        public GetAllResourceItemsQueryHandler(IResourceUHIARepository resourceUHIARepository,IPackageHeaderRepository packageHeaderRepository)
        {
            _resourceUHIARepository = resourceUHIARepository;
            _packageHeaderRepository = packageHeaderRepository;
        }
        public async Task<PagedResponse<GetAllResourceItemsDTO>> Handle(GetAllResourceItemsQuery request, CancellationToken cancellationToken)
        {
            var packageDate = await PackageHeader.Get(request.PackageHeaderId, _packageHeaderRepository);
            var DateRange = new DateRangeDto()
            {
                Start = packageDate.ActivationDateFrom.Date,

                End = packageDate.ActivationDateTo.HasValue ? packageDate.ActivationDateTo.Value.Date : null
            };
            var result = await ResourceUHIA.Search(_resourceUHIARepository, f =>
            f.IsDeleted!= true &&
            (!string.IsNullOrEmpty(request.ItemEn) ? f.DescriptorEn.ToLower().Contains(request.ItemEn.ToLower()) : true) &&
            (!string.IsNullOrEmpty(request.ItemAr) ? f.DescriptorAr.ToLower().Contains(request.ItemAr.ToLower()) : true) &&
            (!string.IsNullOrEmpty(request.Code) ? f.Code.ToLower().Contains(request.Code.ToLower()) : true)
            , request.PageNo, request.PageSize, request.EnablePagination, request.OrderBy, request.Ascending);

            result.Data = result.Data.
            Where(f => (!DateAndTimeOperations.DoesNotOverlap(DateRange, new DateRangeDto()
            {
                Start = f.DataEffectiveDateFrom.Date,

                End = f.DataEffectiveDateTo.HasValue ? f.DataEffectiveDateTo.Value.Date : null
            }))).ToList();

            var data = result.Data.Select(s => GetAllResourceItemsDTO.FromGetAllResourceItems(s)).ToList();

            return new PagedResponse<GetAllResourceItemsDTO>
            {
                PageNumber = result.PageNumber,
                TotalCount = result.TotalCount,
                PageSize = result.PageSize,
                Data = data
            };
        }
    }
}
