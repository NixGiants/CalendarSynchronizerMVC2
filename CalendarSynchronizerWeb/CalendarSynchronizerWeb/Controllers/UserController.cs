using Core.Models;
using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Security.Claims;

namespace CalendarSynchronizerWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        private readonly UserManager<AppUser> userManager;

        public UserController(ApplicationDbContext dbContext, UserManager<AppUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userList = dbContext.AppUsers.ToList();
            var roleList = dbContext.UserRoles.ToList();
            var roles = dbContext.Roles.ToList();

            foreach (var user in userList)
            {
                var role = roleList.FirstOrDefault(x => x.UserId == user.Id);
                if (role == null)
                {
                    user.Role = "None";
                }
                else
                {
                    user.Role = roles.FirstOrDefault(u => u.Id == role.RoleId).Name;
                }
            }

            return View(userList);
        }

        [HttpGet]
        public IActionResult Edit(string userId)
        {
            var user = dbContext.AppUsers.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            var userRole = dbContext.UserRoles.ToList();
            var roles = dbContext.Roles.ToList();
            var role = userRole.FirstOrDefault(x => x.UserId == userId);
            if (role == null)
            {
                user.RoleId = roles.FirstOrDefault(r => r.Name == "User").Id;
            }

            user.RoleId = roles.FirstOrDefault(r => r.Id == role.RoleId).Id;
            user.RoleList = dbContext.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id,
            });
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AppUser user)
        {
            if (ModelState.IsValid)
            {
                var userDb = dbContext.AppUsers.FirstOrDefault(u => u.Id == user.Id);
                if (userDb == null)
                {
                    return NotFound();
                }
                var userRole = dbContext.UserRoles.FirstOrDefault(x => x.UserId == userDb.Id);
                if (userRole != null)
                {
                    var previousRoleName = dbContext.Roles.Where(u => u.Id == userRole.RoleId).Select(o => o.Name).FirstOrDefault();
                    await userManager.RemoveFromRoleAsync(userDb, previousRoleName);
                }
                userDb.UserName = user.UserName;
                await userManager.AddToRoleAsync(userDb, dbContext.Roles.FirstOrDefault(u => u.Id == user.RoleId).Name);
                await userManager.UpdateAsync(userDb);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            user.RoleList = dbContext.Roles.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id
            }); ;
            return View(user);
        }

        [HttpPost]
        public IActionResult Delete(string userId)
        {
            var user = dbContext.AppUsers.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            dbContext.AppUsers.Remove(user);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
