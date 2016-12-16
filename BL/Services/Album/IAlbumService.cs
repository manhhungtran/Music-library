using System.Collections.Generic;
using BL.DTO;
using BL.DTO.Album;
using BL.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using SelectListItem = System.Web.Mvc.SelectListItem;

namespace BL.Services
{
    public interface IAlbumService
    {
        /// <summary>
        /// Creates new album
        /// </summary>
        /// <param name="albumDto">Album that should be created</param>
        void CreateAlbum(AlbumDTO albumDto);

        /// <summary>
        /// Updates album according to <see cref="AlbumDTO.ID"/>
        /// </summary>
        /// <param name="albumDto"></param>
        void EditAlbum(AlbumDTO albumDto);

        /// <summary>
        /// Deletes album according to <param name="albumId"/>.
        /// </summary>
        void DeleteAlbum(int albumId);

        /// <summary>
        /// Returns specific album according to <param name="albumId"/>
        /// </summary>
        AlbumDTO GetAlbum(int albumId);

        /// <summary>
        /// Returns all albums
        /// </summary>
        IEnumerable<AlbumDTO> GetAllAlbums();

        /// <summary>
        /// Get albums according to page and filter
        /// </summary>
        /// <param name="requestedPage">page to display</param>
        /// <param name="filter">album filter</param>
        /// <returns></returns>
        AlbumListQueryResultDTO ListAlbums(int requestedPage, AlbumFilter filter = null);

        /// <summary>
        /// Returns all albums by artist
        /// </summary>
        IEnumerable<AlbumDTO> GetAlbumsByFilter(AlbumFilter filter);

        /// <summary>
        /// Returns all albums by artist
        /// </summary>
        IEnumerable<SelectListItem> GetAlbumBasicsByFilter(AlbumFilter filter);
    }
}
