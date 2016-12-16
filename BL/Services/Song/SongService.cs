using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BL.DTO;
using BL.DTO.Song;
using BL.Filters;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Services
{
    public class SongService : MusicLibraryService, ISongService
    {
        private readonly GenreRepository _genreRepository;
        private readonly SongRepository _songRepository;
        private readonly SongListQuery _songListQuery;
        private readonly ArtistRepository _artistRepository;
        private readonly AlbumRepository _albumRepository;
        public int SongsPageSize = 20;

        public SongService(SongRepository songRepository, SongListQuery songListQuery, AlbumRepository albumRepository, ArtistRepository artistRepository, GenreRepository genreRepository)
        {
            this._songRepository = songRepository;
            this._songListQuery = songListQuery;
            this._albumRepository = albumRepository;
            this._artistRepository = artistRepository;
            this._genreRepository = genreRepository;
        }

        public void CreateSong(SongDTO songDto)
        {
            if (GetAllSongs().Any(x => x.Name == songDto.Name && x.Creator == songDto.Creator))
            {
                return;
            }

            using (var uow = UnitOfWorkProvider.Create())
            {
                var song = Mapper.Map<DAL.Entities.Song>(songDto);
                song.Album = _albumRepository.GetById(songDto.AlbumPId);
                song.Artist = _artistRepository.GetById(songDto.ArtistPId);
                _songRepository.Insert(song);
                uow.Commit();
            }
        }

        public void EditSong(SongDTO songDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var song = Mapper.Map<Song>(songDto);
                song.Album = _albumRepository.GetById(songDto.AlbumPId);
                song.Artist = _artistRepository.GetById(songDto.ArtistPId);
                _songRepository.Update(song);
                uow.Commit();
            }
        }

        public void DeleteSong(int songId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _songRepository.Delete(songId);
                uow.Commit();
            }
        }

        public SongDTO GetSong(int songId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var song = _songRepository.GetById(songId);
                return song == null ? null : Mapper.Map<SongDTO>(song);
            }
        }

        public IEnumerable<SongDTO> GetAllSongs()
        {
            using (UnitOfWorkProvider.Create())
            {
                _songListQuery.Filter = null;
                return _songListQuery.Execute() ?? new List<SongDTO>();
            }
        }

        public SongListQueryResultDTO ListSongs(int requestedPage, SongFilter filter = null)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = _songListQuery;
                query.Filter = filter;
                query.Skip = Math.Max(0, requestedPage - 1) * SongsPageSize;
                query.Take = SongsPageSize;
                query.AddSortCriteria(x => x.Name, SortDirection.Ascending);

                return new SongListQueryResultDTO
                {
                    RequestedPage = requestedPage,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultsPage = query.Execute()
                };
            }
        }

        public IEnumerable<SongDTO> GetSongsByFilter(SongFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                _songListQuery.Filter = filter;
                return _songListQuery.Execute() ?? new List<SongDTO>();
            }
        }

        public IEnumerable<SelectListItem> GetSongBasicsByFilter(SongFilter filter)
        {
            var result = GetSongsByFilter(filter);
            return result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.ID.ToString()
            });
        }
    }
}

