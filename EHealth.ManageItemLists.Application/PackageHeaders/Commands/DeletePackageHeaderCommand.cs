using EHealth.ManageItemLists.Application.PackageHeaders.Commands.Validators;
using EHealth.ManageItemLists.Application.PackageHeaders.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageHeaders.Commands
{
    public class DeletePackageHeaderCommand :DeletePackageHeaderDTO,IRequest<bool>,IValidationModel<DeletePackageHeaderCommand>
    {
        private readonly DeletePackageHeaderDTO _deletePackageHeaderDTO;
        private readonly IPackageHeaderRepository _packageHeaderRepository;
        private readonly IInvestmentCostPackageComponentRepository _investmentCostPackageComponentRepository;

        public DeletePackageHeaderCommand(DeletePackageHeaderDTO deletePackageHeaderDTO,IPackageHeaderRepository packageHeaderRepository,IInvestmentCostPackageComponentRepository investmentCostPackageComponentRepository)
        {
            _deletePackageHeaderDTO = deletePackageHeaderDTO;
            _packageHeaderRepository = packageHeaderRepository;
            _investmentCostPackageComponentRepository = investmentCostPackageComponentRepository;
            Id = deletePackageHeaderDTO.Id;
        }


        public AbstractValidator<DeletePackageHeaderCommand> Validator => new DeletePackageHeaderCommandValidator(_packageHeaderRepository,_investmentCostPackageComponentRepository);
    }
}
