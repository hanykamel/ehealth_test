using EHealth.ManageItemLists.Application.Procedure.ICHI.Commands.Validators;
using EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Commands
{
    public class UpdateProcedureICHIPricesCommand: UpdateProcedureICHIPriceDto, IRequest<bool>, IValidationModel<UpdateProcedureICHIPricesCommand>
    {
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        public UpdateProcedureICHIPricesCommand(UpdateProcedureICHIPriceDto request, IProcedureICHIRepository procedureICHIRepository)
        {
            ProcedureICHIId = request.ProcedureICHIId;
            ItemListPrices = request.ItemListPrices;
            _procedureICHIRepository = procedureICHIRepository;
        }
        public AbstractValidator<UpdateProcedureICHIPricesCommand> Validator => new UpdateProcedureICHIPricesCommandValidator(_procedureICHIRepository);
    }
}
