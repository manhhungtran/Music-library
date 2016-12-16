using System;
using System.Data.Entity;
using System.Linq;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTO;
using BL.Filters;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class GenreListQuery : AppQuery<GenreDTO>
    {
        public GenreFilter Filter { get; set; }

        public GenreListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<GenreDTO> GetQueryable()
        {
            IQueryable<Genre> query = Context.Genre;

            if (!String.IsNullOrEmpty(Filter?.Name))
            {
                query = query.Where(
                    genre => genre.Name.Contains(Filter.Name));
            }

            return query.ProjectTo<GenreDTO>();
        }
    }
}
