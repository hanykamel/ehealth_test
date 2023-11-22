using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;


namespace EHealth.ManageItemLists.Domain.Prescribinglevels
{
    public class Prescribinglevel : ItemManagmentBaseClass, IEntity<int>, IValidationModel<Prescribinglevel>
    {
        private Prescribinglevel()
        {
            //default value is Active
            this.Active = true;
        }

        public string? LevelAr { get; private set; }
        public string? LevelENG { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionENG { get; private set; }

        public AbstractValidator<Prescribinglevel> Validator => new PrescribinglevelValidator();
        AbstractValidator<Prescribinglevel> IValidationModel<Prescribinglevel>.Validator => throw new NotImplementedException();
        public async Task<int> Create(IPrescribinglevelRespository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.CreatePrescribinglevel(this);
        }

        public async Task<bool> Update(IPrescribinglevelRespository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.UpdatePrescribinglevel(this);
        }

        public async Task<bool> Delete(IPrescribinglevelRespository repository)
        {
            return await repository.DeletePrescribinglevel(this);
        }

        public static async Task<PagedResponse<Prescribinglevel>> Search(IPrescribinglevelRespository repository, int id, string code, string? levelAr, string? levelENG, string? definitionAr, string? definitionENG, bool active, int pageNumber, int pageSize)
        {
            return await repository.Search(id, code, levelAr, levelENG, definitionAr, definitionENG, active, pageNumber, pageSize);

        }
        public static async Task<Prescribinglevel> Get(int id, IPrescribinglevelRespository repository)
        {
            var dbPrescribinglevel = await repository.Get(id);

            if (dbPrescribinglevel is null)
            {
                throw new DataNotFoundException();
            }

            return dbPrescribinglevel;
        }

        public static Prescribinglevel Create(int? id, string code, string levelAr, string levelENG, string DefinitionAr, string DefinitionENG, string createdBy)
        {
            return new Prescribinglevel
            {
                Id = id ?? 0,
                Code = code,
                LevelAr = levelAr,
                LevelENG = levelENG,
                DefinitionAr = DefinitionAr,
                DefinitionENG = DefinitionENG,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,

            };
        }

        private async Task<bool> EnsureNoDuplicates(IPrescribinglevelRespository repository, bool throwException = true)
        {
            var dbPrescribinglevel = await repository.Search(Id, Code, LevelAr, LevelENG, DefinitionAr, DefinitionENG, Active, 1, 1);
            if (Id == default)
            {
                if (dbPrescribinglevel.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbPrescribinglevel.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
