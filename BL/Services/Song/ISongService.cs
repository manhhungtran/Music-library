using System.Collections.Generic;
using System.Web.Mvc;
using BL.DTO;
using BL.DTO.Song;
using BL.Filters;

namespace BL.Services
{
    public interface ISongService {
        /// <summary>
        /// Creates new song
        /// </summary>
        /// <param name="songDto">Song that should be created</param>
        void CreateSong(SongDTO songDto);

        /// <summary>
        /// Updates song according to <see cref="SongDTO.ID"/>
        /// </summary>
        /// <param name="songDto"></param>
        void EditSong(SongDTO songDto);

        /// <summary>
        /// Deletes song according to <param name="songId"/>.
        /// </summary>
        void DeleteSong(int songId);

        /// <summary>
        /// Returns specific song according to <param name="songId"/>
        /// </summary>
        SongDTO GetSong(int songId);

        /// <summary>
        /// Returns all songs
        /// </summary>
        IEnumerable<SongDTO> GetAllSongs();

        /// <summary>
        /// Get songs according to page and filter
        /// </summary>
        /// <param name="requestedPage">page to display</param>
        /// <param name="filter">song filter</param>
        /// <returns></returns>
        SongListQueryResultDTO ListSongs(int requestedPage, SongFilter filter = null);

        /// <summary>
        /// Returns all songs according to filter
        /// </summary>
        IEnumerable<SongDTO> GetSongsByFilter(SongFilter filter);

        /// <summary>
        /// Returns all songs by given filter
        /// </summary>
        IEnumerable<SelectListItem> GetSongBasicsByFilter(SongFilter filter);
    }
}
