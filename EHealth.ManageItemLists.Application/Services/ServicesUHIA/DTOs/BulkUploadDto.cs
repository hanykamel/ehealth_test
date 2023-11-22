using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs
{
    public class BulkUploadDto
    {
        public UpdateServicesUHIABasicDataDto UpdateServicesUHIABasicDataDto { get; set; }
        public UpdateServicesUHIAPriceDto UpdateServicesUHIAPriceDto { get; set; }

        public int RowNumber { get; set; }
        public bool IsValid { get; set; }
        public string errors { get; set; }
    }
}
