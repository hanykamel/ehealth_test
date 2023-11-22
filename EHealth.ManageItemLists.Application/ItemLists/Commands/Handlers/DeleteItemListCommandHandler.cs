using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System.Collections.Generic;

namespace EHealth.ManageItemLists.Application.ItemLists.Commands.Handlers
{
    public class DeleteItemListCommandHandler : IRequestHandler<DeleteItemListCommand, bool>
    {
        private readonly IItemListRepository _itemListRepository;
        private readonly IItemListSubtypeRepository _itemListSubtypeRepository;
        private readonly IServiceUHIARepository _serviceUHIARepository;
        private readonly IValidationEngine _validationEngine;
        public DeleteItemListCommandHandler(IItemListRepository itemListRepository, IItemListSubtypeRepository itemListSubtypeRepository, IServiceUHIARepository serviceUHIARepository,   IValidationEngine validationEngine)
        {

            _itemListRepository = itemListRepository;
            _itemListSubtypeRepository = itemListSubtypeRepository;
            _serviceUHIARepository = serviceUHIARepository;
            _validationEngine = validationEngine;
        }
        public async Task<bool> Handle(DeleteItemListCommand request, CancellationToken cancellationToken)
        {
            
            var res = await ItemList.Get(request.Id, _itemListRepository);
          
            if (res is not null)
            {
                _validationEngine.Validate(request);
                res.Active = false;
                res.IsDeleted = true;
                await res.Delete(_itemListRepository, _validationEngine);
                return true;
              
            }
            else
            {
                return false;
            }
           
        

        }
    }
}
