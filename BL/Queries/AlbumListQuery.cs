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
    public class AlbumListQuery : AppQuery<AlbumDTO>
    {
        public AlbumFilter Filter { get; set; }

        public AlbumListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<AlbumDTO> GetQueryable()
        {
            IQueryable<Album> query = Context.Album;
            if (Filter == null)
            {
                return query
                    .Include(nameof(Album.Artist))
                    .ProjectTo<AlbumDTO>();
            }

            if (Filter.ArtistId > 0)
            {
                query = query.Where(
                    album => (album.Artist.ID == Filter.ArtistId));
            }

            if (!String.IsNullOrEmpty(Filter.Name))
            {
                query = query.Where(
                    album => album.Name.Contains(Filter.Name));
            }

            if (!String.IsNullOrEmpty(Filter.Artist))
            {
                query = query.Where(
                     album => album.Artist.Name.Contains(Filter.Name));
            }

            if (Filter.PublishDateFrom != null)
            {
                query = query.Where(
                    album => album.PublishDate.Value.Year >= Filter.PublishDateFrom.Value.Year);
            }

            if (Filter.PublishDateTo != null)
            {
                query = query.Where(
                    album => album.PublishDate.Value.Year <= Filter.PublishDateTo.Value.Year);
            }

            if (Filter.CreatorId != null)
            {
                query = query.Where(album => album.Creator == Filter.CreatorId);
            }

            return query
                .Include(nameof(Album.Artist))
                .ProjectTo<AlbumDTO>();
        }
    }
}
