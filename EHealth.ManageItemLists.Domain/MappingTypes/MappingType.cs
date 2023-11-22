using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;

namespace EHealth.ManageItemLists.Domain.MappingTypes
{
    public class MappingType : ItemManagmentBaseClass, IEntity<int>, IValidationModel<MappingType>
    {
        private MappingType()
        {
            //default value is Active
            this.Active = true;
        }
   
        public string? MappingTypeAr { get; private set; }
        public string? MappingTypeENG { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionENG { get; private set; }


        public AbstractValidator<MappingType> Validator => new MappingTypeValidator();
        AbstractValidator<MappingType> IValidationModel<MappingType>.Validator => throw new NotImplementedException();

        public async Task<int> Create(IMappingTypeRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.CreateMappingType(this);
        }

        public async Task<bool> Update(IMappingTypeRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.UpdateMappingType(this);
        }

        public async Task<bool> Delete(IMappingTypeRepository repository)
        {
            return await repository.DeleteMappingType(this);
        }

        public static async Task<PagedResponse<MappingType>> Search(IMappingTypeRepository repository, int id, string? code, string? mappingTypeAr, string? mappingTypeENG, string? definitionAr, string? definitionENG, bool active, int pageNumber, int pageSize)
        {
            return await repository.Search(id, code, mappingTypeAr, mappingTypeENG, definitionAr, definitionENG, active, pageNumber, pageSize);
        }

        public static async Task<MappingType> Get(int id, IMappingTypeRepository repository)
        {
            var dbMappingType = await repository.Get(id);

            if (dbMappingType is null)
            {
                throw new DataNotFoundException();
            }

            return dbMappingType;
        }

        public static MappingType Create(int? id,string code, string mappingTypeAr, string mappingTypeENG, string DefinitionAr, string DefinitionENG, string createdBy)
        {
            return new MappingType
            {
                Id = id ?? 0,
                Code =code,
                MappingTypeAr = mappingTypeAr,
                MappingTypeENG = mappingTypeENG,
                DefinitionAr = DefinitionAr,
                DefinitionENG = DefinitionENG,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,

            };
        }

        private async Task<bool> EnsureNoDuplicates(IMappingTypeRepository repository, bool throwException = true)
        {
            var dbMappingType = await repository.Search(Id, Code, MappingTypeAr, MappingTypeENG, DefinitionAr, DefinitionENG, Active, 1, 1);
            if (Id == default)
            {
                if (dbMappingType.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbMappingType.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
