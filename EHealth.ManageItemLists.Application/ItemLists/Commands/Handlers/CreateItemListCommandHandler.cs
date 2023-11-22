using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.ItemLists.Commands.Handlers
{

    public class CreateItemListCommandHandler : IRequestHandler<CreateItemListCommand, ItemListDto>
    {
        private readonly IValidationEngine _validationEngine;
        private readonly IItemListRepository _itemListsRepository;
        private readonly IItemListSubtypeRepository _itemSubtypeRepository;
        private readonly IIdentityProvider _identityProvider;

        public CreateItemListCommandHandler(IValidationEngine validationEngine, IItemListRepository itemListsRepository, IItemListSubtypeRepository itemSubtypeRepository, IIdentityProvider identityProvider)
        {
            _validationEngine = validationEngine;
            _itemListsRepository = itemListsRepository;
            _itemSubtypeRepository = itemSubtypeRepository;
            _identityProvider = identityProvider;
        }

        public async Task<ItemListDto> Handle(CreateItemListCommand request, CancellationToken cancellationToken)
        {
            var itemLists = await _itemListsRepository.Search(f => f.ItemListSubtypeId == request.CreateItemListDto.ItemListSubtypeId, 1, 1, null, null, true);
            var itemListSubtype = await _itemSubtypeRepository.GetById(request.CreateItemListDto.ItemListSubtypeId);
            string itemListNumber = $"{itemLists.TotalCount + 1}";
            string Code = $"{itemListSubtype.Code}_{itemListNumber.PadLeft(3, '0')}";
            var itemList = request.CreateItemListDto.ToItemList(Code, _identityProvider.GetUserName(), _identityProvider.GetTenantId());
            await itemList.Create(_itemListsRepository, _validationEngine);

            return ItemListDto.FromItemList(itemList);
        }
    }
}
