using EHealth.ManageItemLists.Application.Lookups.Subtype.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Subtype.Queries;
using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.Lookups.Subtype.Queries.Handlers
{
    public class GetSubTypeByIdHandler : IRequestHandler<GetSubTypeByIdQuery, SubTypeDto>
    {
        private readonly IItemListSubtypeRepository _ItemListSubtypeRepository;
        public GetSubTypeByIdHandler(IItemListSubtypeRepository iItemListSubtypeRepository)
        {
            _ItemListSubtypeRepository = iItemListSubtypeRepository;
        }
        public async Task<SubTypeDto> Handle(GetSubTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var output = await ItemListSubtype.Get(request.Id, _ItemListSubtypeRepository);

            return SubTypeDto.FromSubtype(output);

        }
    }
}
