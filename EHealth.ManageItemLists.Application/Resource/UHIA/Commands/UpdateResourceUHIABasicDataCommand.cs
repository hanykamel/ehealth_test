using EHealth.ManageItemLists.Application.Resource.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Resource.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Commands
{
    public class UpdateResourceUHIABasicDataCommand : UpdateResourceUHIABasicDataDto, IRequest<bool>, IValidationModel<UpdateResourceUHIABasicDataCommand>
    {
        private readonly IResourceUHIARepository _resourceUHIARepository;
        public UpdateResourceUHIABasicDataCommand(UpdateResourceUHIABasicDataDto request, IResourceUHIARepository resourceUHIARepository)
        {
            Id = request.Id;
            EHealthCode = request.EHealthCode;
            DescriptorAr = request.DescriptorAr;
            DescriptorEn = request.DescriptorEn;
            CategoryId = request.CategoryId;
            SubCategoryId = request.SubCategoryId;
            DataEffectiveDateFrom = request.DataEffectiveDateFrom;
            DataEffectiveDateTo = request.DataEffectiveDateTo;
            _resourceUHIARepository = resourceUHIARepository;
        }
        public AbstractValidator<UpdateResourceUHIABasicDataCommand> Validator => new UpdateResourceUHIABasicDataCommandValidator(_resourceUHIARepository);
    }
}
