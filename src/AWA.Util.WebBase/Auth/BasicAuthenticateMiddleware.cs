
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AWA.Util.WebBase.Auth
{
    public class BasicAuthenticateMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<BasicAuthenticateMiddleware> _logger;
        private readonly string _pathMatch;
        private readonly BasicAuthAuthorizationFilterOptions _options;

        public BasicAuthenticateMiddleware(
            RequestDelegate next
            , ILogger<BasicAuthenticateMiddleware> logger
            , string pathMatch
            , BasicAuthAuthorizationFilterOptions options)
        {
            _next = next;
            _logger = logger;
            _pathMatch = pathMatch;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments(_pathMatch) && !Authorize(context))
            {
                return;
            }

            await _next(context);
        }

        private bool Authorize(HttpContext context)
        {
            string header = context.Request.Headers["Authorization"];

            if (string.IsNullOrWhiteSpace(header) == false)
            {
                var authValues = AuthenticationHeaderValue.Parse(header);

                if ("Basic".Equals(authValues.Scheme, StringComparison.OrdinalIgnoreCase))
                {
                    var parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
                    var parts = parameter.Split(':');

                    if (parts.Length > 1)
                    {
                        string login = parts[0];
                        string password = parts[1];

                        if ((string.IsNullOrWhiteSpace(login) == false) && (string.IsNullOrWhiteSpace(password) == false))
                        {
                            return _options.Users
                                    .Any(user => user.Validate(login, password, _options.LoginCaseSensitive)) || Challenge(context, _options.Realm);
                        }
                    }
                }
            }

            return Challenge(context, _options.Realm);
        }

        private bool Challenge(HttpContext context, string realm)
        {
            context.Response.StatusCode = 401;
            context.Response.Headers.Append("WWW-Authenticate", $"Basic realm=\"{realm}\"");

            context.Response.WriteAsync("Authentication is required.");

            return false;
        }
    }
}
