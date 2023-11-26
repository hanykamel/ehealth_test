using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.RolePrivilegeManagement.RolePrivilege.DTOs
{
    public class ItemTypeSubTypeDto
    {
        public string? ItemTypeAr { get; set; }
        public string ItemTypeEn { get; set; }
        public int ItemTypeId { get; set; }
        public string? ItemSubTypeAr { get; set; }
        public string ItemSubTypeEn { get; set; }
        public int ItemSubTypeId { get; set; }
    }
    //public class PrivilegeDto
    //{
    //public int Id { get;  set; }
    //public string NameEn { get;  set; }
    //public string? NameAr { get;  set; }

    //}

    public class PrivilegeDto
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class RolePrivilegeDto
    {
        public int Id { get; set; }
        public string RoleNameEn { get; set; }
        public bool HasAccess { get; set; }
        public string? ModuleNameAr { get; set; }
        public string ModuleNameEn { get; set; }
        public int ModuleId { get; set; }
        public string? PrivTypeNameAr { get; set; }
        public string PrivTypeNameEn { get; set; }
        public int PrivTypeId { get; set; }
        public List<ItemTypeSubTypeDto> ItemSubTypeList { get; set; } = new List<ItemTypeSubTypeDto>();
        public List<PrivilegeDto> PrivilegeList { get; set; } = new List<PrivilegeDto>();
        public bool IsDeleted { get; set; }


     
    }
}
