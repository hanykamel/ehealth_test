using EHealth.ManageItemLists.Application.ItemLists.Commands;
using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageHeaders.Commands.Handlers
{
    public class CreatePackageHeaderCommandHandler : IRequestHandler<CreatePackageHeaderCommand, Guid>
    {
        private readonly IValidationEngine _validationEngine;
        private readonly IPackageHeaderRepository _packageHeaderRepository;
        private readonly IIdentityProvider _identityProvider;

        public CreatePackageHeaderCommandHandler(IValidationEngine validationEngine, IPackageHeaderRepository packageHeaderRepository,IIdentityProvider identityProvider)
        {
            _validationEngine = validationEngine;
            _packageHeaderRepository = packageHeaderRepository;
            _identityProvider = identityProvider;
        }

        public async Task<Guid> Handle(CreatePackageHeaderCommand request, CancellationToken cancellationToken)
        {
            //double packagePrice = 0.0;
            //double packageRoundPrice = packagePrice - (packagePrice % 5) + 5;
            var packageHeader = request.CreatePackageHeaderDto.ToPackageHeader(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
            await packageHeader.Create(_packageHeaderRepository, _validationEngine);

            return packageHeader.Id;
        }
    }
}
