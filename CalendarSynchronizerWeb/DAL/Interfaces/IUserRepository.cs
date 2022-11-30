using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<AppUser>> GetAll();
        public Task<AppUser> GetById(string userId);
        public Task<UserClaimsViewModel> GetForClaims(string userId);
        public Task ManageClaims(UserClaimsViewModel viewModel);
        public Task Update(AppUser user, string userId);
        public Task Delete(string userId);
    }
}
