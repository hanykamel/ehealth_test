using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;

namespace EHealth.ManageItemLists.Domain.Pre_authorizationlevel
{
    public class PreAuthorizationlevel : ItemManagmentBaseClass, IEntity<int>, IValidationModel<PreAuthorizationlevel>
    {
        private PreAuthorizationlevel()
        {
            //default value is Active
            this.Active = true;
        }
   
        public string? LevelAr { get; private set; }
        public string? LevelENG { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionENG { get; private set; }
       
        public AbstractValidator<PreAuthorizationlevel> Validator => new PreAuthorizationlevelValidator();
        AbstractValidator<PreAuthorizationlevel> IValidationModel<PreAuthorizationlevel>.Validator => throw new NotImplementedException();

        public async Task<int> Create(IPreAuthorizationlevelRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.CreatePreAuthorizationlevel(this);
        }

        public async Task<bool> Update(IPreAuthorizationlevelRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.UpdatePreAuthorizationlevel(this);
        }

        public async Task<bool> Delete(IPreAuthorizationlevelRepository repository)
        {
            return await repository.DeletePreAuthorizationlevel(this);
        }

        public static async Task<PagedResponse<PreAuthorizationlevel>> Search(IPreAuthorizationlevelRepository repository, int id, string?code, string? levelAr, string? levelENG, string? definitionAr, string? definitionENG, bool active, int pageNumber, int pageSize)
        {
            return await repository.Search(id, code, levelAr, levelENG, definitionAr, definitionENG, active, pageNumber, pageSize);
        }

        public static async Task<PreAuthorizationlevel> Get(int id, IPreAuthorizationlevelRepository repository)
        {
            var dbPreAuthorizationlevel = await repository.Get(id);

            if (dbPreAuthorizationlevel is null)
            {
                throw new DataNotFoundException();
            }

            return dbPreAuthorizationlevel;
        }

        public static PreAuthorizationlevel Create(int? id,string code, string? levelAr, string? levelENG, string? DefinitionAr, string? DefinitionENG, string createdBy)
        {
            return new PreAuthorizationlevel
            {
                Id = id ?? 0,
                Code =code,
                LevelAr = levelAr,
                LevelENG = levelENG,
                DefinitionAr = DefinitionAr,
                DefinitionENG = DefinitionENG,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,

            };
        }

        private async Task<bool> EnsureNoDuplicates(IPreAuthorizationlevelRepository repository, bool throwException = true)
        {
            var dbPreAuthorizationlevel = await repository.Search(Id, Code, LevelAr, LevelENG, DefinitionAr, DefinitionENG, Active, 1, 1);
            if (Id == default)
            {
                if (dbPreAuthorizationlevel.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbPreAuthorizationlevel.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
