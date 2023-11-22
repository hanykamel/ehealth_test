using EHealth.ManageItemLists.Domain.PriceUnits;
using EHealth.ManageItemLists.Domain.Resource.ItemPrice;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Domain.UnitOfTheDoctor_sfees;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.DoctorFees.ItemPrice
{
    public class DoctorFeesItemPrice : EHealthDomainObject, IEntity<int>, IValidationModel<DoctorFeesItemPrice>
    {
        private DoctorFeesItemPrice()
        {

        }
        public int Id { get; private set; }
        public double DoctorFees { get; private set; }
        public int UnitOfDoctorFeesId { get; private set; }
        public UnitDOF UnitOfDoctorFees { get; private set; }
        public DateTime EffectiveDateFrom { get; private set; }
        public DateTime? EffectiveDateTo { get; private set; }

        public AbstractValidator<DoctorFeesItemPrice> Validator => new DoctorFeesItemPriceValidator();
        public void SetDoctorFees(double price)
        {
            if (DoctorFees == price) return;
            DoctorFees = price;
        }
        public void SetUnitOfDoctorFeesId(int unitOfDoctorFeesId)
        {
            if (UnitOfDoctorFeesId == unitOfDoctorFeesId) return;
            UnitOfDoctorFeesId = unitOfDoctorFeesId;
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
        public void SetIsDeleted(bool isDeleted)
        {
            if (IsDeleted == isDeleted) return;
            IsDeleted = isDeleted;
        }
        public void SetIsDeletedBy(string? isDeletedBy)
        {
            if (IsDeletedBy == isDeletedBy) return;
            IsDeletedBy = isDeletedBy;
        }
        public void SetModifiedOn()
        {
            ModifiedOn = DateTimeOffset.Now;
        }
        public void SetModifiedBy(string? modifiedBy)
        {
            if (ModifiedBy == modifiedBy) return;
            ModifiedBy = modifiedBy;
        }
        public async Task<int> Create(IDoctorFeesItemPriceRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IDoctorFeesItemPriceRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IDoctorFeesItemPriceRepository repository)
        {
            return await repository.Delete(this);
        }

        public static async Task<DoctorFeesItemPrice> Get(int id, IDoctorFeesItemPriceRepository repository)
        {
            var dbDoctorFeesItemPrice = await repository.Get(id);

            if (dbDoctorFeesItemPrice is null)
            {
                throw new DataNotFoundException();
            }

            return dbDoctorFeesItemPrice;
        }
        public static DoctorFeesItemPrice Create(int? id, double doctorFees, int unitOfDoctorFeesId, DateTime effectiveDateFrom, DateTime? effectiveDateTo, string createdBy, string tenantId)
        {
            return new DoctorFeesItemPrice
            {
                Id = id == null ? new int() : (int)id,
                DoctorFees = doctorFees,
                UnitOfDoctorFeesId = unitOfDoctorFeesId,
                EffectiveDateFrom = effectiveDateFrom,
                EffectiveDateTo = effectiveDateTo,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                TenantId = tenantId
            };
        }
        private async Task<bool> EnsureNoDuplicates(IDoctorFeesItemPriceRepository repository, bool throwException = true)
        {
            var DoctorFeesItemPrice = await repository.Get(Id);
            if (Id == default)
            {
                if (DoctorFeesItemPrice is not null)
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (DoctorFeesItemPrice is not null)
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
