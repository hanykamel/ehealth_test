using EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageCompomnent.Commands.DTOs;
using EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageCompomnent.Commands.Validators;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageDrugs;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageCompomnent.Commands
{
    public class UpdateDrugItemCommand :UpdateDrugItemDTO, IRequest<bool>, IValidationModel<UpdateDrugItemCommand>
    {
        private readonly IPackageHeaderRepository packageHeaderRepository;
        private readonly IDrugsUHIARepository drugsUHIARepository;
        private readonly ISharedItemsPackageDrugRepository sharedItemsPackageDrugRepository;
        private readonly ILocationsRepository locationsRepository;

        public UpdateDrugItemCommand(UpdateDrugItemDTO request,
                                     IPackageHeaderRepository packageHeaderRepository,
                                     IDrugsUHIARepository drugsUHIARepository,
                                     ISharedItemsPackageDrugRepository sharedItemsPackageDrugRepository,
                                     ILocationsRepository locationsRepository)
        {
            this.packageHeaderRepository = packageHeaderRepository;
            this.drugsUHIARepository = drugsUHIARepository;
            this.sharedItemsPackageDrugRepository = sharedItemsPackageDrugRepository;
            this.locationsRepository = locationsRepository;

            SharedItemsPackageDrugId = request.SharedItemsPackageDrugId;
            PackageHeaderId = request.PackageHeaderId;
            DrugUHIAId = request.DrugUHIAId;
            Quantity = request.Quantity;
            DrugPerCase = request.DrugPerCase;
            NumberOfCasesInTheUnit = request.NumberOfCasesInTheUnit;
            LocationId = request.LocationId;
            TotalCost = request.TotalCost;
        }
        

        public AbstractValidator<UpdateDrugItemCommand> Validator => new UpdateDrugItemCommandValidaor(packageHeaderRepository, drugsUHIARepository, sharedItemsPackageDrugRepository,locationsRepository);
    }
}
