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
    public class ArtistListQuery : AppQuery<ArtistDTO>
    {
        public ArtistFilter Filter { get; set; }

        public ArtistListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<ArtistDTO> GetQueryable()
        {
            IQueryable<Artist> query = Context.Artist;

            if (Filter == null)
            {
                return query.ProjectTo<ArtistDTO>();
            }

            if (!String.IsNullOrEmpty(Filter.Name))
            {
                query = query.Where(
                    artist => (artist.Name.Contains(Filter.Name)));
            }

            if (Filter.CreatorId != null)
            {
                query = query.Where(artist => artist.Creator == Filter.CreatorId);
            }

            return query.ProjectTo<ArtistDTO>();
        }
    }
}
