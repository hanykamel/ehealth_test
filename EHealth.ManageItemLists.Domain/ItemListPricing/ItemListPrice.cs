using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
using EHealth.ManageItemLists.Domain.DrugsPricing;
using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;

namespace EHealth.ManageItemLists.Domain.ItemListPricing
{
    public class ItemListPrice : EHealthDomainObject, IEntity<int>, IValidationModel<ItemListPrice>
    {
        private ItemListPrice()
        {

        }
        public int Id { get; private set; }
        public double Price { get; private set; }
        public DateTime EffectiveDateFrom { get; private set; }
        public DateTime? EffectiveDateTo { get; private set; }

        public Guid? ServiceUHIAId { get; private set; }

        [ExcludeFromCodeCoverage]
        public ServiceUHIA? ServiceUHIA { get; private set; }

        public Guid? ConsumablesAndDevicesUHIAId { get; private set; }

        [ExcludeFromCodeCoverage]
        public ConsumablesAndDevicesUHIA? ConsumablesAndDevicesUHIA { get; private set; }

        public Guid? DevicesAndAssetsUHIAId { get; private set; }

        [ExcludeFromCodeCoverage]
        public DevicesAndAssetsUHIA? DevicesAndAssetsUHIA { get; private set; }

        public Guid? ProcedureICHIId { get; private set; }

        [ExcludeFromCodeCoverage]
        public ProcedureICHI? ProcedureICHI { get; private set; }

        public AbstractValidator<ItemListPrice> Validator => new ItemListPriceValidator();
        public void SetPrice(double input)
        {
            if (Price == input) return;
            Price = input;
        }

        public void SetEffectiveDateFrom(DateTime input)
        {
            if (EffectiveDateFrom == input) return;
            EffectiveDateFrom = input;
        }

        public void SetEffectiveDateTo(DateTime? input)
        {
            if (EffectiveDateTo == input) return;
            EffectiveDateTo = input;
        }
        public async Task<int> Create(IItemListPriceRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IItemListPriceRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public void Update(ItemListPrice price, string modifiedBy)
        {
            Price = price.Price;
            EffectiveDateFrom = price.EffectiveDateFrom;
            EffectiveDateTo = price.EffectiveDateTo;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTimeOffset.Now;

        }

        public async Task<bool> Delete(IItemListPriceRepository repository)
        {
            return await repository.Delete(this);
        }

        public void SoftDelete(string deletedBy)
        {
            IsDeleted = true;
            ModifiedOn = DateTimeOffset.Now;
            IsDeletedBy = deletedBy;
        }

        public static async Task<ItemListPrice> Get(int id, IItemListPriceRepository repository)
        {
            var dbItemListPrice = await repository.Get(id);

            if (dbItemListPrice is null)
            {
                throw new DataNotFoundException();
            }

            return dbItemListPrice;
        }

        public static ItemListPrice Create(int? id, double price, DateTime effectiveDateFrom, DateTime? effectiveDateTo, string createdBy, string telandId)
        {
            return new ItemListPrice
            {
                Id = id ?? 0,
                Price = price,
                EffectiveDateFrom = effectiveDateFrom,
                EffectiveDateTo = effectiveDateTo,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                TenantId = telandId
            };
        }

        private async Task<bool> EnsureNoDuplicates(IItemListPriceRepository repository, bool throwException = true)
        {
            var dbItemListPrice = await repository.Get(Id);
            if (Id == default)
            {
                if (dbItemListPrice is not null)
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbItemListPrice is not null)
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }

    }
}
