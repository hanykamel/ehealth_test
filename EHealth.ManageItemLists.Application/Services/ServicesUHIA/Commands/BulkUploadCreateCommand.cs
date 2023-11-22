using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands
{
    public class BulkUploadCreateCommand : IRequest<byte[]?>, IValidationModel<BulkUploadCreateCommand>
    {
        public IFormFile file { get; set; }
        public BulkUploadCreateCommand(IFormFile file)
        {
            this.file = file;
        }

        public AbstractValidator<BulkUploadCreateCommand> Validator => new BulkUploadCreateCommandValidator();
    }
}