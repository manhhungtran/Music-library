using System;
using System.Collections.Generic;
using BL.DTO.UserAccount;
using BL.Filters;

namespace BL.Services.User
{
    public interface IVipCodeService
    {
        void CreateVip(VipCodesDTO codesDto);

        void RemoveVip(int id);

        IEnumerable<VipCodesDTO> GetAllCodes();

        IEnumerable<VipCodesDTO> GetCodesByFilter(VipCodeFilter filter);

        VipCodesListQueryResultsDTO ListCodes(int requestedPage, VipCodeFilter filter = null);

        VipCodesDTO GetCodeById(Guid id);

        void CreateVip(Guid guid, string name);
    }
}
