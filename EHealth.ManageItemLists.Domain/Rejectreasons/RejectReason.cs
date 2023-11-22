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

namespace EHealth.ManageItemLists.Domain.Rejectreasons
{
    public class RejectReason : ItemManagmentBaseClass, IEntity<int>, IValidationModel<RejectReason>
    {
        private RejectReason()
        {
            //default value is Active
            this.Active = true;
        }

        public string? RejectReasonAr { get; private set; }
        public string? RejectReasonEn { get; private set; }

        public AbstractValidator<RejectReason> Validator => new RejectReasonValidator();
        AbstractValidator<RejectReason> IValidationModel<RejectReason>.Validator => throw new NotImplementedException();
        public async Task<int> Create(IRejectReasonRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IRejectReasonRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IRejectReasonRepository repository)
        {
            return await repository.Delete(this);
        }

        public static async Task<PagedResponse<RejectReason>> Search(IRejectReasonRepository repository, int? id, string? code, string? rejectReasonAr, string? rejectReasonEn, bool active, int pageNumber, int pageSize)
        {
            return await repository.Search(id, code, rejectReasonAr, rejectReasonEn, active, pageNumber, pageSize);
        }

        public static async Task<RejectReason> Get(int id, IRejectReasonRepository repository)
        {
            var dbRejectReason = await repository.Get(id);

            if (dbRejectReason is null)
            {
                throw new DataNotFoundException();
            }

            return dbRejectReason;
        }

        public static RejectReason Create(int? id, string code, string rejectReasonAr, string rejectReasonEn, string createdBy)
        {
            return new RejectReason
            {
                Id = id ?? 0,
                Code = code,
                RejectReasonAr = rejectReasonAr,
                RejectReasonEn = rejectReasonEn,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
            };
        }

        private async Task<bool> EnsureNoDuplicates(IRejectReasonRepository repository, bool throwException = true)
        {
            var dbRejectReason = await repository.Search(Id, Code, RejectReasonAr, RejectReasonEn, Active, 1, 1);
            if (Id == default)
            {
                if (dbRejectReason.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbRejectReason.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
