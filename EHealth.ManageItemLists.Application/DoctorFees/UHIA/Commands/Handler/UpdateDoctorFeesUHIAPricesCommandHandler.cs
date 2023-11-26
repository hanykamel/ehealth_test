using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands.Handler
{
    public class UpdateDoctorFeesUHIAPricesCommandHandler : IRequestHandler<UpdateDoctorFeesUHIAPricesCommand, bool>
    {
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;
        public UpdateDoctorFeesUHIAPricesCommandHandler(IDoctorFeesUHIARepository doctorFeesUHIARepository, IValidationEngine validationEngine
            , IIdentityProvider identityProvider)
        {
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(UpdateDoctorFeesUHIAPricesCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var serviceUHIA = await DoctorFeesUHIA.Get(request.DoctorFeesUHIAId, _doctorFeesUHIARepository);
            await DoctorFeesUHIA.IsItemListBusy(_doctorFeesUHIARepository, serviceUHIA.ItemListId);
            // prepare model to update and soft delete Item Prices
            for (int i = 0; i < serviceUHIA.ItemListPrices.Count; i++)
            {
                var itemPrice = request.ItemListPrices.Where(x => x.Id == serviceUHIA.ItemListPrices[i].Id).FirstOrDefault();
                if (itemPrice == null)
                {
                    serviceUHIA.ItemListPrices[i].SetIsDeleted(true);
                    serviceUHIA.ItemListPrices[i].SetIsDeletedBy("tmp");
                }
                else
                {
                    serviceUHIA.ItemListPrices[i].SetDoctorFees(itemPrice.DoctorFees);
                    serviceUHIA.ItemListPrices[i].SetUnitOfDoctorFeesId(itemPrice.UnitOfDoctorFeesId);
                    serviceUHIA.ItemListPrices[i].SetEffectiveDateFrom(itemPrice.EffectiveDateFrom);
                    serviceUHIA.ItemListPrices[i].SetEffectiveDateTo(itemPrice.EffectiveDateTo);
                    serviceUHIA.ItemListPrices[i].SetModifiedOn();
                    serviceUHIA.ItemListPrices[i].SetModifiedBy("tmp");
                }
                _validationEngine.Validate(serviceUHIA.ItemListPrices[i]);
            }

            // prepare model to add new Item Prices
            var addItemPrices = request.ItemListPrices.Where(x => x.Id == 0).ToList();
            foreach (var item in addItemPrices)
            {
                var itemListPrice = item.ToDrFeesItemPrice("tmp", "tmp");
                _validationEngine.Validate(itemListPrice);
                serviceUHIA.ItemListPrices.Add(itemListPrice);
            }

            // update data
            await serviceUHIA.Update(_doctorFeesUHIARepository, _validationEngine, _identityProvider.GetUserName());

            return true;
        }
    }
}
