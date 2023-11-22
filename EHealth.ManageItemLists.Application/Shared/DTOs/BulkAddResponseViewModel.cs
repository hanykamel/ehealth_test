using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Shared.DTOs
{
    public class BulkAddResponseViewModel
    {
        public int AllRows { get; set; }
        public int UploadedRows { get; set; }
        public int FailedRows { get; set; }
        public byte[] File { get; set; }
    }
}
