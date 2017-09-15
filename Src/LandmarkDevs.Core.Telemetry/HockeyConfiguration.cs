using Microsoft.HockeyApp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace LandmarkDevs.Core.Telemetry
{
    /// <summary>
    /// Class HockeyConfiguration.
    /// Used to configure HockeyApp for the application.
    /// </summary>
    public static class HockeyConfiguration
    {
        /// <summary>
        /// Searches for the user's information in the Active Directory.
        /// </summary>
        /// <returns>System.String[].</returns>
        private static string[] SearchDirectoryForUserInformation()
        {
            string user;
            string email;
            if (!string.IsNullOrWhiteSpace(UserPrincipal.Current.EmailAddress)
                && !string.IsNullOrWhiteSpace(UserPrincipal.Current.Name))
                return new[] { UserPrincipal.Current.Name, UserPrincipal.Current.EmailAddress };
            var userName = UserPrincipal.Current.Name;
            using (var ds = new DirectorySearcher())
            {
                ds.Filter = "(&(objectClass=user) (cn=" + userName + "))";
                var usr = ds.FindOne();
                if (usr == null)
                    return null;
                using (var de = new DirectoryEntry(usr.Path))
                {
                    email = de.Properties["mail"].Value as string;
                    user = de.Properties["name"].Value as string;
                }
            }
            return new[] { user, email };
        }

        /// <summary>
        /// Configures HockeyApp for the application.
        /// </summary>
        /// <param name="applicationToken">The application token.</param>
        public static void ConfigureHockeyApp(string applicationToken)
        {
            var userName = Environment.UserName;
            try
            {
                var userData = SearchDirectoryForUserInformation();
                userName = userData[0];
                Configure(userData, applicationToken);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.Trim());
                string[] userData = { userName, "UnknownEmail" };
                Configure(userData, applicationToken);
            }
        }

        /// <summary>
        /// Configures HockeyApp for the application.
        /// </summary>
        /// <param name="genericUser">The generic user.</param>
        /// <param name="genericEmail">The generic email.</param>
        /// <param name="applicationToken">The application token.</param>
        public static void ConfigureHockeyApp(string genericUser, string genericEmail, string applicationToken)
        {
            var userName = Environment.UserName;
            try
            {
                var userData = SearchDirectoryForUserInformation();
                userName = userData[0];
                Configure(genericUser, genericEmail, userData, applicationToken);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.Trim());
                string[] userData = { userName, "UnknownEmail" };
                Configure(genericUser, genericEmail, userData, applicationToken);
            }
        }

        private static void Configure(string genericUser, string genericEmail, IReadOnlyList<string> userData, string applicationToken)
        {
            var user = string.IsNullOrWhiteSpace(userData[0]) ? genericUser : userData[0];
            var email = string.IsNullOrWhiteSpace(userData[1]) ? genericEmail : userData[1];
            HockeyClient.Current.Configure(applicationToken).SetContactInfo(user, email);
        }

        private static void Configure(IReadOnlyList<string> userData, string applicationToken)
        {
            Configure("ApplicationUser", "UnknownEmail@UnknownEmail.com", userData, applicationToken);
        }
    }
}