using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageHeaders.Commands.Handlers
{
    public class DeletePackageHeaderCommandHandler : IRequestHandler<DeletePackageHeaderCommand, bool>
    {
        private readonly IPackageHeaderRepository _packageHeaderRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public DeletePackageHeaderCommandHandler(IPackageHeaderRepository packageHeaderRepository, IValidationEngine validationEngine, IIdentityProvider identityProvider )
        {
            _packageHeaderRepository = packageHeaderRepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        async Task<bool> IRequestHandler<DeletePackageHeaderCommand, bool>.Handle(DeletePackageHeaderCommand request, CancellationToken cancellationToken)
        {
            var package = await PackageHeader.Get(request.Id, _packageHeaderRepository);
            _validationEngine.Validate(request);
            
           return await package.Delete(_packageHeaderRepository,_identityProvider.GetUserName() ,_validationEngine);
            
        }
    }
}
