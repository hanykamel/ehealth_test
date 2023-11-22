using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.Queries;
using EHealth.ManageItemLists.Application.Lookups.LocalSpecialtyDepartment.DTOs;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.LocalSpecialtyDepartment.Queries.Handler
{
    public class LocalSpecialtyDepartmentsSearchQueryHandler : IRequestHandler<LocalSpecialtyDepartmentsSearchQuery, PagedResponse<LocalSpecialtyDepartmentDto>>
    {
        private readonly ILocalSpecialtyDepartmentsRepository _localSpecialtyDepartmentsRepository;
        public LocalSpecialtyDepartmentsSearchQueryHandler(ILocalSpecialtyDepartmentsRepository localSpecialtyDepartmentsRepository)
        {
            _localSpecialtyDepartmentsRepository = localSpecialtyDepartmentsRepository;
        }
        public async Task<PagedResponse<LocalSpecialtyDepartmentDto>> Handle(LocalSpecialtyDepartmentsSearchQuery request, CancellationToken cancellationToken)
        {
            var res = await Domain.LocalSpecialtyDepartments.LocalSpecialtyDepartment.Search(_localSpecialtyDepartmentsRepository, f => f.IsDeleted == false, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<LocalSpecialtyDepartmentDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => LocalSpecialtyDepartmentDto.FromLLocalSpecialityDepartment(s)).ToList()
            };
        }
    }
}
