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
    public class SongListQuery : AppQuery<SongDTO>
    {
        public SongFilter Filter { get; set; }

        public SongListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<SongDTO> GetQueryable()
        {
            IQueryable<Song> query = Context.Song;

            if (Filter == null)
            {
                return query
                    .Include(nameof(Song.Album))
                    .Include(nameof(Song.Artist))
                    .ProjectTo<SongDTO>();
            }

            if (Filter.Genres != null && Filter.Genres.Any())
            {
                if (Filter.GenresAll == true)
                {
                    query = query.Where(album => (album.Genres.Split(';').Intersect(Filter.Genres).Count() == album.Genres.Length));
                }
                else
                {
                    query = query.Where(album => album.Genres.Split(';').Intersect(Filter.Genres).Any());
                }
            }

            if (!String.IsNullOrEmpty(Filter.Name))
            {
                query = query.Where(
                    song => (song.Name.Contains(Filter.Name)));
            }

            if (!String.IsNullOrEmpty(Filter.ArtistName))
            {
                query = query.Where(
                    song => (song.Artist.Name.Contains(Filter.ArtistName)));
            }

            if (Filter.AlbumId != null)
            {
                query = query.Where(song => song.AlbumPId == Filter.AlbumId);
            }

            if (Filter.CreatorId != null)
            {
                query = query.Where(song => song.Creator == Filter.CreatorId);
            }

            return query
                .Include(nameof(Song.Artist))
                .Include(nameof(Song.Album))
                .ProjectTo<SongDTO>();
        }
    }
}
