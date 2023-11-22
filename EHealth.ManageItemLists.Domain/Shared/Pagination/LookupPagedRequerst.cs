using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Pagination
{
    public class LookupPagedRequerst : PagedRequerst
    {
        public bool EnablePagination { get; set; } = true;
    }
}
