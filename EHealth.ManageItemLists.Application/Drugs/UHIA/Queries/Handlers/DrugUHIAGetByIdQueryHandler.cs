using EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Queries.Handlers
{
    public class DrugUHIAGetByIdQueryHandler : IRequestHandler<DrugUHIAGetByIdQuery, DrugUHIAGetByIdDto>
    {
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        public DrugUHIAGetByIdQueryHandler(IDrugsUHIARepository drugsUHIARepository)
        {
            _drugsUHIARepository = drugsUHIARepository;
        }
        public async Task<DrugUHIAGetByIdDto> Handle(DrugUHIAGetByIdQuery request, CancellationToken cancellationToken)
        {
            var res = await DrugUHIA.Get(request.Id, _drugsUHIARepository);
            return DrugUHIAGetByIdDto.FromDrugsGetById(res);

        }
    }
}
