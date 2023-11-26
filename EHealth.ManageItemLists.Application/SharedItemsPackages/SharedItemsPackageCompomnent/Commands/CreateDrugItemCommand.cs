using DocumentFormat.OpenXml.Bibliography;
using EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageCompomnent.Commands.DTOs;
using EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageCompomnent.Commands.Validators;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;

namespace EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageCompomnent.Commands
{
    public class CreateDrugItemCommand : CreateDrugItemDTO,IRequest<Guid>, IValidationModel<CreateDrugItemCommand>
    {
        private readonly IPackageHeaderRepository _packageHeaderRepository;
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        private readonly ILocationsRepository _locationsRepository;

        public CreateDrugItemCommand(CreateDrugItemDTO request, IPackageHeaderRepository packageHeaderRepository,
                                        IDrugsUHIARepository drugsUHIARepository,
                                        ILocationsRepository locationsRepository)
        {
            _packageHeaderRepository = packageHeaderRepository;
            _drugsUHIARepository = drugsUHIARepository;
            _locationsRepository = locationsRepository;

            PackageHeaderId=request.PackageHeaderId;
            DrugUHIAId=request.DrugUHIAId;
            Quantity = request.Quantity;
            DrugPerCase=request.DrugPerCase;
            NumberOfCasesInTheUnit = request.NumberOfCasesInTheUnit;
            LocationId = request.LocationId;
            TotalCost = request.TotalCost;
        }
        public AbstractValidator<CreateDrugItemCommand> Validator => new CreateDrugItemCommandValidaor(_packageHeaderRepository,_drugsUHIARepository,_locationsRepository);
    }
}
