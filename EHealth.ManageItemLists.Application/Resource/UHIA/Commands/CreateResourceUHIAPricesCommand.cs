using EHealth.ManageItemLists.Application.Resource.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Resource.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Commands
{
    public class CreateResourceUHIAPricesCommand : CreateResourceUHIAPriceDto, IRequest<bool>, IValidationModel<CreateResourceUHIAPricesCommand>
    {
        private readonly IResourceUHIARepository _resourcUHIARepository;
        public CreateResourceUHIAPricesCommand(CreateResourceUHIAPriceDto request, IResourceUHIARepository resourceUHIARepository)
        {
            ResourceUHIAId = request.ResourceUHIAId;
            ResourceItemPrices = request.ResourceItemPrices;
            _resourcUHIARepository = resourceUHIARepository;
        }

        public AbstractValidator<CreateResourceUHIAPricesCommand> Validator => new CreateResourceUHIAPricesCommandValidator(_resourcUHIARepository);

    }
}
