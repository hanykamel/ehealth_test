using EHealth.ManageItemLists.Domain.Shared.Identity;
using System.Security.Claims;

namespace EHealth.ManageItemLists.Presentation.Identity
{
    public class IdentityProvider : IIdentityProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;

        public IdentityProvider(IHttpContextAccessor httpContextAccessor,  IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }
        public string GetTenantId()
        {
            Claim claim = GetClaimsIdentity().Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.RealmId);
            if (claim != null)
            {
                return claim.Value;
            }
            return string.Empty;
        }

        public string GetUserId()
        {
            Claim claim = GetClaimsIdentity().Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                return claim.Value;
            }
            return string.Empty;
        }

        public string GetUserName()
        {
            Claim claim = GetClaimsIdentity().Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                return claim.Value;
            }
            return string.Empty;
        }

        private ClaimsIdentity GetClaimsIdentity()
        {
            return (_httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity);
        }
    }
}
