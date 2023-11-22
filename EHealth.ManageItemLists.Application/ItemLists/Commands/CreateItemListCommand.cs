using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System.Data;
using System.Net.Mail;
using System.Net;

namespace EHealth.ManageItemLists.Application.ItemLists.Commands
{
    public record CreateItemListCommand(CreateItemListDto CreateItemListDto) : IRequest<ItemListDto>;
}
