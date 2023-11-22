using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands
{
    public class DownloadBulkTemplateCommand : IRequest<byte[]>
    {
        public int ItemListId { get; set; }
        public int ItemListSubtypeId { get; set; }
        public string? EHealthCode { get; set; }
        public string? UHIAId { get; set; }
        public string? ShortDescriptionAr { get; set; }
        public string? ShortDescriptionEn { get; set; }
    }
}
