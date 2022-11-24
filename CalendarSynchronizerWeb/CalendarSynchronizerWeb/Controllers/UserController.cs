using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CalendarSynchronizerWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        private readonly UserManager<IdentityUser> userManager;

        public UserController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }


        public IActionResult Index()
        {
            var userList = dbContext.Users.ToList();
            var roleList = dbContext.UserRoles.ToList();

            foreach(var user in userList)
            {
                var role = roleList.FirstOrDefault(x => x.Id == user.Id);
                if(role == null)
                {
                    user.Role = "None";
                }
            }
        }
    }
}
