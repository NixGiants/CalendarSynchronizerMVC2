using Core.Models;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<AppUser> userManager;

        public UserRepository(ApplicationDbContext dbContext, UserManager<AppUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task Delete(string userId)
        {
            var user = dbContext.AppUsers.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentException($"No user with id {userId}");
            }

            dbContext.AppUsers.Remove(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<AppUser>> GetAll()
        {
            var userList = await dbContext.AppUsers.ToListAsync();
            var roleList = await dbContext.UserRoles.ToListAsync();
            var roles = await dbContext.Roles.ToListAsync();

            foreach (var user in userList)
            {
                var role = roleList.FirstOrDefault(x => x.UserId == user.Id);
                if (role == null)
                {
                    user.Role = "None";
                }
                else
                {
                    user.Role = roles.FirstOrDefault(u => u.Id == role.RoleId)!.Name;
                }
            }

            return userList;

        }

        public async Task<AppUser> GetById(string userId)
        {
            var user = await dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == userId);

            if(user == null)
            {
                throw new ArgumentException($"No user with ID {userId}");
            }

            var role =  await dbContext.UserRoles.FirstOrDefaultAsync(r => r.UserId == userId);

            if (role != null)
            {
                user.RoleId = dbContext.Roles.FirstOrDefault(r => r.Id == role.RoleId)!.Id;  
            }
            else
            {
                user.RoleId = dbContext.Roles.FirstOrDefault(r => r.Name == "User")!.Id;
            }

            user.RoleList = dbContext.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id,
            });

            return user;
        }

        public async Task<UserClaimsViewModel> GetForClaims(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentException($"No user with ID {userId}");
            }

            var existingUserClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel()
            {
                UserId = userId,
            };

            foreach (Claim claim in ClaimStore.claimList)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type,
                };

                if (existingUserClaims.Any(claim => claim.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                model.Claims.Add(userClaim);
            }

            return model;
        }

        public async Task ManageClaims(UserClaimsViewModel userClaims)
        {
            var user = await userManager.FindByIdAsync(userClaims.UserId);

            if (user == null)
            {
                throw new ArgumentException($"No user with ID {userClaims.UserId}");
            }

            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                throw new Exception("cant delete previous claims");
            }

            result = await userManager.AddClaimsAsync(user,
                userClaims.Claims.Where(cl => cl.IsSelected)
                .Select(c => new Claim(c.ClaimType, c.IsSelected.ToString())));

            if (!result.Succeeded)
            {
                throw new Exception("cant add new claims");
            }
        }

        public async Task Update(AppUser user, string userId)
        {
            var dbUser = await dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == userId);

            if(dbUser == null)
            {
                throw new ArgumentException($"No user with ID {userId}");
            }

            var userRole = dbContext.UserRoles.FirstOrDefault(x => x.UserId == dbUser.Id);

            if (userRole != null)
            {
                var previousRoleName = dbContext.Roles.Where(u => u.Id == userRole.RoleId).Select(o => o.Name).FirstOrDefault();
                await userManager.RemoveFromRoleAsync(dbUser, previousRoleName);
            }

            dbUser.UserName = user.UserName;
            await userManager.AddToRoleAsync(dbUser, dbContext.Roles.FirstOrDefault(u => u.Id == user.RoleId)!.Name);
            await userManager.UpdateAsync(dbUser);
            dbContext.SaveChanges();
        }
    }
}
