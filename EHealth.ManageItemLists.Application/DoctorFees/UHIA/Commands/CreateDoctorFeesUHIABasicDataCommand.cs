using EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs;
using MediatR;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands
{
    public class CreateDoctorFeesUHIABasicDataCommand : CreateDoctorFeesUHIABasicDataDto, IRequest<Guid>
    {
        public CreateDoctorFeesUHIABasicDataCommand(CreateDoctorFeesUHIABasicDataDto request)
        {
            EHealthCode = request.EHealthCode;
            DescriptorAr = request.DescriptorAr;
            DescriptorEn = request.DescriptorEn;
            ItemListId = request.ItemListId;
            PackageCompexityClassificationId = request.PackageCompexityClassificationId;
            DataEffectiveDateFrom = request.DataEffectiveDateFrom;
            DataEffectiveDateTo = request.DataEffectiveDateTo;
        }
    }
}
