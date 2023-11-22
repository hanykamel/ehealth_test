using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;

namespace EHealth.ManageItemLists.Domain.DrugsPricing
{
    public class DrugPrice : EHealthDomainObject, IEntity<int>, IValidationModel<DrugPrice>
    {
        private DrugPrice()
        {

        }
        public int Id { get; private set; }
        public double MainUnitPrice { get; private set; }
        public double FullPackPrice { get; private set; }
        public double SubUnitPrice { get; private set; }
        public DateTime EffectiveDateFrom { get; private set; }
        public DateTime? EffectiveDateTo { get; private set; }
        public AbstractValidator<DrugPrice> Validator => new DrugPriceValidator();
        public async Task<int> Create(IDrugPriceRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IDrugPriceRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IDrugPriceRepository repository)
        {
            return await repository.Delete(this);
        }
        public void SoftDelete(string deletedBy)
        {
            IsDeleted = true;
            ModifiedOn = DateTimeOffset.Now;
            IsDeletedBy = deletedBy;
        }

        public void Update(DrugPrice drugPrice, string modifiedBy)
        {
            MainUnitPrice = drugPrice.MainUnitPrice;
            FullPackPrice = drugPrice.FullPackPrice;
            SubUnitPrice = drugPrice.SubUnitPrice;
            EffectiveDateFrom = drugPrice.EffectiveDateFrom;
            EffectiveDateTo = drugPrice.EffectiveDateTo;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTimeOffset.Now;

        }
        public static async Task<DrugPrice> Get(int id, IDrugPriceRepository repository)
        {
            var dbDrugPrice = await repository.Get(id);

            if (dbDrugPrice is null)
            {
                throw new DataNotFoundException();
            }

            return dbDrugPrice;
        }

        public static DrugPrice Create(int? id, double mainUnitPrice, double fullPackPrice, double subUnitPrice, DateTime effectiveDateFrom, DateTime? effectiveDateTo, string createdBy, string tenantId)
        {
            return new DrugPrice
            {
                Id = id ?? 0,
                MainUnitPrice = mainUnitPrice,
                FullPackPrice = fullPackPrice,
                SubUnitPrice = subUnitPrice,
                EffectiveDateFrom = effectiveDateFrom,
                EffectiveDateTo = effectiveDateTo,
                CreatedBy = createdBy,
                CreatedOn = DateTimeOffset.Now,
                TenantId = tenantId,
            };
        }

        public void SetMainUnitPrice(double mainUnitPrice)
        {
            if (MainUnitPrice == mainUnitPrice) return;
            MainUnitPrice = mainUnitPrice;
        }
        public void SetFullPackPrice(double fullPackPrice)
        {
            if (FullPackPrice == fullPackPrice) return;
            FullPackPrice = fullPackPrice;
        }
        public void SetSubUnitPrice(double subUnitPrice)
        {
            if (SubUnitPrice == subUnitPrice) return;
            SubUnitPrice = subUnitPrice;
        }
        public void SetEffectiveDateFrom(DateTime effectiveDateFrom)
        {
            if (EffectiveDateFrom == effectiveDateFrom) return;
            EffectiveDateFrom = effectiveDateFrom;
        }
        public void SetEffectiveDateTo(DateTime? effectiveDateTo)
        {
            if (EffectiveDateTo == effectiveDateTo) return;
            EffectiveDateTo = effectiveDateTo;
        }
        private async Task<bool> EnsureNoDuplicates(IDrugPriceRepository repository, bool throwException = true)
        {
            var dbDrugPrice = await repository.Get(Id);
            if (Id == default)
            {
                if (dbDrugPrice is not null)
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbDrugPrice is not null)
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
