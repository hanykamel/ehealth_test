using EHealth.ManageItemLists.Presentation.Keycloack;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace EHealth.ManageItemLists.Presentation.Authentication
{
    public static class ConfigureAuthentificationServiceExtensions
    {

        private static RsaSecurityKey BuildRSAKey(string publicKeyJWT)
        {
            RSA rsa = RSA.Create();

            rsa.ImportSubjectPublicKeyInfo(

                source: Convert.FromBase64String(publicKeyJWT),
                bytesRead: out _
            );

            var IssuerSigningKey = new RsaSecurityKey(rsa);

            return IssuerSigningKey;
        }
        public static void ConfigureJWTWithKeycloack(this IServiceCollection services, bool IsDevelopment, KeycloackConfig keycloackConfig)
        {
            var AuthenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            AuthenticationBuilder.AddJwtBearer(o =>
            {

                #region == JWT Token Validation ===
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidIssuers = new[] { $"{keycloackConfig.Host}/realms/{keycloackConfig.Realm}" },
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = BuildRSAKey(keycloackConfig.PublicKeyJWT),
                    ValidateLifetime = false
                };
                #endregion
                #region === Event Authentification Handlers ===
                o.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = c =>
                    {
                        Console.WriteLine("User successfully authenticated");
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        c.Response.ContentType = "text/plain";
                        if (IsDevelopment)
                        {
                            return c.Response.WriteAsync(c.Exception.ToString());
                        }
                        return c.Response.WriteAsync("An error occured processing your authentication.");
                    }
                };
                #endregion
            });
        }
    }
}
