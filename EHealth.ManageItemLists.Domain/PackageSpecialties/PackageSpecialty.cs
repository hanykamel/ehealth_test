using EHealth.ManageItemLists.Domain.GlobelPackageTypes;
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

namespace EHealth.ManageItemLists.Domain.PackageSpecialties
{
    public class PackageSpecialty : ItemManagmentBaseClass, IEntity<int>, IValidationModel<PackageSpecialty>
    {

        public PackageSpecialty()
        {
            Active = true;
        }

        public string SpecialtyAr { get; private set; }
        public string SpecialtyEn { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEn { get; private set; }

        public AbstractValidator<PackageSpecialty> Validator => new PackageSpecialtyValidator();
        AbstractValidator<PackageSpecialty> IValidationModel<PackageSpecialty>.Validator => throw new NotImplementedException();

        //public int Id => throw new NotImplementedException();

        public async Task<int> Create(IPackageSpecialtiesRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IPackageSpecialtiesRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IPackageSpecialtiesRepository repository)
        {
            return await repository.Delete(this);
        }

        public static async Task<PagedResponse<PackageSpecialty>> Search(IPackageSpecialtiesRepository repository, Expression<Func<PackageSpecialty, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }


        public static async Task<PackageSpecialty> Get(int id, IPackageSpecialtiesRepository repository)
        {
            var dbPackageSpecialty = await repository.Get(id);

            if (dbPackageSpecialty is null)
            {
                throw new DataNotFoundException();
            }

            return dbPackageSpecialty;
        }

        public static PackageSpecialty Create(int? id, string code, string specialtyAr, string specialtyEn, string? definitionAr, string? definitionEn, string createdBy)
        {
            return new PackageSpecialty
            {
                Id = id ?? 0,
                Code = code,
                SpecialtyAr = specialtyAr,
                SpecialtyEn = specialtyEn,
                DefinitionAr = definitionAr,
                DefinitionEn = definitionEn,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,

            };
        }

        private async Task<bool> EnsureNoDuplicates(IPackageSpecialtiesRepository repository, bool throwException = true)
        {
            var dbPackageSpecialty = await repository.Search(x => (x.Id == Id || x.Code == Code || x.SpecialtyAr == SpecialtyAr
            || x.SpecialtyEn == SpecialtyEn) && x.IsDeleted != true, 1, 1, false);

            if (Id == default)
            {
                if (dbPackageSpecialty.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbPackageSpecialty.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }

    }
}
