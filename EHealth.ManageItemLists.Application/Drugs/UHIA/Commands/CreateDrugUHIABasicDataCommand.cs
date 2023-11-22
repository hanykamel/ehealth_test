using EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Commands
{
    public class CreateDrugUHIABasicDataCommand : CreateDrugUHIABasicDataDto, IRequest<Guid>
    {
        public CreateDrugUHIABasicDataCommand(CreateDrugUHIABasicDataDto request)
        {
            ItemListId = request.ItemListId;
            EHealthCode = request.EHealthCode;
            LocalDrugCode = request.LocalDrugCode;
            InternationalNonProprietaryName = request.InternationalNonProprietaryName;
            ProprietaryName = request.ProprietaryName;
            DosageForm = request.DosageForm;
            RouteOfAdministration = request.RouteOfAdministration;
            Manufacturer = request.Manufacturer;
            MarketAuthorizationHolder = request.MarketAuthorizationHolder;
            RegistrationTypeId = request.RegistrationTypeId;
            DrugsPackageTypeId = request.DrugsPackageTypeId;
            MainUnitId = request.MainUnitId;
            NumberOfMainUnit = request.NumberOfMainUnit;
            SubUnitId = request.SubUnitId;
            NumberOfSubunitPerMainUnit = request.NumberOfSubunitPerMainUnit;
            TotalNumberSubunitsOfPack = request.TotalNumberSubunitsOfPack;
            ReimbursementCategoryId = request.ReimbursementCategoryId;
            DataEffectiveDateFrom = request.DataEffectiveDateFrom;
            DataEffectiveDateTo = request.DataEffectiveDateTo;

        }
    }
}
