using System.Collections.Generic;
using BL.DTO;
using BL.DTO.Artist;
using BL.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using SelectListItem = System.Web.Mvc.SelectListItem;

namespace BL.Services
{
    public interface IArtistService
    {
        /// <summary>
        /// Creates new artist
        /// </summary>
        /// <param name="artistDto">Artist that should be created</param>
        void CreateArtist(ArtistDTO artistDto);

        /// <summary>
        /// Updates artist according to <see cref="ArtistDTO.ID"/>
        /// </summary>
        /// <param name="artistDto"></param>
        void EditArtist(ArtistDTO artistDto);

        /// <summary>
        /// Deletes artist according to <param name="artistId"/>.
        /// </summary>
        void DeleteArtist(int artistId);

        /// <summary>
        /// Returns specific artist according to <param name="artistId"/>
        /// </summary>
        ArtistDTO GetArtist(int artistId);

        /// <summary>
        /// Returns all artists
        /// </summary>
        IEnumerable<ArtistDTO> GetAllArtists();

        /// <summary>
        /// Get artists according to page and filter
        /// </summary>
        /// <param name="requestedPage">page to display</param>
        /// <param name="filter">album filter</param>
        /// <returns></returns>
        ArtistListQueryResultDTO ListArtists(int requestedPage, ArtistFilter filter = null);

        /// <summary>
        /// Returns all artists by given filter
        /// </summary>
        IEnumerable<ArtistDTO> GetArtistsByFilter(ArtistFilter filter);

        /// <summary>
        /// Returns all artists by given filter
        /// </summary>
        IEnumerable<SelectListItem> GetArtistBasicsByFilter(ArtistFilter filter);
    }
}
