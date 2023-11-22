using EHealth.ManageItemLists.Application.Lookups.Subtype.DTOs;
using MediatR;

namespace EHealth.ManageItemLists.Application.Lookups.Subtype.Queries
{
    public record GetSubTypeByIdQuery(int Id) : IRequest<SubTypeDto>;

}
