using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BL.DTO.UserAccount;
using BL.Filters;
using X.PagedList;

namespace PL.Models
{
    public class VipCodeListViewModel
    {
        public IPagedList<VipCodesDTO> Codes { get; set; }

        public VipCodeFilter Filter { get; set; }
    }
}