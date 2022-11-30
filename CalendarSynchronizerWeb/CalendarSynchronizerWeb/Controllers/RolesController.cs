using BLL.Intrfaces;
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
        private readonly IRoleService roleService;
        private readonly ILogger<RolesController> logger;
        public RolesController(IRoleService roleService, ILogger<RolesController> logger)
        {
            this.roleService = roleService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await roleService.GetAll();
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(string id)
        {
            try
            {
                var role = await roleService.GetById(id);
                if (role == null)
                {
                    return View();
                }
                return View(role);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(IdentityRole role)
        {
            try
            {
                await roleService.Upsert(role);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await roleService.Delete(id);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

           return RedirectToAction(nameof(Index));
        }
    }
}
