using BLL.Intrfaces;
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
        private readonly IUserService userService;
        private readonly ILogger<UserController> logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        //[HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            var userList = await userService.GetAll(searchString);
            return View(userList);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await userService.GetById(userId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AppUser user)
        {
            if (ModelState.IsValid)
            {
                await userService.Update(user, user.Id);
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string userId)
        {
            await userService.Delete(userId);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ManageClaims(string userId)
        {

            var model = await userService.GetForClaims(userId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageClaims(UserClaimsViewModel viewModel)
        {
            try
            {
                await userService.ManageClaims(viewModel);
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
