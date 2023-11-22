using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.DataAccess.Migrations;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
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

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Commands.Handlers
{
    public class UpdateDrugUHIABasicDataCommandHandler : IRequestHandler<UpdateDrugUHIABasicDataCommand, bool>
    {
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public UpdateDrugUHIABasicDataCommandHandler(IDrugsUHIARepository drugsUHIARepository,
        IValidationEngine validationEngine, IIdentityProvider identityProvider)
        {
            _drugsUHIARepository = drugsUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(UpdateDrugUHIABasicDataCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var drugUHIA = await DrugUHIA.Get(request.Id, _drugsUHIARepository);
            drugUHIA.SetEHealthDrugCode(request.EHealthCode);
            drugUHIA.SetLocalDrugCode(request.LocalDrugCode);
            drugUHIA.SetInternationalNonProprietaryName(request.InternationalNonProprietaryName);
            drugUHIA.SetProprietaryName(request.ProprietaryName);
            drugUHIA.SetDosageForm(request.DosageForm);
            drugUHIA.SetRouteOfAdministration(request.RouteOfAdministration);
            drugUHIA.SetManufacturer(request.Manufacturer);
            drugUHIA.SetMarketAuthorizationHolder(request.MarketAuthorizationHolder);
            drugUHIA.SetRegistrationTypeId(request.RegistrationTypeId);
            drugUHIA.SetDrugsPackageTypeId(request.DrugsPackageTypeId);
            drugUHIA.SetMainUnitId(request.MainUnitId);
            drugUHIA.SetNumberOfMainUnit(request.NumberOfMainUnit);
            drugUHIA.SetSubUnitId(request.SubUnitId);
            drugUHIA.SetNumberOfSubunitPerMainUnit(request.NumberOfSubunitPerMainUnit);
            drugUHIA.SetTotalNumberSubunitsOfPack(request.TotalNumberSubunitsOfPack);
            drugUHIA.SetReimbursementCategoryId(request.ReimbursementCategoryId);
            drugUHIA.SetDataEffectiveDateFrom(request.DataEffectiveDateFrom);
            drugUHIA.SetDataEffectiveDateTo(request.DataEffectiveDateTo);

            return (await drugUHIA.Update(_drugsUHIARepository, _validationEngine, _identityProvider.GetUserName()));

        }
    }
}
