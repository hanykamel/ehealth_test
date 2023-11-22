using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Commands.Handlers
{
    public class DeleteProcedureICHICommandHandler : IRequestHandler<DeleteProcedureICHICommand, bool>
    {
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        private readonly IValidationEngine _validationEngine;
        public DeleteProcedureICHICommandHandler(IProcedureICHIRepository procedureICHIRepository, IValidationEngine validationEngine)
        {
            _procedureICHIRepository = procedureICHIRepository;
            _validationEngine = validationEngine;   
        }
        public async Task<bool> Handle(DeleteProcedureICHICommand request, CancellationToken cancellationToken)
        {
            var ProdcedureICHI = await ProcedureICHI.Get(request.Id, _procedureICHIRepository);
            if (ProdcedureICHI is not null)
            {
                ProdcedureICHI.IsDeleted = true;
                ProdcedureICHI.IsDeletedBy = "tmp";
                for (int i = 0; i < ProdcedureICHI.ItemListPrices.Count; i++)
                {
                    var itemPrice = ProdcedureICHI.ItemListPrices.Where(x => x.Id == ProdcedureICHI.ItemListPrices[i].Id).FirstOrDefault();
                    if (itemPrice == null)
                    {
                        continue;
                    }

                    ProdcedureICHI.ItemListPrices[i].SetIsDeleted(true);
                    ProdcedureICHI.ItemListPrices[i].SetIsDeletedBy("tmp");

                    _validationEngine.Validate(ProdcedureICHI.ItemListPrices[i]);
                }

                return (await ProdcedureICHI.Delete(_procedureICHIRepository, _validationEngine));
            }
            else { throw new DataNotFoundException(); }
        }
    }
}
