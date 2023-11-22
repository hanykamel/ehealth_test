using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Aggregates
{
    public abstract class EHealthDomainObject
    {
        public EHealthDomainObject()
        {
            
        }
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string? IsDeletedBy { get; set;}
        public string TenantId { get; set;}
        public object this[string propertyName]
        {
            get => this.GetType().GetProperty(propertyName).GetValue((object)this, (object[])null);
            set => this.GetType().GetProperty(propertyName).SetValue((object)this, value ?? (object)string.Empty, (object[])null);
        }

        public void SetModifiedBy(string modifiedBy)
        {
            if (ModifiedBy == modifiedBy) return;
            ModifiedBy = modifiedBy;
        }

        public void SetModifiedOn()
        {
            ModifiedOn = DateTimeOffset.Now;
        }

        public void SetIsDeleted(bool isDeleted)
        {
            if (IsDeleted == isDeleted) return;
            IsDeleted = isDeleted;
        }
        public void SetIsDeletedBy(string isDeletedBy)
        {
            if (IsDeletedBy == isDeletedBy) return;
            IsDeletedBy = isDeletedBy;
        }

        public void SoftDelete(string deletedBy)
        {
            IsDeleted = true;
            ModifiedOn = DateTimeOffset.Now;
            IsDeletedBy = deletedBy;
        }
    }
}
