using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;


namespace EHealth.ManageItemLists.Domain.ConsumablesCodesStatus
{
    public class ConsumablesCodeStatus : ItemManagmentBaseClass, IEntity<int>, IValidationModel<ConsumablesCodeStatus>
    {
        private ConsumablesCodeStatus()
        {
            //default value is Active
            this.Active = true;
        }

        public string? CodeStatusDescAr { get;private set; }
        public string? CodeStatusDescEng { get; private set; }

        public AbstractValidator<ConsumablesCodeStatus> Validator => new ConsumablesCodeStatusValidator();
        AbstractValidator<ConsumablesCodeStatus> IValidationModel<ConsumablesCodeStatus>.Validator => throw new NotImplementedException();
        public async Task<int> Create(IConsumablesCodeStatusRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.CreateConsumablesCodeStatus(this);
        }

        public async Task<bool> Update(IConsumablesCodeStatusRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.UpdateConsumablesCodeStatus(this);
        }

        public async Task<bool> Delete(IConsumablesCodeStatusRepository repository)
        {
            return await repository.DeleteConsumablesCodeStatus(this);
        }

        public static async Task<PagedResponse<ConsumablesCodeStatus>> Search(IConsumablesCodeStatusRepository repository, int id, string? code, string? CodeStatusDescAr, string? CodeStatusDescEng, bool active, int pageNumber, int pageSize)
        {
            return await repository.Search(id, code, CodeStatusDescAr, CodeStatusDescEng, active, pageNumber, pageSize);
        }

        public static async Task<ConsumablesCodeStatus> Get(int id, IConsumablesCodeStatusRepository repository)
        {
            var dbConsumablesCodeStatus = await repository.Get(id);

            if (dbConsumablesCodeStatus is null)
            {
                throw new DataNotFoundException();
            }

            return dbConsumablesCodeStatus;
        }

        public static ConsumablesCodeStatus Create(int? id, string code, string? CodeStatusDescAr, string? CodeStatusDescEng, string createdBy)
        {
            return new ConsumablesCodeStatus
            {
                Id = id ?? 0,
                Code  = code,
                CodeStatusDescAr = CodeStatusDescAr,
                CodeStatusDescEng = CodeStatusDescEng,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,

            };
        }

        private async Task<bool> EnsureNoDuplicates(IConsumablesCodeStatusRepository repository, bool throwException = true)
        {
            var dbConsumablesCodeStatus = await repository.Search(Id, Code, CodeStatusDescAr, CodeStatusDescEng,  Active, 1, 1);
            if (Id == default)
            {
                if (dbConsumablesCodeStatus.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbConsumablesCodeStatus.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }

    }
}
