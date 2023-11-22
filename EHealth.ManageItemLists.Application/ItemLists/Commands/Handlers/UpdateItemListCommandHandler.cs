using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.ItemLists.Commands.Handlers
{
    public class UpdateItemListCommandHandler : IRequestHandler<UpdateItemListCommand, ItemListDto>
    {
        private readonly IItemListRepository _itemListsRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public UpdateItemListCommandHandler(IItemListRepository itemListsRepository,
        IValidationEngine validationEngine, IIdentityProvider identityProvider)
        {
            _itemListsRepository = itemListsRepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }

        public async Task<ItemListDto> Handle(UpdateItemListCommand request, CancellationToken cancellationToken)
        {
            var itemList = await ItemList.Get(request.UpdateItemListDto.Id, _itemListsRepository);
            itemList.SetNameAr(request.UpdateItemListDto.NameAr);
            itemList.SetNameEn(request.UpdateItemListDto.NameEN);
            itemList.SetItemListSubtypeId(request.UpdateItemListDto.ItemListSubtypeId);
            itemList.ModifiedOn=DateTimeOffset.Now;
            await itemList.Update(_itemListsRepository, _validationEngine, _identityProvider.GetUserName());
            return ItemListDto.FromItemList(itemList);
        }
    }
}
