using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.RegistrationTypes
{
    public class RegistrationType : ItemManagmentBaseClass, IEntity<int>, IValidationModel<RegistrationType>
    {
    
        private RegistrationType()
        {
            //default value is Active
            this.Active = true;
        }

        public string RegistrationTypeAr { get; private set; }
        public string RegistrationTypeENG { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionENG { get; private set; }


        public AbstractValidator<RegistrationType> Validator => new RegistrationTypeValidator();
        AbstractValidator<RegistrationType> IValidationModel<RegistrationType>.Validator => throw new NotImplementedException();
        public async Task<int> Create(IRegistrationRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.CreateRegistrationType(this);
        }

        public async Task<bool> Update(IRegistrationRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.UpdateRegistrationType(this);
        }

        public async Task<bool> Delete(IRegistrationRepository repository)
        {
            return await repository.DeleteRegistrationType(this);
        }

        public static async Task<PagedResponse<RegistrationType>> Search(IRegistrationRepository repository, Expression<Func<RegistrationType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }

        public static async Task<RegistrationType> Get(int id, IRegistrationRepository repository)
        {
            var dbRegistrationType = await repository.Get(id);

            if (dbRegistrationType is null)
            {
                throw new DataNotFoundException();
            }

            return dbRegistrationType;
        }

        public static RegistrationType Create(int? id, string code, string registrationTypeAr, string registrationTypeENG, string DefinitionAr, string DefinitionENG, string createdBy)
        {
            return new RegistrationType
            {
                Id = id ?? 0,
                Code = code,
                RegistrationTypeAr = registrationTypeAr,
                RegistrationTypeENG = registrationTypeENG,
                DefinitionAr = DefinitionAr,
                DefinitionENG = DefinitionENG,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
            };
        }

        private async Task<bool> EnsureNoDuplicates(IRegistrationRepository repository, bool throwException = true)
        {
            var dbRegistrationType = await repository.Search(x => x.Id == Id || x.Code == Code, 1, 1, false);
            if (Id == default)
            {
                if (dbRegistrationType.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbRegistrationType.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
