using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageType.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Queries;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using MediatR;
using System.Xml.Linq;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Queries.Handlers
{
    public class DrugUHIASearchQueryHandler : IRequestHandler<DrugUHIASearchQuery, PagedResponse<DrugsUHIADto>>
    {
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        public DrugUHIASearchQueryHandler(IDrugsUHIARepository drugsUHIARepository)
        {
            _drugsUHIARepository = drugsUHIARepository;
        }
        public async Task<PagedResponse<DrugsUHIADto>> Handle(DrugUHIASearchQuery request, CancellationToken cancellationToken)
        {
            var res = await DrugUHIA.Search(_drugsUHIARepository, f => f.ItemListId == request.ItemListId &&
            (!string.IsNullOrEmpty(request.EHealthCode) ? f.EHealthDrugCode.ToLower().Contains(request.EHealthCode.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.LocalDrugCode) && !string.IsNullOrEmpty(f.LocalDrugCode) ? f.LocalDrugCode.ToLower().Contains(request.LocalDrugCode.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.InternationalNonProprietaryName) ? f.InternationalNonProprietaryName.ToLower().Contains(request.InternationalNonProprietaryName.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.ProprietaryName) ? f.ProprietaryName.ToLower().Contains(request.ProprietaryName.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.DosageForm) ? f.DosageForm.ToLower().Contains(request.DosageForm.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.RouteOfAdministration) ? f.RouteOfAdministration.ToLower().Contains(request.RouteOfAdministration.ToLower()) : true)
            
            , request.PageNo, request.PageSize, true, request.OrderBy, request.Ascending);

            var data = res.Data.Select(s => DrugsUHIADto.FromDrugsUHIA(s)).ToList();

            return new PagedResponse<DrugsUHIADto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = data
            };
        }
    }
}
