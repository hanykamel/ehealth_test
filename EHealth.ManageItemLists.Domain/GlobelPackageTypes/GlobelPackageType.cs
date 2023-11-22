using EHealth.ManageItemLists.Domain.LocalUnitOfMeasure;
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

namespace EHealth.ManageItemLists.Domain.GlobelPackageTypes
{
    public class GlobelPackageType : ItemManagmentBaseClass, IEntity<int>, IValidationModel<GlobelPackageType>
    {
        public GlobelPackageType()
        {
            Active = true;
        }

        public string GlobalTypeAr { get; private set; }
        public string GlobalTypeEn { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEn { get; private set; }

        public AbstractValidator<GlobelPackageType> Validator => new GlobelPackageTypeValidator();
        AbstractValidator<GlobelPackageType> IValidationModel<GlobelPackageType>.Validator => throw new NotImplementedException();

        //public int Id => throw new NotImplementedException();

        public async Task<int> Create(IGlobelPackageTypeRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IGlobelPackageTypeRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IGlobelPackageTypeRepository repository)
        {
            return await repository.Delete(this);
        }

        public static async Task<PagedResponse<GlobelPackageType>> Search(IGlobelPackageTypeRepository repository, Expression<Func<GlobelPackageType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }


        public static async Task<GlobelPackageType> Get(int id, IGlobelPackageTypeRepository repository)
        {
            var dbGlobelPackageType = await repository.Get(id);

            if (dbGlobelPackageType is null)
            {
                throw new DataNotFoundException();
            }

            return dbGlobelPackageType;
        }

        public static GlobelPackageType Create(int? id, string code, string globalTypeAr, string globalTypeEn, string? definitionAr, string? definitionEn, string createdBy)
        {
            return new GlobelPackageType
            {
                Id = id ?? 0,
                Code = code,
                GlobalTypeAr = globalTypeAr,
                GlobalTypeEn = globalTypeEn,
                DefinitionAr = definitionAr,
                DefinitionEn = definitionEn,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,

            };
        }

        private async Task<bool> EnsureNoDuplicates(IGlobelPackageTypeRepository repository, bool throwException = true)
        {
            var dbGlobelPackageType = await repository.Search(x => (x.Id == Id || x.Code == Code || x.GlobalTypeAr == GlobalTypeAr
            || x.GlobalTypeEn == GlobalTypeEn) && x.IsDeleted != true, 1, 1, false);
            if (Id == default)
            {
                if (dbGlobelPackageType.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbGlobelPackageType.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }

    }
}
