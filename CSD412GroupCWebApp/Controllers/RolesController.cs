using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSD412GroupCWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CSD412GroupCWebApp
{
    public class RolesController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Roles
        [Authorize(Roles = Constants.OwnerRole)]
        [Authorize(Roles = Constants.AdministratorRole)]
        public async Task<IActionResult> Index()
        {
            return View(await _roleManager.Roles.ToListAsync());
        }

        // GET: Roles/Edit/5
        [Authorize(Roles = Constants.OwnerRole)]
        [Authorize(Roles = Constants.AdministratorRole)]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var users = _userManager.Users;
            var members = new List<ApplicationUser>();
            var nonMembers = new List<ApplicationUser>();
            foreach (var user in users)
            {
                bool isInRole = await _userManager.IsInRoleAsync(user, role.Name);
                if (isInRole)
                {
                    members.Add(user);
                }
                else
                {
                    nonMembers.Add(user);
                }
            }

            RoleViewModel roleViewModel = new RoleViewModel()
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };

            return View(roleViewModel);
        }

        // POST: Roles/Edit/5
        [Authorize(Roles = Constants.OwnerRole)]
        [Authorize(Roles = Constants.AdministratorRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result;
                foreach (string addedUserId in model.AddedUserIds)
                {
                    var user = await _userManager.FindByIdAsync(addedUserId);
                    if (user != null)
                    {
                        result = await _userManager.AddToRoleAsync(user, model.Role.Name);
                        if (!result.Succeeded)
                        {
                            Errors(result);
                        }
                    }
                }

                foreach (string removedUserId in model.RemovedUserIds)
                {
                    var user = await _userManager.FindByIdAsync(removedUserId);
                    if (user != null)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, model.Role.Name);
                        if (!result.Succeeded)
                        {
                            Errors(result);
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return await Edit(model.Role.Id);
            }
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
