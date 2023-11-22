using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.ItemListTypes;
using EHealth.ManageItemLists.Domain.PackageTypes;
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

namespace EHealth.ManageItemLists.Domain.PackageSubTypes
{
    public class PackageSubType : ItemManagmentBaseClass, IEntity<int>, IValidationModel<PackageSubType>
    {
        private PackageSubType()
        {

        }
        public string NameAr { get; private set; }
        public string NameEN { get; private set; }
        public int PackageTypeId { get; private set; }

        public PackageType PackageType { get; private set; }
        public AbstractValidator<PackageSubType> Validator => new PackageSubTypeValidator();
        public static async Task<PackageSubType> Get(int id, IPackageSubTypeRepository repository)
        {
            var dbPackageSubType = await repository.GetById(id);

            if (dbPackageSubType is null)
            {
                throw new DataNotFoundException();
            }

            return dbPackageSubType;
        }

        public static async Task<PagedResponse<PackageSubType>> Search(IPackageSubTypeRepository repository, Expression<Func<PackageSubType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }
    }
}
