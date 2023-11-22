using EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs;
using MediatR;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Queries
{
    public class DrugUHIAGetByIdQuery : IRequest<DrugUHIAGetByIdDto>
    {
        public Guid Id { get; set; }
    }
}
