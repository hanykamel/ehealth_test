using EHealth.ManageItemLists.Application.Lookups.Type.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Type.Queries;
using EHealth.ManageItemLists.Domain.ItemListTypes;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.Lookups.Type.Queries.Handlers
{
    public class GetTypeByIdHandler : IRequestHandler<GetTypeByIdQuery, TypeDto>
    {
        private readonly IItemListTypeRepository _iItemListTypeRepository;
        public GetTypeByIdHandler(IItemListTypeRepository iItemListTypeRepository)
        {
            _iItemListTypeRepository = iItemListTypeRepository;
        }
        public async Task<TypeDto> Handle(GetTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var output = await ItemListType.Get(request.Id, _iItemListTypeRepository);

            return TypeDto.FromType(output);

        }
    }
}
