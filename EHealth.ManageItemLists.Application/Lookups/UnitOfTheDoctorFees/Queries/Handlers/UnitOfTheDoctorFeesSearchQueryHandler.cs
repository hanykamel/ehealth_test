using EHealth.ManageItemLists.Application.Lookups.PriceUnits.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PriceUnits.Queries;
using EHealth.ManageItemLists.Application.Lookups.UnitOfTheDoctorFees.DTOs;
using EHealth.ManageItemLists.Domain.PriceUnits;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.UnitOfTheDoctor_sfees;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.UnitOfTheDoctorFees.Queries.Handlers
{
    public class UnitOfTheDoctorFeesSearchQueryHandler : IRequestHandler<UnitOfTheDoctorFeesSearchQuery, PagedResponse<UnitOfTheDoctorFeesDto>>
    {
        private readonly IUnitDOFRepository _unitDOFRepository;
        public UnitOfTheDoctorFeesSearchQueryHandler(IUnitDOFRepository unitDOFRepository)
        {
            _unitDOFRepository = unitDOFRepository;
        }
        public async Task<PagedResponse<UnitOfTheDoctorFeesDto>> Handle(UnitOfTheDoctorFeesSearchQuery request, CancellationToken cancellationToken)
        {
            var res = await UnitDOF.Search(_unitDOFRepository, f => f.IsDeleted == false, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<UnitOfTheDoctorFeesDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => UnitOfTheDoctorFeesDto.FromUnitOfTheDoctorFees(s)).ToList()
            };
        }
    }
}
