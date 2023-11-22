using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Commands.Validators
{
    public class UpdateResourceUHIABasicDataCommandValidator : AbstractValidator<UpdateResourceUHIABasicDataCommand>
    {
        private readonly IResourceUHIARepository _resourceUHIARepository;
        private bool _validResourceUHIA = false;
        public UpdateResourceUHIABasicDataCommandValidator(IResourceUHIARepository resourceUHIARepository)
        {
            _resourceUHIARepository = resourceUHIARepository;

            RuleFor(x => x.Id).MustAsync(async (ResourceUHIAId, CancellationToken) =>
            {
                try
                {
                    var resourceUHIA = await ResourceUHIA.Get(ResourceUHIAId, _resourceUHIARepository);
                    if (resourceUHIA is not null)
                    {
                        _validResourceUHIA = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }).WithErrorCode("ResourceUHIANotExist").WithMessage("ResourceUHIA with ResourceUHIAId not exist.")
                .When(x => !string.IsNullOrEmpty(x.Id.ToString()));
        }
    }
}
