using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageHeaders.Commands.Handlers
{
    public class UpdatePackageHeaderCommandHandler : IRequestHandler<UpdatePackageHeaderCommand, bool>
    {
        private readonly IPackageHeaderRepository _packageHeaderRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public UpdatePackageHeaderCommandHandler(IPackageHeaderRepository packageHeaderRepository,
        IValidationEngine validationEngine,
        IIdentityProvider identityProvider)
        {
            _packageHeaderRepository = packageHeaderRepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(UpdatePackageHeaderCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var packageHeader = await PackageHeader.Get(request.Id, _packageHeaderRepository);

            packageHeader.SetUHIACode(request.UHIACode);
            packageHeader.SetNameAr(request.NameAr);
            packageHeader.SetNameEn(request.NameEn);
            packageHeader.SetGlobelPackageTypeId(request.GlobelPackageTypeId);
            packageHeader.SetPackageComplexityClassificationId(request.PackageComplexityClassificationId);
            packageHeader.SetPackageDuration(request.PackageDuration);
            packageHeader.SetPackagePrice(request.PackagePrice);
            packageHeader.SetPackageRoundPrice(request.PackageRoundPrice);
            packageHeader.SetPackageSpecialtyId(request.PackageSpecialtyId);
            packageHeader.SetPackageSubTypeId(request.PackageSubTypeId);
            packageHeader.SetPackageTypeId(request.PackageTypeId);
            packageHeader.SetActivationDateFrom(request.ActivationDateFrom);
            packageHeader.SetActivationDateTo(request.ActivationDateTo);
            packageHeader.SetModifiedBy(_identityProvider.GetUserName());
            packageHeader.SetModifiedOn();

            return (await packageHeader.Update(_packageHeaderRepository, _validationEngine));
        }
    }
}
