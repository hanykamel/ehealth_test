using EHealth.ManageItemLists.Domain.Prescribinglevels;
using EHealth.ManageItemLists.Domain.PriceUnits;
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

namespace EHealth.ManageItemLists.Domain.PackageComplexityClassifications
{
    public class PackageComplexityClassification : ItemManagmentBaseClass, IEntity<int>, IValidationModel<PackageComplexityClassification>
    {
        private PackageComplexityClassification()
        {
            //default value is Active
            this.Active = true;
        }

        public string ComplexityAr { get; private set; }
        public string ComplexityEn { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEn { get; private set; }

        public AbstractValidator<PackageComplexityClassification> Validator => new PackageComplexityClassificationValidator();
        public async Task<int> Create(IPackageComplexityClassificationRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IPackageComplexityClassificationRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IPackageComplexityClassificationRepository repository)
        {
            return await repository.Delete(this);
        }

        public static async Task<PagedResponse<PackageComplexityClassification>> Search(IPackageComplexityClassificationRepository repository, Expression<Func<PackageComplexityClassification, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }

        public static async Task<PackageComplexityClassification> Get(int id, IPackageComplexityClassificationRepository repository)
        {
            var dbPackageComplexityClassification = await repository.Get(id);

            if (dbPackageComplexityClassification is null)
            {
                throw new DataNotFoundException();
            }

            return dbPackageComplexityClassification;
        }

        public static PackageComplexityClassification Create(int? id, string code, string complexityAr, string complexityEn, string DefinitionAr, string DefinitionEn, string createdBy)
        {
            return new PackageComplexityClassification
            {
                Id = id ?? 0,
                Code = code,
                ComplexityAr = complexityAr,
                ComplexityEn = complexityEn,
                DefinitionAr = DefinitionAr,
                DefinitionEn = DefinitionEn,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
            };
        }

        private async Task<bool> EnsureNoDuplicates(IPackageComplexityClassificationRepository repository, bool throwException = true)
        {
            var dbPackageComplexityClassification = await repository.Search(p => p.Id == Id && p.Code == Code && p.ComplexityAr == ComplexityAr && p.ComplexityEn == ComplexityEn && p.DefinitionAr == DefinitionAr && p.DefinitionEn == DefinitionEn && p.IsDeleted != true, 1,1,true);
            if (Id == default)
            {
                if (dbPackageComplexityClassification.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbPackageComplexityClassification.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
