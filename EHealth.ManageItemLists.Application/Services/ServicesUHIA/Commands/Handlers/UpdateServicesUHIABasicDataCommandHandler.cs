using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Handlers
{
    public class UpdateServicesUHIABasicDataCommandHandler: IRequestHandler<UpdateServicesUHIABasicDataCommand, bool>
    {
        private readonly IServiceUHIARepository _serviceUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public UpdateServicesUHIABasicDataCommandHandler(IServiceUHIARepository serviceUHIARepository,
        IValidationEngine validationEngine,
        IIdentityProvider identityProvider)
        {
            _serviceUHIARepository = serviceUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(UpdateServicesUHIABasicDataCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var serviceUHIA = await ServiceUHIA.Get(request.Id, _serviceUHIARepository);

            // Throw exception if item list busy
            await ServiceUHIA.IsItemListBusy(_serviceUHIARepository, serviceUHIA.ItemListId);

            serviceUHIA.SetEHealthCode(request.EHealthCode);
            serviceUHIA.SetUHIAId(request.UHIAId);
            serviceUHIA.SetShortDescAr(request.ShortDescAr);
            serviceUHIA.SetShortDescEn(request.ShortDescEn);
            serviceUHIA.SetServiceCategoryId(request.ServiceCategoryId);
            serviceUHIA.SetServiceSubCategoryId(request.ServiceSubCategoryId);
            //serviceUHIA.SetItemListId(request.ItemListId);
            serviceUHIA.SetDataEffectiveDateFrom(request.DataEffectiveDateFrom);
            serviceUHIA.SetDataEffectiveDateTo(request.DataEffectiveDateTo);
            serviceUHIA.SetModifiedBy(_identityProvider.GetUserName());
            serviceUHIA.SetModifiedOn();

            return (await serviceUHIA.Update(_serviceUHIARepository, _validationEngine));
        }
    }
}
