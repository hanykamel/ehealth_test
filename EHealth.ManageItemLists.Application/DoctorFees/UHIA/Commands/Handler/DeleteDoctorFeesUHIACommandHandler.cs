using EHealth.ManageItemLists.Application.Resource.UHIA.Commands;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands.Handler
{
    public class DeleteDoctorFeesUHIACommandHandler : IRequestHandler<DeleteDoctorFeesUHIACommand, bool>
    {
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public DeleteDoctorFeesUHIACommandHandler(IDoctorFeesUHIARepository doctorFeesUHIARepository, IValidationEngine validationEngine, IIdentityProvider identityProvider)
        {
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(DeleteDoctorFeesUHIACommand request, CancellationToken cancellationToken)
        {
            var doctorFeesUHIA = await DoctorFeesUHIA.Get(request.Id, _doctorFeesUHIARepository);
            if (doctorFeesUHIA is not null)
            {
                doctorFeesUHIA.SoftDelete(_identityProvider.GetUserName());

                for (int i = 0; i < doctorFeesUHIA.ItemListPrices.Count; i++)
                {
                    var itemPrice = doctorFeesUHIA.ItemListPrices.Where(x => x.Id == doctorFeesUHIA.ItemListPrices[i].Id).FirstOrDefault();
                    if (itemPrice == null)
                    {
                        continue;
                    }

                    doctorFeesUHIA.ItemListPrices[i].SoftDelete(_identityProvider.GetUserName());

                    _validationEngine.Validate(doctorFeesUHIA.ItemListPrices[i]);
                }

                return (await doctorFeesUHIA.Delete(_doctorFeesUHIARepository, _validationEngine));
            }
            else { throw new DataNotFoundException(); }

        }

    //    //        return (await serviceUHIA.Delete(_serviceUHIARepository, _validationEngine));
    //    //    }
    //    //    else { throw new DataNotFoundException(); }

    }
}
