using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.ItemListTypes;
using EHealth.ManageItemLists.Domain.PackageSubTypes;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.PackageTypes
{
    public class PackageType : ItemManagmentBaseClass, IEntity<int>, IValidationModel<PackageType>
    {
        private PackageType()
        {
            
        }
        public string NameAr { get; private set; }
        public string NameEN { get; private set; }
        public IList<PackageSubType> PackageSubTypes { get; private set; } = new List<PackageSubType>();
        public AbstractValidator<PackageType> Validator => new PackageTypeValidator();

        public static async Task<PackageType> Get(int id, IPackageTypeRepository repository)
        {
            var dbPackageType = await repository.Get(id);

            if (dbPackageType is null)
            {
                throw new DataNotFoundException();
            }

            return dbPackageType;
        }
        public static async Task<PagedResponse<PackageType>> Search(IPackageTypeRepository repository, Expression<Func<PackageType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }
    }
}
