using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Intrfaces
{
    public interface IRoleService
    {
        public Task<List<IdentityRole>> GetAll();

        public Task<IdentityRole?> GetById(string id);

        public Task Delete(string id);

        public Task Upsert(IdentityRole role);
    }
}
