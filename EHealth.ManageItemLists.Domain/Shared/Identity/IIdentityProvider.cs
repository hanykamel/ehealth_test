using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Identity
{
    public interface IIdentityProvider
    {
        string GetUserId();
        string GetUserName();
        string GetTenantId();
    }
}
