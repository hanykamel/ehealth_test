using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.DTOs;
using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.Queries;
using EHealth.ManageItemLists.Application.Lookups.RegistrationTypes.DTOs;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.RegistrationTypes;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.RegistrationTypes.Queries.Handlers
{
    public class RegistrationTypesSearchQueryHandler : IRequestHandler<RegistrationTypesSearchQuery, PagedResponse<RegistrationTypeDto>>
    {
        private readonly IRegistrationRepository _registrationRepository;
        public RegistrationTypesSearchQueryHandler(IRegistrationRepository registrationTypeRepository)
        {
            _registrationRepository = registrationTypeRepository;
        }
        public async Task<PagedResponse<RegistrationTypeDto>> Handle(RegistrationTypesSearchQuery request, CancellationToken cancellationToken)
        {
            var res = await RegistrationType.Search(_registrationRepository, f => f.IsDeleted == false, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<RegistrationTypeDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => RegistrationTypeDto.FromRegistrationType(s)).ToList()
            };
        }
    }
}
