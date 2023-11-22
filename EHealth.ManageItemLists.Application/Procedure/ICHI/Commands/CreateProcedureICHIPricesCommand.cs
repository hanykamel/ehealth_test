using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs;
using EHealth.ManageItemLists.Application.Procedure.ICHI.Commands.Validators;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Commands
{
    public class CreateProcedureICHIPricesCommand : CreateProcedureICHIPriceDto, IRequest<Guid>, IValidationModel<CreateProcedureICHIPricesCommand>
    {
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        public CreateProcedureICHIPricesCommand(CreateProcedureICHIPriceDto request, IProcedureICHIRepository procedureICHIRepository)
        {
            ProcedureICHIId = request.ProcedureICHIId;
            ItemListPrices = request.ItemListPrices;
            _procedureICHIRepository = procedureICHIRepository;
        }

        public AbstractValidator<CreateProcedureICHIPricesCommand> Validator => new CreateProcedureICHIPricesCommandValidator(_procedureICHIRepository);

    }
}
