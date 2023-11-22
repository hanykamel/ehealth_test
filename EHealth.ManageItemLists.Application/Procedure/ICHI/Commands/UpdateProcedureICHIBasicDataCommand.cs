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
    public class UpdateProcedureICHIBasicDataCommand : UpdateProcedureICHIBasicDataDto, IRequest<bool>, IValidationModel<UpdateProcedureICHIBasicDataCommand>
    {
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        public UpdateProcedureICHIBasicDataCommand(UpdateProcedureICHIBasicDataDto request, IProcedureICHIRepository procedureICHIRepository)
        {
            Id = request.Id;
            EHealthCode = request.EHealthCode;
            UHIAId = request.UHIAId;
            TitleAr = request.TitleAr;
            TitleEn = request.TitleEn;
            ServiceCategoryId = request.ServiceCategoryId;
            ServiceSubCategoryId = request.ServiceSubCategoryId;
            //ItemListId = request.ItemListId;
            DataEffectiveDateFrom = request.DataEffectiveDateFrom;
            DataEffectiveDateTo = request.DataEffectiveDateTo;
            LocalSpecialtyDepartmentId = request.LocalSpecialtyDepartmentId;
            _procedureICHIRepository = procedureICHIRepository;

        }
        public AbstractValidator<UpdateProcedureICHIBasicDataCommand> Validator => new UpdateProcedureICHIBasicDataCommandValidator(_procedureICHIRepository);
    }
}
