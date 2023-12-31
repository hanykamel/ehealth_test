﻿using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageResources;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IFeesOfResourcesPerUnitPackageResourceRepository
    {
        Task<Guid> Create(FeesOfResourcesPerUnitPackageResource input);
        Task<bool> Update(FeesOfResourcesPerUnitPackageResource input);
        Task<bool> Delete(FeesOfResourcesPerUnitPackageResource input);
        Task<FeesOfResourcesPerUnitPackageResource?> Get(Guid id);
        Task<PagedResponse<FeesOfResourcesPerUnitPackageResource>> Search(Expression<Func<FeesOfResourcesPerUnitPackageResource, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending);
    }
}
