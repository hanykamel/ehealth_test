using EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Queries.Handlers
{
    public class ProcedureICHIGetByIdQueryHandler : IRequestHandler<ProcedureICHIGetByIdQuery, ProcedureICHIGetByIdDto>
    {
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        public ProcedureICHIGetByIdQueryHandler(IProcedureICHIRepository procedureICHIRepository)
        {
            _procedureICHIRepository = procedureICHIRepository;
        }
        public async Task<ProcedureICHIGetByIdDto> Handle(ProcedureICHIGetByIdQuery request, CancellationToken cancellationToken)
        {
            var res = await ProcedureICHI.Get(request.Id, _procedureICHIRepository);
            return ProcedureICHIGetByIdDto.FromProcedureIVHIGetById(res);

        }
    }
}
