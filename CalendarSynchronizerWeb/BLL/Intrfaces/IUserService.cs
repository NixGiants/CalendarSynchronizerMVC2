using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Intrfaces
{
    public interface IUserService
    {
        public Task<List<AppUser>> GetAll(string searchString);
        public Task<AppUser?> GetById(string userId);
        public Task Update(AppUser user, string userId);
        public Task Delete(string userId);
        public Task<UserClaimsViewModel?> GetForClaims(string userId);
        public Task ManageClaims(UserClaimsViewModel viewModel);
    }
}
