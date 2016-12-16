using System;
using System.Collections.Generic;
using BL.DTO;
using BL.Utilities.AccountPolicy;

namespace BL.Services.User
{
    /// <summary>
    /// Provides user account related functionality
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Registers user (typically with default claims)
        /// </summary>
        /// <param name="userRegistration">User registration details</param>
        /// <param name="createAdmin">Grant user admin rights</param>
        /// <returns>ID of registered user</returns>
        Guid RegisterUserAccount(UserRegistrationDTO userRegistration, bool createAdmin = false);

        /// <summary>
        /// Authenticates user with given username and password
        /// </summary>
        /// <param name="loginDto">user login details</param>
        /// <returns>ID of the authenticated user</returns>
        Guid AuthenticateUser(UserLoginDTO loginDto);

        /// <summary>
        /// Returns User according to his email
        /// </summary>
        UserAccountDTO GetUserByEmail(string email);

        /// <summary>
        /// Edits user
        /// </summary>
        void ChangePassword(UserPasswordDTO userAccount);

        /// <summary>
        /// Returns user Guid according to his email
        /// </summary>
        Guid GetUserGuidByEmail(string email);

        void PromoteUser(Guid userId, bool giveAdmin = false, bool giveVip = false);

        UserAccountDTO GetUserByGuid(Guid id);
    }
}