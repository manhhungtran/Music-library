using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BL.DTO;
using BL.DTO.Playlist;
using BL.Filters;

namespace BL.Services
{
    public interface IPlaylistService
    {
        /// <summary>
        /// Creates new playlist
        /// </summary>
        /// <param name="playlistDto">Playlist that should be created</param>
        void CreatePlaylist(PlaylistDTO playlistDto);

        /// <summary>
        /// Updates playlist according to <see cref="PlaylistDTO.ID"/>
        /// </summary>
        /// <param name="playlistDto"></param>
        void EditPlaylist(PlaylistDTO playlistDto);

        /// <summary>
        /// Deletes playlist according to <param name="playlistId"/>.
        /// </summary>
        void DeletePlaylist(int playlistId);

        /// <summary>
        /// Returns specific playlist according to <param name="playlistId"/>
        /// </summary>
        PlaylistDTO GetPlaylist(int playlistId);

        /// <summary>
        /// Returns all playlists
        /// </summary>
        IEnumerable<PlaylistDTO> GetAllPlaylists();

        /// <summary>
        /// Returns all playlists by author
        /// </summary>
        IEnumerable<PlaylistDTO> GetPlaylistsByAuthor(Guid authorId);

        /// <summary>
        /// Get playlists according to page and filter
        /// </summary>
        /// <param name="requestedPage">page to display</param>
        /// <param name="filter">playlist filter</param>
        /// <returns></returns>
        PlaylistListQueryResultDTO ListPlaylists(int requestedPage, PlaylistFilter filter = null);

        /// <summary>
        /// Returns all playlists by filter
        /// </summary>
        IEnumerable<PlaylistDTO> GetPlaylistsByFilter(PlaylistFilter filter);

        /// <summary>
        /// Adds song to given playlist
        /// </summary>
        void AddSongToPlaylist(int playlist, int song);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getGuidByEmail"></param>
        /// <returns></returns>
        IEnumerable<SelectListItem> GetSelectedItems(Guid getGuidByEmail);

        /// <summary>
        /// Removes song from given playlist
        /// </summary>
        void RemoveSongFromPlaylist(int playlist, int song);

        /// <summary>
        /// Get recommended playlists according to genres
        /// If at least 75% of genres match, the list is recommended
        /// </summary>
        /// <param name="playlist">Playlist to find recommendations for</param>
        /// <returns>List of recommended playlists</returns>
        IEnumerable<PlaylistDTO> GetRecommendedPlaylists(PlaylistDTO playlist);

    }
}
