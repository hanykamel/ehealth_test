using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Commands.Handlers
{
    public class UpdateProcedureICHIBasicDataCommandHandler : IRequestHandler<UpdateProcedureICHIBasicDataCommand, bool>
    {
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public UpdateProcedureICHIBasicDataCommandHandler(IProcedureICHIRepository procedureICHIRepository,
        IValidationEngine validationEngine,
        IIdentityProvider identityProvider)
        {
            _procedureICHIRepository = procedureICHIRepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(UpdateProcedureICHIBasicDataCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var pocedureICHI = await ProcedureICHI.Get(request.Id, _procedureICHIRepository);
            await ProcedureICHI.IsItemListBusy(_procedureICHIRepository, pocedureICHI.ItemListId);
            pocedureICHI.SetEHealthCode(request.EHealthCode);
            pocedureICHI.SetUHIAId(request.UHIAId);
            pocedureICHI.SetTitleAr(request.TitleAr);
            pocedureICHI.SetTitleEn(request.TitleEn);
            pocedureICHI.SetServiceCategoryId(request.ServiceCategoryId);
            pocedureICHI.SetServiceSubCategoryId(request.ServiceSubCategoryId);
            pocedureICHI.SetDataEffectiveDateFrom(request.DataEffectiveDateFrom);
            pocedureICHI.SetDataEffectiveDateTo(request.DataEffectiveDateTo);
            pocedureICHI.SetLocalSpecialtyDepartmentId(request.LocalSpecialtyDepartmentId);

            return (await pocedureICHI.Update(_procedureICHIRepository, _validationEngine, _identityProvider.GetUserName()));
        }
    }
}
