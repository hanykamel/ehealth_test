using EHealth.ManageItemLists.Domain.ConsumablesCodesStatus;
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

namespace EHealth.ManageItemLists.Domain.ProceduresCodesStatus
{
    public class ProceduresCodeStatus: ItemManagmentBaseClass, IEntity<int>, IValidationModel<ProceduresCodeStatus>
    {
        private ProceduresCodeStatus()
        {
            //default value is Active
            this.Active = true;
        }

        public string? CodeStatusDescAr { get; private set; }
        public string? CodeStatusDescEng { get; private set; }

        public AbstractValidator<ProceduresCodeStatus> Validator => new ProceduresCodeStatusValidator();
        AbstractValidator<ProceduresCodeStatus> IValidationModel<ProceduresCodeStatus>.Validator => throw new NotImplementedException();
        public async Task<int> Create(IProceduresCodeStatusRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.CreateProceduresCodeStatus(this);
        }

        public async Task<bool> Update(IProceduresCodeStatusRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.UpdateProceduresCodeStatus(this);
        }

        public async Task<bool> Delete(IProceduresCodeStatusRepository repository)
        {
            return await repository.DeleteProceduresCodeStatus(this);
        }

        public static async Task<PagedResponse<ProceduresCodeStatus>> Search(IProceduresCodeStatusRepository repository, int id, string? code, string? CodeStatusDescAr, string? CodeStatusDescEng, bool active, int pageNumber, int pageSize)
        {
            return await repository.Search(id, code, CodeStatusDescAr, CodeStatusDescEng, active, pageNumber, pageSize);
        }

        public static async Task<ProceduresCodeStatus> Get(int id, IProceduresCodeStatusRepository repository)
        {
            var dbProceduresCodeStatus = await repository.Get(id);

            if (dbProceduresCodeStatus is null)
            {
                throw new DataNotFoundException();
            }

            return dbProceduresCodeStatus;
        }

        public static ProceduresCodeStatus Create(int? id, string code, string? CodeStatusDescAr, string? CodeStatusDescEng, string createdBy)
        {
            return new ProceduresCodeStatus
            {
                Id = id ?? 0,
                Code = code,
                CodeStatusDescAr = CodeStatusDescAr,
                CodeStatusDescEng = CodeStatusDescEng,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,

            };
        }

        private async Task<bool> EnsureNoDuplicates(IProceduresCodeStatusRepository repository, bool throwException = true)
        {
            var dbProceduresCodeStatus = await repository.Search(Id, Code, CodeStatusDescAr, CodeStatusDescEng, Active, 1, 1);
            if (Id == default)
            {
                if (dbProceduresCodeStatus.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbProceduresCodeStatus.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
