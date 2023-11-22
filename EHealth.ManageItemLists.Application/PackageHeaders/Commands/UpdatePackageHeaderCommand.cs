using DocumentFormat.OpenXml.Office2010.Excel;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Validators;
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
using EHealth.ManageItemLists.Application.PackageHeaders.DTOs;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using EHealth.ManageItemLists.Application.PackageHeaders.Commands.Validators;

namespace EHealth.ManageItemLists.Application.PackageHeaders.Commands
{
    public class UpdatePackageHeaderCommand : UpdatePackageHeaderDto, IRequest<bool>, IValidationModel<UpdatePackageHeaderCommand>
    {
        private readonly IPackageHeaderRepository _packageHeaderRepository;
        public UpdatePackageHeaderCommand(UpdatePackageHeaderDto request, IPackageHeaderRepository packageHeaderRepository)
        {
            Id = request.Id;
            UHIACode = request.UHIACode;
            NameEn = request.NameEn;
            NameAr = request.NameAr;
            PackageTypeId = request.PackageTypeId;
            PackageSubTypeId = request.PackageSubTypeId;
            PackageComplexityClassificationId = request.PackageComplexityClassificationId;
            GlobelPackageTypeId = request.GlobelPackageTypeId;
            PackageSpecialtyId = request.PackageSpecialtyId;
            PackageDuration = request.PackageDuration;
            ActivationDateFrom = request.ActivationDateFrom;
            ActivationDateTo = request.ActivationDateTo;
            PackagePrice = request.PackagePrice;
            PackageRoundPrice = request.PackageRoundPrice;
            _packageHeaderRepository = packageHeaderRepository;
        }

        public AbstractValidator<UpdatePackageHeaderCommand> Validator => new UpdatePackageHeaderCommandValidator(_packageHeaderRepository);
    }
}
