using EHealth.ManageItemLists.Application.Drugs.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Drugs.UHIA.Commands;
using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.Commands.Handlers.Validators;
using EHealth.ManageItemLists.Domain.Shared.Validation;

namespace EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.Commands
{
    public class UpdateFeesOfResourcesPerUnitPackageResourceCommand : UpdateFeesOfResourcesPerUnitPackageResourceDto, IRequest<bool>, IValidationModel<UpdateFeesOfResourcesPerUnitPackageResourceCommand>
    {
        private readonly IFeesOfResourcesPerUnitPackageResourceRepository _feesOfResourcesPerUnitPackageResourceRepository;
        public UpdateFeesOfResourcesPerUnitPackageResourceCommand(UpdateFeesOfResourcesPerUnitPackageResourceDto request, IFeesOfResourcesPerUnitPackageResourceRepository feesOfResourcesPerUnitPackageResourceRepository)
        {
            Id = request.Id;
            FeesOfResourcesPerUnitPackageComponentId = request.FeesOfResourcesPerUnitPackageComponentId;
            ResourceUHIAId = request.ResourceUHIAId;
            Quantity = request.Quantity;
            _feesOfResourcesPerUnitPackageResourceRepository = feesOfResourcesPerUnitPackageResourceRepository;
        }
        public AbstractValidator<UpdateFeesOfResourcesPerUnitPackageResourceCommand> Validator => new UpdateFeesOfResourcesPerUnitPackageResourceValidator(_feesOfResourcesPerUnitPackageResourceRepository);
    }
}
