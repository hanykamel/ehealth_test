using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;

namespace EHealth.ManageItemLists.Domain.DispensingLocations
{
    public class DispensingLocation : ItemManagmentBaseClass, IEntity<int>, IValidationModel<DispensingLocation>
    {
        private DispensingLocation()
        {
            //default value is Active
            this.Active = true;
        }

        public string? NameAr { get; private set; }
        public string? NameENG { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionENG { get; private set; }

   
        public AbstractValidator<DispensingLocation> Validator => new DispensingLocationValidator();
        AbstractValidator<DispensingLocation> IValidationModel<DispensingLocation>.Validator => throw new NotImplementedException();

        public async Task<int> Create(IDispensingLocationRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.CreateDispensingLocation(this);
        }

        public async Task<bool> Update(IDispensingLocationRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.UpdateDispensingLocation(this);
        }

        public async Task<bool> Delete(IDispensingLocationRepository repository)
        {
            return await repository.DeleteDispensingLocation(this);
        }

        public static async Task<PagedResponse<DispensingLocation>> Search(IDispensingLocationRepository repository, int id, string code, string? nameAr, string? nameENG, string? definitionAr, string? definitionENG, bool active, int pageNumber, int pageSize)
        {
            return await repository.Search(id, code, nameAr, nameENG, definitionAr, definitionENG, active, pageNumber, pageSize);
        }

        public static async Task<DispensingLocation> Get(int id, IDispensingLocationRepository repository)
        {
            var dbDispensingLocation = await repository.Get(id);

            if (dbDispensingLocation is null)
            {
                throw new DataNotFoundException();
            }

            return dbDispensingLocation;
        }

        public static DispensingLocation Create(int? id, string code, string nameAr, string nameENG, string DefinitionAr, string DefinitionENG, string createdBy)
        {
            return new DispensingLocation
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

        private async Task<bool> EnsureNoDuplicates(IDispensingLocationRepository repository, bool throwException = true)
        {
            var dbDispensingLocation = await repository.Search(Id, Code, NameAr, NameENG, DefinitionAr, DefinitionENG, Active, 1, 1);
            if (Id == default)
            {
                if (dbDispensingLocation.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbDispensingLocation.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }

    }
}
