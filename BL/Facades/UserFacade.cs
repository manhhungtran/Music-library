using System;
using System.Collections.Generic;
using BL.DTO;
using BL.DTO.UserAccount;
using BL.Filters;
using BL.Services.User;

namespace BL.Facades
{
    public class UserFacade
    {
        #region Dependencies

        private readonly IUserService _userService;
        private readonly IVipCodeService _vipCodeService;

        public UserFacade(IUserService userService, IVipCodeService vipCodeService = null)
        {
            _userService = userService;
            _vipCodeService = vipCodeService;
        }

        public void CreateVip(Guid guid, string name)
        {
            _vipCodeService.CreateVip(guid, name);
        }

        #endregion

        #region User management

        /// <summary>
        /// Gets customer (including its user account) according to email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Customer with specified email</returns>
        public UserAccountDTO GetUserByEmail(string email)
        {
            return _userService.GetUserByEmail(email);
        }

        public Guid RegisterUser(UserRegistrationDTO registrationDto, out bool success)
        {
            if (_userService.GetUserByEmail(registrationDto.Email) != null)
            {
                success = false;
                return new Guid();
            }
            var accountId = _userService.RegisterUserAccount(registrationDto);
            success = true;
            return accountId;
        }

        public Guid AuthenticateUser(UserLoginDTO loginDto)
        {
            return _userService.AuthenticateUser(loginDto);
        }

        public Guid GetGuidByEmail(string email)
        {
            return _userService.GetUserGuidByEmail(email);
        }

        public void ChangePassword(UserPasswordDTO user)
        {
            _userService.ChangePassword(user);
        }

        public void PromoteUser(Guid userId, bool giveAdmin = false, bool giveVip = false)
        {
            _userService.PromoteUser(userId, giveAdmin, giveVip);
        }

        public UserAccountDTO GetUserByGuid(Guid id)
        {
            return _userService.GetUserByGuid(id);
        }

        #endregion

        #region Claim management

        public void RemoveVip(int dto)
        {
            _vipCodeService.RemoveVip(dto);
        }
        public IEnumerable<VipCodesDTO> GetCodesByFilter(VipCodeFilter filter)
        {
            return _vipCodeService.GetCodesByFilter(filter);
        }

        public IEnumerable<VipCodesDTO> GetAllCodes()
        {
            return _vipCodeService.GetAllCodes();
        }

        public VipCodesListQueryResultsDTO ListCodes(VipCodeFilter filter = null, int page = 1)
        {
            return _vipCodeService.ListCodes(page, filter);
        }

        public VipCodesDTO GetCodeById(Guid id)
        {
            return _vipCodeService.GetCodeById(id);
        }
        #endregion

    }
}

