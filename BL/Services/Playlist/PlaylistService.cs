using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BL.DTO;
using BL.DTO.Playlist;
using BL.Filters;
using BL.Queries;
using BL.Repositories;
using Castle.Core.Internal;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Services
{
    public class PlaylistService : MusicLibraryService, IPlaylistService
    {
        private readonly PlaylistRepository _playlistRepository;
        private readonly SongRepository _songRepository;
        private readonly GenreRepository _genreRepository;
        private readonly PlaylistListQuery _playlistListQuery;
        public int PlaylistsPageSize = 20;

        public PlaylistService(PlaylistRepository playlistRepository, SongRepository songRepository, PlaylistListQuery playlistListQuery, GenreRepository genreRepository)
        {
            this._playlistRepository = playlistRepository;
            this._playlistListQuery = playlistListQuery;
            this._songRepository = songRepository;
            this._genreRepository = genreRepository;
        }

        public void CreatePlaylist(PlaylistDTO playlistDto)
        {
            if (GetAllPlaylists().Any(x => x.Name == playlistDto.Name))
            {
                return;
            }

            using (var uow = UnitOfWorkProvider.Create())
            {
                var playlist = Mapper.Map<DAL.Entities.Playlist>(playlistDto);
                _playlistRepository.Insert(playlist);
                uow.Commit();
            }
        }

        public void EditPlaylist(PlaylistDTO playlistDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var playlist = Mapper.Map<DAL.Entities.Playlist>(playlistDto);
                _playlistRepository.Update(playlist);
                uow.Commit();
            }
        }

        public void DeletePlaylist(int playlistId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _playlistRepository.Delete(playlistId);
                uow.Commit();
            }
        }

        public PlaylistDTO GetPlaylist(int playlistId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var playlist = _playlistRepository.GetById(playlistId);
                return playlist == null ? null : Mapper.Map<PlaylistDTO>(playlist);
            }
        }

        public IEnumerable<PlaylistDTO> GetAllPlaylists()
        {
            using (UnitOfWorkProvider.Create())
            {
                _playlistListQuery.Filter = null;
                return _playlistListQuery.Execute();
            }
        }

        public PlaylistListQueryResultDTO ListPlaylists(int requestedPage, PlaylistFilter filter = null)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = _playlistListQuery;
                query.Filter = filter;
                query.Skip = Math.Max(0, requestedPage - 1) * PlaylistsPageSize;
                query.Take = PlaylistsPageSize;
                query.AddSortCriteria(x => x.Name, SortDirection.Ascending);

                return new PlaylistListQueryResultDTO
                {
                    RequestedPage = requestedPage,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultsPage = query.Execute() ?? new List<PlaylistDTO>()
                };
            }
        }

        public IEnumerable<PlaylistDTO> GetPlaylistsByFilter(PlaylistFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                _playlistListQuery.Filter = filter;
                return _playlistListQuery.Execute() ?? new List<PlaylistDTO>();
            }
        }

        public IEnumerable<PlaylistDTO> GetPlaylistsByAuthor(Guid authorId)
        {
            using (UnitOfWorkProvider.Create())
            {
                _playlistListQuery.Filter = new PlaylistFilter {CreatorId = authorId};
                return _playlistListQuery.Execute() ?? new List<PlaylistDTO>();
            }
        }

        public void AddSongToPlaylist(int playlist, int song)
        {
            PlaylistDTO newPlaylist = GetPlaylist(playlist);
            if (!newPlaylist.Songs.Contains(song.ToString()))
            {
                newPlaylist.Songs.Add(song.ToString());
                newPlaylist.Songs = newPlaylist.Songs.Where(s => !String.IsNullOrWhiteSpace(s)).ToList();
                EditPlaylist(newPlaylist);
            }
        }

        public IEnumerable<SelectListItem> GetSelectedItems(Guid getGuidByEmail)
        {
            var filter = new PlaylistFilter
            {
                CreatorId = getGuidByEmail
            };

            return GetPlaylistsByFilter(filter).Select(x => new SelectListItem()
            {
                Value = x.ID.ToString(),
                Text = $"{x.Name}"
            });
        }

        public void RemoveSongFromPlaylist(int playlist, int song)
        {
            PlaylistDTO newPlaylist = GetPlaylist(playlist);
            if (newPlaylist.Songs.Contains(song.ToString()))
            {
                newPlaylist.Songs.Remove(song.ToString());
                EditPlaylist(newPlaylist);
            }
        }

        public IEnumerable<PlaylistDTO> GetRecommendedPlaylists(PlaylistDTO playlist)
        {
            List<string> genres = new List<string>();
            List<PlaylistDTO> playlists = new List<PlaylistDTO>();
            foreach (var s in playlist.Songs)
            {
                if (!s.IsNullOrEmpty())
                {
                    foreach (var genre in _songRepository.GetById(Convert.ToInt32(s)).Genres.Split(';'))
                    {
                        if (!String.IsNullOrWhiteSpace(genre))
                        {
                            genres.Add(genre);
                        }
                    }
                }
            }
            var newGenres = genres;
            int count = genres.Count;
            if (!genres.IsNullOrEmpty())
            {
                foreach (var p in GetAllPlaylists())
                {
                    foreach (var s in p.Songs)
                    {
                        foreach (var g in _songRepository.GetById(Convert.ToInt32(s)).Genres.Split(';'))
                        {
                            if (newGenres.Contains(g))
                            {
                                newGenres.Remove(g);
                            }
                            if (newGenres.Count <= count*0.25)
                            {
                                playlists.Add(p);
                                break;
                            }
                        }
                        if (newGenres.Count <= count * 0.25)
                        {
                            newGenres = genres;
                            break;
                        }
                    }
                }
            }
            return playlists;
        }
    }
}
