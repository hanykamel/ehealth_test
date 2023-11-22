using EHealth.ManageItemLists.Domain.ItemLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.ItemLists.DTOs
{
    public class UpdateItemListDto
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEN { get; set; }
        public int ItemListSubtypeId { get; set; }
        public bool Active { get; set; }
    }
}
