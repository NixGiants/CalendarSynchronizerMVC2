using BLL.Intrfaces;
using Core.Models;
using DAL.Interfaces;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<UserService> logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task Delete(string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Bad Id was given");
            }
            try
            {
                await userRepository.Delete(userId);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }

        public async Task<List<AppUser>> GetAll(string searchstring)
        {
            List<AppUser> users = new List<AppUser>();

            try
            {
                users = await userRepository.GetAll();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            users = sortFilterList(users, searchstring);
            return users;
        }

        public async Task<AppUser?> GetById(string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("Bad Id was given");
            }

            try
            {
                return await userRepository.GetById(userId);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return null;
        }

        public async Task<UserClaimsViewModel?> GetForClaims(string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("Bad Id was given");
            }

            try
            {
               return await userRepository.GetForClaims(userId);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return null;
        }

        public async Task ManageClaims(UserClaimsViewModel viewModel)
        {
            try
            {
               await userRepository.ManageClaims(viewModel);
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }

        public async Task Update(AppUser user, string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("Bad Id was given");
            }

            try
            {
                await userRepository.Update(user, userId);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }

        private List<AppUser> sortFilterList(List<AppUser> users, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName!.Contains(searchString)).ToList();
            }

            return users;
        }
    }
}
