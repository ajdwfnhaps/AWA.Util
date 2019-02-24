
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using Microsoft.AspNetCore.Builder;

namespace AWA.Util.WebBase.Auth
{
    /// <summary>
    /// Represents options for basic authentication
    /// </summary>
    public class BasicAuthAuthorizationFilterOptions
    {
        public BasicAuthAuthorizationFilterOptions()
        {
            Realm = "Zimmer";
            LoginCaseSensitive = true;
            Users = new BasicAuthAuthorizationUser[] { };
        }

        /// <summary>
        /// Represents realm for authentication
        /// </summary>
        public string Realm { get; set; }

        /// <summary>
        /// Whether or not login checking is case sensitive.
        /// </summary>
        public bool LoginCaseSensitive { get; set; }

        /// <summary>
        /// Represents users list to access Hangfire dashboard.
        /// </summary>
        public IEnumerable<BasicAuthAuthorizationUser> Users { get; set; }
    }

    /// <summary>
    /// Represents user to access path via basic authentication
    /// </summary>
    public class BasicAuthAuthorizationUser
    {
        /// <summary>
        /// Represents user's name
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// SHA1 hashed password
        /// </summary>
        public byte[] Password { get; set; }

        /// <summary>
        /// Setter to update password as plain text
        /// </summary>
        public string PasswordClear
        {
            set
            {
                using (var cryptoProvider = SHA1.Create())
                {
                    Password = cryptoProvider.ComputeHash(Encoding.UTF8.GetBytes(value));
                }
            }
        }

        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="login">User name</param>
        /// <param name="password">User password</param>
        /// <param name="loginCaseSensitive">Whether or not login checking is case sensitive</param>
        /// <returns></returns>
        public bool Validate(string login, string password, bool loginCaseSensitive)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentNullException(nameof(login));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            if (login.Equals(Login, loginCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
            {
                using (var cryptoProvider = SHA1.Create())
                {
                    byte[] passwordHash = cryptoProvider.ComputeHash(Encoding.UTF8.GetBytes(password));
                    return StructuralComparisons.StructuralEqualityComparer.Equals(passwordHash, Password);
                }
            }

            return false;
        }
    }

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBasicAuthenticate(this IApplicationBuilder app, string pathMatch, Action<BasicAuthAuthorizationFilterOptions> setupOptions)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (pathMatch == null)
                throw new ArgumentNullException(nameof(pathMatch));
            if (setupOptions == null)
                throw new ArgumentNullException(nameof(setupOptions));

            var options = new BasicAuthAuthorizationFilterOptions();

            setupOptions(options);

            app.UseMiddleware<BasicAuthenticateMiddleware>(pathMatch, options);

            return app;
        }
    }
}
