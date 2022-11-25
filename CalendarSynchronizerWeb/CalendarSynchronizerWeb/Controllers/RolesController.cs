using Core.Models;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CalendarSynchronizerWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public RolesController(ApplicationDbContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = dbContext.Roles.ToList();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Upsert(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            var user = dbContext.Roles.FirstOrDefault(u => u.Id == id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert (IdentityRole role)
        {
            if(await roleManager.RoleExistsAsync(role.Name))
            {
                return RedirectToAction("Index");
            }
            if (string.IsNullOrEmpty(role.Id))
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = role.Name });
            }
            else
            {
                var roleDb = dbContext.Roles.FirstOrDefault(u => u.Id == role.Id);
                if(roleDb == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                roleDb.Name = role.Name;
                roleDb.NormalizedName = role.Name.ToUpper();
                var reault = await roleManager.UpdateAsync(roleDb);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete (string id)
        {
            var roleDb = dbContext.Roles.FirstOrDefault(r => r.Id == id);
            if(roleDb == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var userRolesForThisRoel = dbContext.UserRoles.Where(u => u.RoleId == id).Count();
            if(userRolesForThisRoel > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            await roleManager.DeleteAsync(roleDb);
            return RedirectToAction(nameof(Index));
        }
    }
}
