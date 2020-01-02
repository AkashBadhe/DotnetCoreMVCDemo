using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using myApp.ViewModals;
using myApp.Models;

namespace myApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            if (ModelState.IsValid)
            {
                var newRole = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(newRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var model = new EditRoleViewModel
                {
                    Id = id,
                    Rolename = role.Name
                };

                foreach (var user in userManager.Users.ToList())
                {
                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        model.Users.Add(user.UserName);
                    }
                }
                return View(model);
            }
            else
            {
                ViewBag.Error = $"role with {id} not found";
                return Redirect("Error");
            }

        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role != null)
            {
                role.Name = model.Rolename;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            else
            {
                ViewBag.Error = $"role with {model.Id} not found";
                return Redirect("Error");
            }

        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = ${roleId} not found";
                return View("NotFound");
            }
            else
            {
                var model = new List<UserRoleViewModel>();
                foreach (var user in userManager.Users.ToList())
                {
                    var userRoleViewModel = new UserRoleViewModel()
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };

                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        userRoleViewModel.IsSelected = true;
                    }
                    else
                    {
                        userRoleViewModel.IsSelected = false;
                    }
                    model.Add(userRoleViewModel);
                }
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {roleId} not found";
                return View("NotFound");
            }
            else
            {
                for(var i=0; i< model.Count; i++)
                {
                    var user = await userManager.FindByIdAsync(model[i].UserId);
                    IdentityResult result;
                    if(model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                    {
                        result = await userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if(!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                    {
                        result = await userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }

                    if (result.Succeeded)
                    {
                        if(i < (model.Count - 1))
                        {
                            continue;
                        } else
                        {
                            return RedirectToAction("EditRole", new { Id = roleId });
                        }
                    }
                }
                return RedirectToAction("EditRole", new { Id = roleId });
            }
        }
    }
}