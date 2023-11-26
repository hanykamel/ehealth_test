using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.PackageItems.DTOs;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
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
    public class GetAllDrugsQueryHandler : IRequestHandler<GetAllDrugsQuery, PagedResponse<GetAllDrugsDTO>>
    {
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        private readonly IPackageHeaderRepository _packageHeaderRepository;

        public GetAllDrugsQueryHandler(IDrugsUHIARepository drugsUHIARepository,IPackageHeaderRepository packageHeaderRepository)
        {
            _drugsUHIARepository = drugsUHIARepository;
            _packageHeaderRepository = packageHeaderRepository;
        }
        public async Task<PagedResponse<GetAllDrugsDTO>> Handle(GetAllDrugsQuery request, CancellationToken cancellationToken)
        {
            var packageDate = await PackageHeader.Get(request.PackageHeaderId, _packageHeaderRepository);
            var DateRange = new DateRangeDto()
            {
                Start = packageDate.ActivationDateFrom.Date,

                End = packageDate.ActivationDateTo.HasValue ? packageDate.ActivationDateTo.Value.Date : null
            };
            var result  = await DrugUHIA.Search(_drugsUHIARepository, f=> f.IsDeleted!=null &&
            (!string.IsNullOrEmpty(request.ProprietaryName)? f.ProprietaryName.ToLower().Contains(request.ProprietaryName.ToLower()):true)&&
            (!string.IsNullOrEmpty(request.EHealthDrugCode) ? f.EHealthDrugCode.ToLower().Contains(request.EHealthDrugCode.ToLower()) : true)&&
            (!string.IsNullOrEmpty(request.LocalDrugCode) ? f.LocalDrugCode.ToLower().Contains(request.LocalDrugCode.ToLower()) : true)
            , request.PageNo, request.PageSize, request.EnablePagination, request.OrderBy, request.Ascending);

            result.Data = result.Data.
                Where(f => (!DateAndTimeOperations.DoesNotOverlap(DateRange, new DateRangeDto()
                {
                    Start = f.DataEffectiveDateFrom.Date,

                    End = f.DataEffectiveDateTo.HasValue ? f.DataEffectiveDateTo.Value.Date : null
                }))).ToList();

            var data = result.Data.Select(s=>GetAllDrugsDTO.FromGetAllDrugsDTO(s)).ToList();

            return new PagedResponse<GetAllDrugsDTO>
            {
                PageNumber = result.PageNumber,
                TotalCount = result.TotalCount,
                PageSize = result.PageSize,
                Data = data
            };
        }
    }
}
