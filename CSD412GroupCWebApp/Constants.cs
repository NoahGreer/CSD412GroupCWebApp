using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSD412GroupCWebApp
{
    public class Constants
    {
        public const string OwnerEmailAddress = "owner@lwtech.edu";
        public const string OwnerRole = "Owner";
        public const string AdministratorRole = "Administrator";
        public const string AuthorRole = "Author";
        public static string[] RoleNames { get; } = new string[] { OwnerRole, AdministratorRole, AuthorRole };
    }
}
