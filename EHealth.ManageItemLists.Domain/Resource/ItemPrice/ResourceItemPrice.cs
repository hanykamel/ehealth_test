using EHealth.ManageItemLists.Domain.PriceUnits;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace EHealth.ManageItemLists.Domain.Resource.ItemPrice
{
    public class ResourceItemPrice : EHealthDomainObject, IEntity<int>, IValidationModel<ResourceItemPrice>
    {
        private ResourceItemPrice()
        {

        }
        public int Id { get; private set; }
        public double Price { get; private set; }
        public int PriceUnitId { get; private set; }
        public PriceUnit PriceUnit { get; private set; }
        public DateTime EffectiveDateFrom { get; private set; }
        public DateTime? EffectiveDateTo { get; private set; }
        public Guid? ResourceUHIAId { get; private set; }

        [ExcludeFromCodeCoverage]
        public ResourceUHIA? ResourceUHIA { get; private set; }
        public AbstractValidator<ResourceItemPrice> Validator => new ResourceItemPriceValidator();
        public void SetPrice(double input)
        {
            if (Price == input) return;
            Price = input;
        }

        public void SetPriceUnitId(int input)
        {
            if (PriceUnitId == input) return;
            PriceUnitId = input;
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
        public async Task<int> Create(IResourceItemPriceRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IResourceItemPriceRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public void Update(ResourceItemPrice resourceItemPrice, string modifiedBy)
        {
            
            Price = resourceItemPrice.Price;
            PriceUnitId = resourceItemPrice.PriceUnitId;
            EffectiveDateFrom = resourceItemPrice.EffectiveDateFrom;
            EffectiveDateTo = resourceItemPrice.EffectiveDateTo;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTimeOffset.Now;

        }

        public async Task<bool> Delete(IResourceItemPriceRepository repository)
        {
            return await repository.Delete(this);
        }

        public static async Task<ResourceItemPrice> Get(int id, IResourceItemPriceRepository repository)
        {
            var dbResourceItemPrice = await repository.Get(id);

            if (dbResourceItemPrice is null)
            {
                throw new DataNotFoundException();
            }

            return dbResourceItemPrice;
        }
        public static ResourceItemPrice Create(int? id, double price, int priceUnitId, DateTime effectiveDateFrom, DateTime? effectiveDateTo, string? createdBy, string tenantId)
        {
            return new ResourceItemPrice
            {
                Id = id ?? 0,
                Price = price,
                PriceUnitId = priceUnitId,
                EffectiveDateFrom = effectiveDateFrom,
                EffectiveDateTo = effectiveDateTo,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                TenantId = tenantId
            };
        }
        private async Task<bool> EnsureNoDuplicates(IResourceItemPriceRepository repository, bool throwException = true)
        {
            var ResourceItemPrice = await repository.Get(Id);
            if (Id == default)
            {
                if (ResourceItemPrice is not null)
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (ResourceItemPrice is not null)
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
