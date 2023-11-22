using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.ItemLists.Commands
{
    public record UpdateItemListCommand(UpdateItemListDto UpdateItemListDto) : IRequest<ItemListDto>;
}
