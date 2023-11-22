using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;


namespace EHealth.ManageItemLists.Domain.Pre_authorizationProtocol
{
    public class PreAuthorizationProtocol : ItemManagmentBaseClass, IEntity<int>, IValidationModel<PreAuthorizationProtocol>
    {
        private PreAuthorizationProtocol()
        {
            //default value is Active
            this.Active = true;
        }

        public string? NameAr { get; private set; }
        public string? NameENG { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionENG { get; private set; }
       

        public AbstractValidator<PreAuthorizationProtocol> Validator => new PreAuthorizationProtocolValidator();
        AbstractValidator<PreAuthorizationProtocol> IValidationModel<PreAuthorizationProtocol>.Validator => throw new NotImplementedException();

        public async Task<int> Create(IPreAuthorizationProtocolRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.CreatePreAuthorizationProtocol(this);
        }

        public async Task<bool> Update(IPreAuthorizationProtocolRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.UpdatePreAuthorizationProtocol(this);
        }

        public async Task<bool> Delete(IPreAuthorizationProtocolRepository repository)
        {
            return await repository.DeletePreAuthorizationProtocol(this);
        }

        public static async Task<PagedResponse<PreAuthorizationProtocol>> Search(IPreAuthorizationProtocolRepository repository, int id, string? code, string? nameAr, string? nameENG, string? definitionAr, string? definitionENG, bool active, int pageNumber, int pageSize)
        {
            return await repository.Search(id, code, nameAr, nameENG, definitionAr, definitionENG, active, pageNumber, pageSize);
        }

        public static async Task<PreAuthorizationProtocol> Get(int id, IPreAuthorizationProtocolRepository repository)
        {
            var dbPreAuthorizationProtocol = await repository.Get(id);

            if (dbPreAuthorizationProtocol is null)
            {
                throw new DataNotFoundException();
            }

            return dbPreAuthorizationProtocol;
        }

        public static PreAuthorizationProtocol Create(int? id,string code, string nameAr, string nameENG, string DefinitionAr, string DefinitionENG, string createdBy)
        {
            return new PreAuthorizationProtocol
            {
                Id = id ?? 0,
                Code = code,
                NameAr = nameAr,
                NameENG = nameENG,
                DefinitionAr = DefinitionAr,
                DefinitionENG = DefinitionENG,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,

            };
        }

        private async Task<bool> EnsureNoDuplicates(IPreAuthorizationProtocolRepository repository, bool throwException = true)
        {
            var dbPreAuthorizationProtocol = await repository.Search(Id, Code, NameAr, NameENG, DefinitionAr, DefinitionENG, Active, 1, 1);
            if (Id == default)
            {
                if (dbPreAuthorizationProtocol.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbPreAuthorizationProtocol.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
