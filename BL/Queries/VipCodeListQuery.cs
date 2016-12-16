using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTO.UserAccount;
using BL.Filters;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class VipCodeListQuery : AppQuery<VipCodesDTO>
    {
        public VipCodeFilter Filter { get; set; }

        public VipCodeListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<VipCodesDTO> GetQueryable()
        {
            IQueryable<VipCodes> query = Context.Codes;

            if (Filter == null)
            {
                return query.ProjectTo<VipCodesDTO>();
            }

            if (!String.IsNullOrWhiteSpace(Filter.Name))
            {
                query = query.Where(x => x.Code.Contains(Filter.Name));
            }

            if (!String.IsNullOrWhiteSpace(Filter.UserName))
            {
                query = query.Where(x => x.Code.Contains(Filter.UserName));
            }

            if (Filter.UserGu != null && Filter.UserGu != Guid.Empty)
            {
                query = query.Where(x => x.User == Filter.UserGu);
            }

            return query.ProjectTo<VipCodesDTO>();
        }
    }
}
