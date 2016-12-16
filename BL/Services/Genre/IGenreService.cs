using System.Collections.Generic;
using BL.DTO;
using BL.DTO.Genre;
using BL.Filters;

namespace BL.Services
{
    public interface IGenreService
    {
        /// <summary>
        /// Creates new genre
        /// </summary>
        /// <param name="genreDto">Genre that should be created</param>
        void CreateGenre(GenreDTO genreDto);

        /// <summary>
        /// Updates genre according to <see cref="GenreDTO.ID"/>
        /// </summary>
        /// <param name="genreDto"></param>
        void EditGenre(GenreDTO genreDto);

        /// <summary>
        /// Deletes genre according to <param name="genreId"/>.
        /// </summary>
        void DeleteGenre(int genreId);

        /// <summary>
        /// Returns specific genre according to <param name="genreId"/>
        /// </summary>
        GenreDTO GetGenre(int genreId);

        /// <summary>
        /// Returns all genres
        /// </summary>
        IEnumerable<GenreDTO> GetAllGenres();

        /// <summary>
        /// Get genres according to page and filter
        /// </summary>
        /// <param name="requestedPage">page to display</param>
        /// <param name="filter">genre filter</param>
        /// <returns></returns>
        GenreListQueryResultDTO ListGenres(int requestedPage, GenreFilter filter = null);

        /// <summary>
        /// Returns all genres given by filter
        /// </summary>
        IEnumerable<GenreDTO> GetGenresByFilter(GenreFilter filter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genres"></param>
        /// <returns></returns>
        List<string> FormGenres(IEnumerable<string> genres);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelGenres"></param>
        /// <returns></returns>
        List<string> GetGenreNames(List<string> modelGenres);
    }
}
