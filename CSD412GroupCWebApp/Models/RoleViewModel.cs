using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSD412GroupCWebApp.Models
{
    public class RoleViewModel
    {
        public IdentityRole Role { get; set; }
        public List<ApplicationUser> Members { get; set; }
        public string[] AddedUserIds { get; set; } = new string[0];
        public List<ApplicationUser> NonMembers { get; set; }
        public string[] RemovedUserIds { get; set; } = new string[0];
    }
}
