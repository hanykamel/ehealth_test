using EHealth.ManageItemLists.Application.Resource.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Queries.Handler
{
    public class ServiceUHIASearchQueryHandler : IRequestHandler<ServiceUHIASearchQuery, PagedResponse<ServiceUHIADto>>
    {
        private readonly IServiceUHIARepository _serviceUHIARepository;
        public ServiceUHIASearchQueryHandler(IServiceUHIARepository serviceUHIARepository)
        {

            _serviceUHIARepository = serviceUHIARepository;
        }
        public async Task<PagedResponse<ServiceUHIADto>> Handle(ServiceUHIASearchQuery request, CancellationToken cancellationToken)
        {
            // var res = await ServiceUHIA.Search(_serviceUHIARepository, f => f.ItemListId == request.ItemListId &&
            var res = await ServiceUHIA.Search(_serviceUHIARepository, f => f.ItemListId == request.ItemListId && 
            //f.IsDeleted != true &&
            (!string.IsNullOrEmpty(request.EHealthCode) ? f.EHealthCode.ToLower().Contains(request.EHealthCode.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.UHIAId) ? f.UHIAId.ToLower().Contains(request.UHIAId.ToLower()) : true)
            //&& (!string.IsNullOrEmpty(request.ShortDescriptionAr) && !string.IsNullOrEmpty(f.ShortDescAr) ? f.ShortDescAr.ToLower().Contains(request.ShortDescriptionAr.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.ShortDescriptionAr) ? f.ShortDescAr.ToLower().Contains(request.ShortDescriptionAr.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.ShortDescriptionEn) ? f.ShortDescEn.ToLower().Contains(request.ShortDescriptionEn.ToLower()) : true)
            , request.PageNo, request.PageSize,request.EnablePagination, request.OrderBy, request.Ascending);

            return new PagedResponse<ServiceUHIADto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => ServiceUHIADto.FromServiceUHIA(s)).ToList()
            };
        }
    }
}
