using System.Collections.Generic;
using System.Web.Mvc;
using BL.DTO;
using BL.DTO.Album;
using BL.DTO.Artist;
using BL.DTO.Genre;
using BL.DTO.Playlist;
using BL.DTO.Song;
using BL.Filters;
using BL.Services;

namespace BL.Facades
{
    public class LibraryFacade
    {
        public int PageSize = 20;

        private readonly IAlbumService _albumService;
        private readonly IArtistService _artistService;
        private readonly IGenreService _genreService;
        private readonly ISongService _songService;
        private readonly IPlaylistService _playlistService;

        public LibraryFacade(ISongService songService, IGenreService genreService, IArtistService artistService, IAlbumService albumService, IPlaylistService playlistService)
        {
            _songService = songService;
            _genreService = genreService;
            _artistService = artistService;
            _albumService = albumService;
            _playlistService = playlistService;
        }

        #region CRUD Operations

        #region GET

        public IEnumerable<SongDTO> GetAllSongs()
        {
            return _songService.GetAllSongs();
        }

        public AlbumListQueryResultDTO GetAlbums(AlbumFilter filter = null, int requiredPage = 1)
        {
            return _albumService.ListAlbums(requiredPage, filter);
        }

        public ArtistListQueryResultDTO GetArtists(ArtistFilter filter = null, int requiredPage = 1)
        {
            return _artistService.ListArtists(requiredPage, filter);
        }

        public GenreListQueryResultDTO GetGenres(GenreFilter filter = null, int requiredPage = 1)
        {
            return _genreService.ListGenres(requiredPage, filter);
        }

        public SongListQueryResultDTO GetSongs(SongFilter filter = null, int requiredPage = 1)
        {
            return _songService.ListSongs(requiredPage, filter);
        }

        public PlaylistListQueryResultDTO GetPlaylists(PlaylistFilter filter = null, int requiredPage = 1)
        {
            return _playlistService.ListPlaylists(requiredPage, filter);
        }

        #endregion

        #region "GET BY ID"

        public SongDTO GetSong(int id)
        {
            return _songService.GetSong(id);
        }

        public ArtistDTO GetArtist(int id)
        {
            return _artistService.GetArtist(id);
        }

        public PlaylistDTO GetPlaylist(int id)
        {
            return _playlistService.GetPlaylist(id);
        }

        public AlbumDTO GetAlbum(int id)
        {
            return _albumService.GetAlbum(id);
        }

        public GenreDTO GetGenre(int id)
        {
            return _genreService.GetGenre(id);
        }

        #endregion

        #region EDIT

        public void EditAlbum(AlbumDTO album)
        {
            _albumService.EditAlbum(album);
        }

        public void EditArtist(ArtistDTO artist)
        {
            _artistService.EditArtist(artist);
        }

        public void EditGenre(GenreDTO genre)
        {
            _genreService.EditGenre(genre);
        }

        public void EditSong(SongDTO song)
        {
            _songService.EditSong(song);
        }

        public void EditPlaylist(PlaylistDTO playlist)
        {
            _playlistService.EditPlaylist(playlist);
        }

        #endregion

        #region CREATE

        public void CreateAlbum(AlbumDTO album)
        {
            _albumService.CreateAlbum(album);
        }

        public void CreateArtist(ArtistDTO artist)
        {
            _artistService.CreateArtist(artist);
        }

        public void CreateGenre(GenreDTO genre)
        {
            _genreService.CreateGenre(genre);
        }

        public void CreateSong(SongDTO song)
        {
            _songService.CreateSong(song);
        }

        public void CreatePlaylist(PlaylistDTO playlist)
        {
            _playlistService.CreatePlaylist(playlist);
        }

        #endregion

        #region DELETE

        public void DeleteAlbum(int albumId)
        {
            _albumService.DeleteAlbum(albumId);
        }

        public void DeleteArtist(int artistId)
        {
            _artistService.DeleteArtist(artistId);
        }

        public void DeleteGenre(int genreId)
        {
            _genreService.DeleteGenre(genreId);
        }

        public void DeleteSong(int songId)
        {
            _songService.DeleteSong(songId);
        }

        public void DeletePlaylist(int playlistId)
        {
            _playlistService.DeletePlaylist(playlistId);
        }

        #endregion

        #endregion

        #region BasicViews

        public IEnumerable<SelectListItem> GetAlbumBasicsByFilter(AlbumFilter filter)
        {
            return _albumService.GetAlbumBasicsByFilter(filter);
        }

        public IEnumerable<SelectListItem> GetArtistBasicsByFilter(ArtistFilter filter)
        {
            return _artistService.GetArtistBasicsByFilter(filter);
        }

        public IEnumerable<SelectListItem> GetSongBasicsByFilter(SongFilter filter)
        {
            return _songService.GetSongBasicsByFilter(filter);
        }

        #endregion

        public List<string> FormGenres(IEnumerable<string> genres)
        {
            return _genreService.FormGenres(genres);
        }

        public List<string> GetGenreNames(List<string> modelGenres)
        {
            return _genreService.GetGenreNames(modelGenres);
        }
    }
}
