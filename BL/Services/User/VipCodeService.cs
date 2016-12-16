using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BL.DTO.UserAccount;
using BL.Filters;
using BL.Queries;
using BL.Repositories;
using BL.Repositories.UserAccount;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Services.User
{
    public class VipCodeService : MusicLibraryService, IVipCodeService
    {

        private readonly VipCodeRepository _vipCodeRepository;
        private readonly VipCodeListQuery _vipCodeListQuery;

        private int PageSize = 20;

        public VipCodeService(VipCodeListQuery vipCodeListQuery, VipCodeRepository vipCodeRepository)
        {
            this._vipCodeListQuery = vipCodeListQuery;
            this._vipCodeRepository = vipCodeRepository;
        }

        public void CreateVip(VipCodesDTO codesDto)
        {
            // if there is already genre with same name do nothing
            if (GetAllCodes().Any(x => x.User == codesDto.User))
            {
                throw new ArgumentException("You already requested promotion.");
            }

            using (var uow = UnitOfWorkProvider.Create())
            {
                var genre = Mapper.Map<DAL.Entities.VipCodes>(codesDto);
                _vipCodeRepository.Insert(genre);
                uow.Commit();
            }
        }

        public void RemoveVip(int id)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _vipCodeRepository.Delete(id);
                uow.Commit();
            }
        }

        public IEnumerable<VipCodesDTO> GetAllCodes()
        {
            return GetCodesByFilter(null);
        }

        public IEnumerable<VipCodesDTO> GetCodesByFilter(VipCodeFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = _vipCodeListQuery;
                query.Filter = filter;
                return query.Execute() ?? new List<VipCodesDTO>();
            }
        }

        public VipCodesListQueryResultsDTO ListCodes(int requestedPage, VipCodeFilter filter = null)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = _vipCodeListQuery;
                query.Filter = filter;
                query.Skip = Math.Max(0, requestedPage - 1)*PageSize;
                query.Take = PageSize;
                query.AddSortCriteria(x => x.User, SortDirection.Ascending);

                return new VipCodesListQueryResultsDTO
                {
                    RequestedPage = requestedPage,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultsPage = query.Execute()
                };
            }
        }

        public VipCodesDTO GetCodeById(Guid id)
        {
            var filter = new VipCodeFilter()
            {
                UserGu = id
            };
            return GetCodesByFilter(filter).First();
        }

        public void CreateVip(Guid guid, string name)
        {
            var code = new VipCodesDTO
            {
                User = guid,
                Code = RandomString(50),
                UserName = name
            };

            CreateVip(code);
        }

        private static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
