using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
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

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Commands.Handlers
{
    public class UpdateResourceUHIABasicDataCommandHandler : IRequestHandler<UpdateResourceUHIABasicDataCommand, bool>
    {
        private readonly IResourceUHIARepository _resourceUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public UpdateResourceUHIABasicDataCommandHandler(IResourceUHIARepository resourceUHIARepository,
        IValidationEngine validationEngine, IIdentityProvider identityProvider)
        {
            _resourceUHIARepository = resourceUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(UpdateResourceUHIABasicDataCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var resourceUHIA = await ResourceUHIA.Get(request.Id, _resourceUHIARepository);
            await ResourceUHIA.IsItemListBusy(_resourceUHIARepository, resourceUHIA.ItemListId);
            resourceUHIA.SetEHealthCode(request.EHealthCode);
            resourceUHIA.SetDescriptorAr(request.DescriptorAr);
            resourceUHIA.SetDescriptorEn(request.DescriptorEn);
            resourceUHIA.SetCategoryId(request.CategoryId);
            resourceUHIA.SetSubCategoryId(request.SubCategoryId);
            resourceUHIA.SetDataEffectiveDateFrom(request.DataEffectiveDateFrom);
            resourceUHIA.SetDataEffectiveDateTo(request.DataEffectiveDateTo);

            return (await resourceUHIA.Update(_resourceUHIARepository, _validationEngine, _identityProvider.GetUserName()));
        }
    }
}
