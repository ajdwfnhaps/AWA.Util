using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AWA.Util.WebBase.IdentityServer.Client
{
    public static class ClientServiceCollectionExtensions
    {
        /// <summary>
        /// 客户端Oidc预配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="authenticationScheme"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddOpenIdConnectPreset(this AuthenticationBuilder builder, string authenticationScheme, IConfigurationRoot configuration)
        {
            return builder.AddOpenIdConnectPreset(authenticationScheme, configuration, null);
        }

        /// <summary>
        /// 客户端Oidc预配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="authenticationScheme"></param>
        /// <param name="configuration"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddOpenIdConnectPreset(this AuthenticationBuilder builder, string authenticationScheme, IConfigurationRoot configuration, Action<OpenIdConnectOptions> configureOptions)
        {
            return builder
            .AddOpenIdConnect(authenticationScheme, options =>
             {
                 options.GetClaimsFromUserInfoEndpoint = true;
                 options.TokenValidationParameters.NameClaimType = "name";
                 var setting = configuration.GetSection("OIDCOptions");
                 var isScopeNeedPower = false;

                 options.ResponseType = setting?.GetValue<string>("ResponseType");
                 options.Authority = setting?.GetValue<string>("Authority");
                 options.RequireHttpsMetadata = setting.GetValue<bool>("RequireHttpsMetadata");
                 options.ClientId = setting?.GetValue<string>("ClientId");
                 options.SaveTokens = setting.GetValue<bool>("SaveTokens");
                 setting?.GetValue<string>("Scopes")?.Split(',').ToList().ForEach(x =>
                 {
                     options.Scope.Add(x);
                     if (x == "power") isScopeNeedPower = true;
                 });

                 setting?.GetValue<string>("MapJsonKeys")?.Split(',').ToList().ForEach(x =>
                 {
                     options.ClaimActions.MapJsonKey(x, x);
                 });

                 if (isScopeNeedPower)
                 {
                     options.Events.OnUserInformationReceived = context =>
                     {
                         if (context.User.TryGetValue("frontpower", out JToken frontPower))
                         {
                             var claims = new List<Claim>();
                             switch (frontPower.Type)
                             {
                                 case JTokenType.Array:
                                     foreach (var r in frontPower)
                                         claims.Add(new Claim("frontpower", (string)r));
                                     break;

                                 default:
                                     claims.Add(new Claim("frontpower", frontPower.Value<string>()));
                                     break;
                             }
                             var id = context.Principal.Identity as ClaimsIdentity;
                             id.AddClaims(claims);
                         }
                         return Task.CompletedTask;
                     };
                 }

                 configureOptions?.Invoke(options);
             });
        }
    }
}
