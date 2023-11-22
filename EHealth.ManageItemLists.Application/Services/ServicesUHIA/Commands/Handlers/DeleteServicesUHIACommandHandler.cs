using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System.ComponentModel.Design;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Handlers
{
    public class DeleteServicesUHIACommandHandler : IRequestHandler<DeleteServicesUHIACommand, bool>
    {
        private readonly IServiceUHIARepository _serviceUHIARepository;
        private readonly IValidationEngine _validationEngine;

        public DeleteServicesUHIACommandHandler(IServiceUHIARepository serviceUHIARepository, IValidationEngine validationEngine)
        {
            _serviceUHIARepository = serviceUHIARepository;
            _validationEngine = validationEngine;
        }
        public async Task<bool> Handle(DeleteServicesUHIACommand request, CancellationToken cancellationToken)
        {
            var serviceUHIA = await ServiceUHIA.Get(request.Id, _serviceUHIARepository);
            if (serviceUHIA is not null)
            {
                // Throw exception if item list busy
                await ServiceUHIA.IsItemListBusy(_serviceUHIARepository, serviceUHIA.ItemListId);

                serviceUHIA.IsDeleted = true;
                serviceUHIA.IsDeletedBy = "tmp";
                for (int i = 0; i < serviceUHIA.ItemListPrices.Count; i++)
                {
                    var itemPrice = serviceUHIA.ItemListPrices.Where(x => x.Id == serviceUHIA.ItemListPrices[i].Id).FirstOrDefault();
                    if (itemPrice == null)
                    {
                        continue;
                    }

                    serviceUHIA.ItemListPrices[i].SetIsDeleted(true);
                    serviceUHIA.ItemListPrices[i].SetIsDeletedBy("tmp");

                    _validationEngine.Validate(serviceUHIA.ItemListPrices[i]);
                }

                return (await serviceUHIA.Delete(_serviceUHIARepository, _validationEngine));
            }
            else {  throw new DataNotFoundException();  }

        }


    }
}
