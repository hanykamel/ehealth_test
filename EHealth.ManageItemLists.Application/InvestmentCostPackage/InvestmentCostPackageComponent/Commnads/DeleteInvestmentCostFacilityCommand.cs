using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Commnads.Validators;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Commnads
{
    public class DeleteInvestmentCostFacilityCommand : IRequest<bool> ,IValidationModel<DeleteInvestmentCostFacilityCommand>
    {
        private readonly IPackageHeaderRepository _packageHeaderRepository;

        public DeleteInvestmentCostFacilityCommand(IPackageHeaderRepository packageHeaderRepository)
        {
            _packageHeaderRepository = packageHeaderRepository;
        }
        public Guid PackageHeaderId { get; set; }

        public AbstractValidator<DeleteInvestmentCostFacilityCommand> Validator => new DeleteInvestmentCostFacilityValidator(_packageHeaderRepository) ;
    }
}
