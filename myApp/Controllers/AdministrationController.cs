using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using myApp.ViewModals;
using myApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace myApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AdministrationController> logger;

        public AdministrationController(RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager, ILogger<AdministrationController> logger)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.logger = logger;
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

        [HttpPost]
        public async Task<IActionResult> Delete(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with id: {roleId} not found";
                return View("Not Found");
            }
            else
            {
                try
                {
                    var result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View("ListRoles");
                    }
                }
                catch (DbUpdateException ex)
                {
                    logger.Log(LogLevel.Error, ex.Message, ex);
                    ViewBag.ErrorTitle = "Role is in use";
                    ViewBag.ErrorMessage = "Role can not be deleted as there are users " +
                        "in this role please delte the user and then try deleting the role.";
                    return View("Error");
                }
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with {userId} not found.";
                return View("NotFound");
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var newUser = new EditUserViewModel()
            {
                Email = user.Email,
                City = user.City,
                Id = user.Id,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles,
                UserName = user.UserName
            };

            return View(newUser);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with {model.Id} not found.";
                return View("NotFound");
            }

            user.Email = model.Email;
            user.City = model.City;
            user.UserName = model.UserName;

            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers", "Administration");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.Id = userId;
            var user = await userManager.FindByIdAsync(userId);
            if(user == null)
            {
                ViewBag.ErrorMessage = "User with id {userId} not found.";
                return View("NotFound");
            }
            var model = new List<UserRolesViewModel>();
            foreach(var role in roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel()
                {
                    RoleName = role.Name,
                    RoleId = role.Id,
                };
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> ManageUserRoles()
        //{
        //    ViewBag.Id = userId;
        //    var user = await userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        ViewBag.ErrorMessage = "User with id {userId} not found.";
        //        return View("NotFound");
        //    }
        //    var model = new List<UserRolesViewModel>();
        //    foreach (var role in roleManager.Roles)
        //    {
        //        var userRolesViewModel = new UserRolesViewModel()
        //        {
        //            RoleName = role.Name,
        //            RoleId = role.Id,
        //        };
        //        if (await userManager.IsInRoleAsync(user, role.Name))
        //        {
        //            userRolesViewModel.IsSelected = true;
        //        }
        //        else
        //        {
        //            userRolesViewModel.IsSelected = false;
        //        }
        //        model.Add(userRolesViewModel);
        //    }
        //    return View(model);
        //}
    }
}