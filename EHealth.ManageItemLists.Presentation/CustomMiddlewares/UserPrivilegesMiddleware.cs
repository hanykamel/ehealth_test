using EHealth.ManageItemLists.Application.RolePrivilegeManagement.RolePrivilege.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Redis;
using EHealth.ManageItemLists.Presentation.Identity;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.RegularExpressions;




namespace EHealth.ManageItemLists.Presentation.CustomMiddlewares
{
    public class UserRole
    {
        public List<string> roles { get; set; }
    }

    public class UserPrivilegesMiddleware : IMiddleware
    {

        private readonly ICacheService _cacheService;
        public UserPrivilegesMiddleware(
            ICacheService cacheService
            )
        {
            _cacheService = cacheService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.User != null && context.User.Identity.IsAuthenticated)
            {

                var realm_accessRoles = (context.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == "realm_access")?.Value;
                UserRole userRole = JsonConvert.DeserializeObject<UserRole>(realm_accessRoles);

                var roleNameEn = userRole.roles.Where(x =>
                (!x.Contains("default", StringComparison.OrdinalIgnoreCase)) &&
                x != "offline_access" &&
                x != "uma_authorization").FirstOrDefault();

                Claim realmIdClaim = (context.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.RealmId);
                Claim JWTIDClaim = (context.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "jti");

                if (roleNameEn != null && realmIdClaim != null && JWTIDClaim != null)
                {
                    //var priviliges = await RolePrivilegeBasic.Search(_rolePrivilegeBasicsRepository, f => f.RoleNameEn == roleNameEn && f.TenantId == realmIdClaim.Value, 1, 1, false, null, null);

                    #region Get privileges from chache
                    var priviliges = _cacheService.GetData<IEnumerable<RolePrivilegeDto>>(JWTIDClaim.Value);
                    #endregion

                    if (priviliges != null)
                    {
                        foreach (var item in priviliges)
                        {
                            if (item.ItemSubTypeList.Count > 0)
                            {
                                foreach (var subtype in item.ItemSubTypeList)
                                {
                                    foreach (var priv in item.PrivilegeList)
                                    {
                                        var module = Regex.Replace(item.ModuleNameEn.ToLower(), @"\s", "") + "_" +
                                            Regex.Replace(subtype.ItemTypeEn.ToLower(), @"\s", "") + "_" +
                                            Regex.Replace(subtype.ItemSubTypeEn.ToLower(), @"\s", "") + "_" +
                                            Regex.Replace(priv.NameEn.ToLower(), @"\s", "");

                                        context.User.Identities.FirstOrDefault().AddClaim(new Claim(ClaimTypes.Role, module));
                                    }
                                }
                            }
                            else
                            {
                                foreach (var priv in item.PrivilegeList)
                                {
                                    var module = Regex.Replace(item.ModuleNameEn.ToLower(), @"\s", "") + "_" +
                                       Regex.Replace(priv.NameEn.ToLower(), @"\s", "");
                                    context.User.Identities.FirstOrDefault().AddClaim(new Claim(ClaimTypes.Role, module));
                                }
                            }
                        }
                    }


                }


            }
            await next(context);
        }
    }
}
