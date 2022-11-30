using BLL.Intrfaces;
using DAL.Interfaces;
using DAL.Repositories;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository roleRepository;
        private readonly ILogger<RoleService> logger;

        public RoleService(IRoleRepository roleRepository, ILogger<RoleService> logger)
        {
            this.roleRepository = roleRepository;
            this.logger = logger;
        }

        public async Task Delete(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Bad Id was given");
            }
            try
            {
                await roleRepository.Delete(id);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }

        public async Task<List<IdentityRole>> GetAll()
        {
            List<IdentityRole> roles = new List<IdentityRole>();

            try
            {
                roles = await roleRepository.GetAll();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return roles;
        }

        public async Task<IdentityRole?> GetById(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Bad Id was given");
            }

            try
            {
                return await roleRepository.GetById(id);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return null;
        }

        public async Task Upsert(IdentityRole role)
        {
            try
            {
                await roleRepository.Upsert(role);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }
    }
}
