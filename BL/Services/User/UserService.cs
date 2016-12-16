using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using BL.DTO;
using BL.Utilities.AccountPolicy;
using BrockAllen.MembershipReboot;

namespace BL.Services.User
{
    /// <summary>
    /// Provides user account related functionality
    /// </summary>
    public class UserService : MusicLibraryService, IUserService
    {
        #region Dependencies

        private readonly UserAccountService<DAL.Entities.UserAccount> _coreService;

        public UserService(UserAccountService<DAL.Entities.UserAccount> service)
        {
            _coreService = service;
        }

        #endregion

        /// <summary>
        /// Registers user (typically with default claims)
        /// </summary>
        /// <param name="userRegistration">User registration details</param>
        /// <param name="createAdmin">Grant user admin rights</param>
        /// <returns>ID of registered user</returns>
        public Guid RegisterUserAccount(UserRegistrationDTO userRegistration, bool createAdmin = false)
        {
            using (UnitOfWorkProvider.Create())
            {
                var userClaims = new List<Claim>();

                if (createAdmin)
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, Claims.Admin));
                }
                else
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, Claims.Standard));
                }

                var account = _coreService.CreateAccount(null, userRegistration.Password, userRegistration.Email, null, null);

                AutoMapper.Mapper.Map(userRegistration, account);

                foreach (var claim in userClaims)
                {
                    _coreService.AddClaim(account.ID, claim.Type, claim.Value);
                }

                _coreService.Update(account);

                return account.ID;
            }
        }

        /// <summary>
        /// Authenticates user with given username and password
        /// </summary>
        /// <param name="loginDto">user login details</param>
        /// <returns>ID of the authenticated user</returns>
        public Guid AuthenticateUser(UserLoginDTO loginDto)
        {
            DAL.Entities.UserAccount account;
            var result = _coreService.Authenticate(loginDto.Username, loginDto.Password, out account);
            if (!result)
            {
                Debug.WriteLine($"Failed to authenticate user: {loginDto.Username}");
                return Guid.Empty;
            }
            return account.ID;
        }

        public UserAccountDTO GetUserByEmail(string email)
        {
            return AutoMapper.Mapper.Map<UserAccountDTO>(_coreService.GetByEmail(email));
        }

        public Guid GetUserGuidByEmail(string email)
        {
            return GetUserByEmail(email).ID;
        }

        public void PromoteUser(Guid userId, bool giveAdmin = false, bool giveVip = false)
        {
            using (UnitOfWorkProvider.Create())
            {
                var userClaims = new List<Claim>();

                if (giveAdmin)
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, Claims.Admin));
                }
                if(giveVip)
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, Claims.Premium));
                }

                foreach (var claim in userClaims)
                {
                    _coreService.AddClaim(userId, claim.Type, claim.Value);
                }
            }
        }

        public UserAccountDTO GetUserByGuid(Guid id)
        {
            return AutoMapper.Mapper.Map<UserAccountDTO>(_coreService.GetByID(id));
        }

        public void ChangePassword(UserPasswordDTO userAccount)
        {
            using (UnitOfWorkProvider.Create())
            {
                _coreService.ChangePassword(GetUserGuidByEmail(userAccount.Username), userAccount.OldPassword, userAccount.NewPassword);
            }
        }


    }
}
