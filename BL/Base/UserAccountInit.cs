using System;
using BL.DTO;
using BL.Services;
using BL.Services.User;
using Castle.Windsor;

namespace BL.Base
{
    public static class UserAccountInit
    {
        /// <summary>
        /// Initializes DB with various user accounts and promo codes
        /// </summary>
        /// <param name="container"></param>
        public static void InitializeUserAccounts(IWindsorContainer container)
        {
            CreateUsers(container);
        }

        /// <summary>
        /// Creates users (admin and customers) for demo eshop
        /// </summary>
        /// <param name="container">The windsor container</param>
        private static void CreateUsers(IWindsorContainer container)
        {
            var userAccountManagementService = container.Resolve<IUserService>();
        }
    }
}
