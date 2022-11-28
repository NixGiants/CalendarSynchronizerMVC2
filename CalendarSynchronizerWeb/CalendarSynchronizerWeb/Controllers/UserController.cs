using CalendarSynchronizerWeb.ViewModels;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Claims;

namespace CalendarSynchronizerWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        private readonly UserManager<AppUser> userManager;

        public UserController(ApplicationDbContext dbContext, UserManager<AppUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        //[HttpGet]
        public IActionResult Index(string searchString)
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

            if (!String.IsNullOrEmpty(searchString))
            {
                userList = userList.Where(u => u.UserName!.Contains(searchString)).ToList();
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

        [HttpGet]
        public async Task<IActionResult> ManageClaims(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return NotFound();
            }

            var existingUserClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel()
            {
                UserId = userId,
            };

            foreach(Claim claim in ClaimStore.claimList)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type,
                };

                if(existingUserClaims.Any(claim => claim.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                model.Claims.Add(userClaim);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageClaims(UserClaimsViewModel viewModel)
        {
            var user = await userManager.FindByIdAsync(viewModel.UserId);

            if(user == null)
            {
                return NotFound();
            }

            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                return View(viewModel);
            }

            result = await userManager.AddClaimsAsync(user, 
                viewModel.Claims.Where(cl => cl.IsSelected)
                .Select(c => new Claim(c.ClaimType, c.IsSelected.ToString())));

            if (!result.Succeeded)
            {
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
