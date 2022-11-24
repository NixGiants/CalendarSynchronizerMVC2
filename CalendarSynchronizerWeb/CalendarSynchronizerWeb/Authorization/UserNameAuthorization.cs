using Core.Models;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CalendarSynchronizerWeb.Authorization
{
    public class UserNameAuthorization : AuthorizationHandler<UserNameRequirement>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApplicationDbContext _dbContext;
        public UserNameAuthorization(UserManager<AppUser> userManager, ApplicationDbContext dbContext)
        {
            this.userManager = userManager;
            _dbContext = dbContext;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserNameRequirement requirement)
        {
            string userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _dbContext.AppUsers.FirstOrDefault(u => u.Id == userId);
            var claims = Task.Run(async () => await userManager.GetClaimsAsync(user)).Result;
            var claim = claims.FirstOrDefault(c => c.Type == "UserName");
            if (claim != null)
            {
                if (claim.Value.ToLower().Contains(requirement.Name.ToLower()))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            return Task.CompletedTask;
        }
    }
}
