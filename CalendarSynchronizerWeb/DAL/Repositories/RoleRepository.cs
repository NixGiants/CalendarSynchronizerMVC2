using Core.Models;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleRepository(ApplicationDbContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task Delete(string id)
        {
            var roleDb = dbContext.Roles.FirstOrDefault(r => r.Id == id);
            if (roleDb == null)
            {
                throw new ArgumentException($"No role with id {id}");
            }
            var userRolesForThisRole = dbContext.UserRoles.Where(u => u.RoleId == id).Count();
            if (userRolesForThisRole > 0)
            {
                throw new ConstraintException($"cant delete role which has users");
            }

            await roleManager.DeleteAsync(roleDb);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<IdentityRole>> GetAll()
        {
            return await dbContext.Roles.ToListAsync();
        }

        public async Task<IdentityRole> GetById(string id)
        {
            var role = await dbContext.Roles.FirstOrDefaultAsync(u => u.Id == id);

            if(role == null)
            {
                throw new ArgumentException($"No role with Id {id}");
            }

            return role;
        }

        public async Task Upsert(IdentityRole role)
        {
            if (await roleManager.RoleExistsAsync(role.Name))
            {
                return;
            }

            if (string.IsNullOrEmpty(role.Id))
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = role.Name });
            }
            else
            {
                var roleDb = dbContext.Roles.FirstOrDefault(u => u.Id == role.Id);

                if (roleDb == null)
                {
                    return;
                }

                roleDb.Name = role.Name;
                roleDb.NormalizedName = role.Name.ToUpper();
                var reault = await roleManager.UpdateAsync(roleDb);
            }
        }
    }
}
