using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;

namespace EHealth.ManageItemLists.Domain.ItemTypes
{
    public class ItemType : ItemManagmentBaseClass, IEntity<int>, IValidationModel<ItemType>
    {
        private ItemType()
        {
            //default value is Active
            this.Active = true;
        }
        public string? NameAr { get; private set; }
        public string? NameEN { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEN { get; private set; }
        public AbstractValidator<ItemType> validator => new ItemTypeValidator();
        AbstractValidator<ItemType> IValidationModel<ItemType>.Validator => throw new NotImplementedException();
        public async Task<int> Create(IItemTypeRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.CreateItemType(this);
        }

        public async Task<bool> Update(IItemTypeRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.UpdateItemType(this);
        }

        public async Task<bool> Delete(IItemTypeRepository repository)
        {
            return await repository.DeleteItemType(this);
        }

        public static async Task<PagedResponse<ItemType>> Search(IItemTypeRepository repository, int id, string code, string? nameAr, string? nameEN, string? definitionAr, string? definitionEN, bool active, int pageNumber, int pageSize)
        {
            return await repository.Search(id, code, nameAr, nameEN, definitionAr, definitionEN, active, pageNumber, pageSize);
        }

        public static async Task<ItemType> Get(int id, IItemTypeRepository repository)
        {
            var dbItemType = await repository.Get(id);

            if (dbItemType is null)
            {
                throw new DataNotFoundException();
            }

            return dbItemType;
        }

        public static ItemType Create(int? id, string code, string nameAr, string nameEN, string DefinitionAr, string DefinitionEN, string createdBy)
        {
            return new ItemType
            {
                Id = id ?? 0,
                Code = code,
                NameAr = nameAr,
                NameEN = nameEN,
                DefinitionAr = DefinitionAr,
                DefinitionEN = DefinitionEN,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,

            };
        }

        private async Task<bool> EnsureNoDuplicates(IItemTypeRepository repository, bool throwException = true)
        {
            var dbItemType = await repository.Search(Id, Code, NameAr, NameEN, DefinitionAr, DefinitionEN, Active, 1, 1);
            if (Id == default)
            {
                if (dbItemType.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbItemType.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
