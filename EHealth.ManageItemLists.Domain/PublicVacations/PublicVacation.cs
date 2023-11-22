using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.PublicVacations
{
    public class PublicVacation : EHealthDomainObject, IEntity<int>, IValidationModel<PublicVacation>
    {
        private PublicVacation()
        {
            
        }
        public int Id { get; set; }
        public string? NameAr { get; private set; }
        public string? NameEn { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }

        public AbstractValidator<PublicVacation> Validator => new PublicVacationValidator();
        AbstractValidator<PublicVacation> IValidationModel<PublicVacation>.Validator => throw new NotImplementedException();
        public async Task<int> Create(IPublicVacationRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IPublicVacationRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IPublicVacationRepository repository)
        {
            return await repository.Delete(this);
        }

        public static async Task<PagedResponse<PublicVacation>> Search(IPublicVacationRepository repository, int id, string? nameAr, string? nameEn, DateTime fromDate, DateTime toDate, int pageNumber, int pageSize)
        {
            return await repository.Search(id, nameAr, nameEn, fromDate, toDate, pageNumber, pageSize);
        }

        public static async Task<PublicVacation> Get(int id, IPublicVacationRepository repository)
        {
            var dbPublicVacation = await repository.Get(id);

            if (dbPublicVacation is null)
            {
                throw new DataNotFoundException();
            }

            return dbPublicVacation;
        }

        public static PublicVacation Create(int? id, string nameAr, string nameEn, DateTime fromDate, DateTime toDate, string createdBy)
        {
            return new PublicVacation
            {
                Id = id ?? 0,
                NameAr = nameAr,
                NameEn = nameEn,
                FromDate = fromDate,
                ToDate = toDate,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
            };
        }

        private async Task<bool> EnsureNoDuplicates(IPublicVacationRepository repository, bool throwException = true)
        {
            var dbPublicVacation = await repository.Search(Id, NameAr, NameEn, FromDate, ToDate, 1, 1);
            if (Id == default)
            {
                if (dbPublicVacation.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbPublicVacation.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
