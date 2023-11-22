using EHealth.ManageItemLists.Application.Lookups.Type.DTOs;
using MediatR;

namespace EHealth.ManageItemLists.Application.Lookups.Type.Queries
{
    public record GetTypeByIdQuery(int Id) : IRequest<TypeDto>;
}
