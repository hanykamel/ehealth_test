using EHealth.ManageItemLists.Application.Drugs.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
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
    public class UpdateDrugUHIAPricesCommand : UpdateDrugUHIAPriceDto, IRequest<bool>, IValidationModel<UpdateDrugUHIAPricesCommand>
    {
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        public UpdateDrugUHIAPricesCommand(UpdateDrugUHIAPriceDto request, IDrugsUHIARepository drugsUHIARepository)
        {
            Id = request.Id;
            drugPrices = request.drugPrices;
            _drugsUHIARepository = drugsUHIARepository;
        }
        public AbstractValidator<UpdateDrugUHIAPricesCommand> Validator => new UpdateDrugUHIAPricesCommandValidator(_drugsUHIARepository);
    }
}
