using EHealth.ManageItemLists.Application.Drugs.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Commands
{
    public class CreateDrugsUHIAPricesCommand : CreateDrugUHIAPricesDto, IRequest<bool>, IValidationModel<CreateDrugsUHIAPricesCommand>
    {
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        public CreateDrugsUHIAPricesCommand(CreateDrugUHIAPricesDto request, IDrugsUHIARepository drugsUHIARepository)
        {
            Id = request.Id;
            ItemListPrices = request.ItemListPrices;
            _drugsUHIARepository = drugsUHIARepository;
        }

        public AbstractValidator<CreateDrugsUHIAPricesCommand> Validator => new CreateDrugsUHIAPricesCommandValidator(_drugsUHIARepository);
    }
}
