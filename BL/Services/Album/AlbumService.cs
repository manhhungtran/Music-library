using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BL.DTO;
using BL.DTO.Album;
using BL.Filters;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Services
{
    public class AlbumService : MusicLibraryService, IAlbumService
    {

        private readonly AlbumRepository albumRepository;
        private readonly AlbumListQuery albumListQuery;
        private readonly ArtistRepository artistRepository;
        public int AlbumsPageSize = 20;

        public AlbumService(AlbumRepository albumRepository, AlbumListQuery albumListQuery, ArtistRepository artistRepository)
        {
            this.albumRepository = albumRepository;
            this.albumListQuery = albumListQuery;
            this.artistRepository = artistRepository;
        }

        public void CreateAlbum(AlbumDTO albumDto)
        {
            if (GetAllAlbums().Any(x => x.Name == albumDto.Name && x.Creator == albumDto.Creator))
            {
                return;
            }

            using (var uow = UnitOfWorkProvider.Create())
            {
                var album = Mapper.Map<Album>(albumDto);
                if (albumDto.ArtistPId != null)
                {
                    album.Artist = GetArtist(albumDto.ArtistPId);
                }
                albumRepository.Insert(album);
                uow.Commit();
            }
        }

        private Artist GetArtist(int? artistPId)
        {
            var artist = artistRepository.GetById(artistPId.Value);
            if (artist == null)
            {
                throw new NullReferenceException("Artist does not exist.");
            }
            return artist;
        }

        public void EditAlbum(AlbumDTO albumDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var album = albumRepository.GetById(albumDto.ID);
                Mapper.Map(albumDto, album);
                if (albumDto.ArtistPId != null)
                {
                    album.Artist = GetArtist(albumDto.ArtistPId);
                }
                albumRepository.Update(album);
                uow.Commit();
            }
        }

        public void DeleteAlbum(int albumId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                albumRepository.Delete(albumId);
                uow.Commit();
            }
        }

        public AlbumDTO GetAlbum(int albumId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var album = albumRepository.GetById(albumId);
                return album == null ? null : Mapper.Map<AlbumDTO>(album);
            }
        }

        public IEnumerable<AlbumDTO> GetAllAlbums()
        {
            using (UnitOfWorkProvider.Create())
            {
                albumListQuery.Filter = null;
                return albumListQuery.Execute() ?? new List<AlbumDTO>() ;
            }
        }

        public IEnumerable<AlbumDTO> GetAlbumsByFilter(AlbumFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                albumListQuery.Filter = filter;
                var genre = albumListQuery.Execute();
                return genre;
            }
        }

        public IEnumerable<SelectListItem> GetAlbumBasicsByFilter(AlbumFilter filter)
        {
            var result = GetAlbumsByFilter(filter);
            return result.Select(x => new SelectListItem()
            {
                Value = x.ID.ToString(),
                Text = $"{x.Name}, {x.Artist.Name}"
            });
        }

        public AlbumListQueryResultDTO ListAlbums(int requestedPage, AlbumFilter filter = null)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = albumListQuery;
                query.Filter = filter;
                query.Skip = Math.Max(0, requestedPage - 1) * AlbumsPageSize;
                query.Take = AlbumsPageSize;
                query.AddSortCriteria(x => x.Name, SortDirection.Ascending);

                return new AlbumListQueryResultDTO
                {
                    RequestedPage = requestedPage,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultsPage = query.Execute()
                };
            }
        }
    }
}
