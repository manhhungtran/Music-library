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
    public class PlaylistListQuery : AppQuery<PlaylistDTO>
    {
        public PlaylistFilter Filter { get; set; }

        public PlaylistListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<PlaylistDTO> GetQueryable()
        {
            IQueryable<Playlist> query = Context.Playlist;

            if (Filter == null)
            {
                return query
                    .ProjectTo<PlaylistDTO>();
            }

            if (!String.IsNullOrEmpty(Filter.Name))
            {
                query = query.Where(
                    playlist => (playlist.Name.Contains(Filter.Name)));
            }

            if (Filter.CreatorId != null)
            {
                query = query.Where(playlist => playlist.Creator == Filter.CreatorId);
            }
            
            return query
                .ProjectTo<PlaylistDTO>();
        }
    }
}
