using System.Linq;
using BL.Base;
using BL.DTO;
using BL.Services;
using Castle.Windsor;
using NUnit.Framework;

namespace ProgramTest
{
    [TestFixture]
    public class UnitTest1
    {
        private static IAlbumService albumService;
        private static ISongService songService;
        private static IArtistService artistService;
        private static IPlaylistService playlistService;
        private static IEventService eventService;
        private static IGenreService genreService;

        private static readonly IWindsorContainer Container = new WindsorContainer();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Container.Install(new BL.Base.BussinessLayerInstaller());

            MappingInit.ConfigureMapping();

            albumService = Container.Resolve<IAlbumService>();
            songService = Container.Resolve<ISongService>();
            artistService = Container.Resolve<IArtistService>();
            eventService = Container.Resolve<IEventService>();
            genreService = Container.Resolve<IGenreService>();
            playlistService = Container.Resolve<IPlaylistService>();

        }

        [SetUp]
        public void SetUp()
        {
            //artistService.CreateArtist(new ArtistDTO()
            //{
            //    Name = "Artist",
            //});

            //artist = artistService.GetAllArtists().First();
        }

        [Test]
        public void SongServiceTest()
        {
            var art = new ArtistDTO()
            {
                Name = "kokot"
            };
           // artistService.CreateArtist(art);
            
            var newSong1 = new SongDTO
            {
                Name = "new song1",
                Artist = artistService.GetAllArtists().First().ID
        };

            var newSong2 = new SongDTO
            {
                Name = "new song2",
                Artist = artistService.GetAllArtists().First().ID
            };

            var newSong3 = new SongDTO
            {
                Name = "new song3",
                Artist = artistService.GetAllArtists().First().ID
            };


            //songService.CreateSong(newSong1);
            //songService.CreateSong(newSong2);
            //songService.CreateSong(newSong3);

            //var songs = songService.GetAllSongs().Where(x => x.Name == "new song");
            
            //Assert.That(songService.GetAllSongs().Count() == 3);
            //Assert.That(songs.Count() == 1);
        }

/*
        [Test]
        public void EditSongTest()
        {
            songService.EditSong(new SongDTO {ID = songId, Name = "different song"});
            string songName = songService.GetSong(songId).Name;
            Assert.That(songName == "different song");
        }

        [Test]
        public void DeleteSongTest()
        {
            int songId = songService.GetSongIdByName("song");
            songService.DeleteSong(songId);
            int id = songService.GetSongIdByName("song");
            Assert.That(id == -1);
        }

        [Test]
        public void CreateAlbumTest()
        {
            var newAlbum = new AlbumDTO {Name = "new album"};
            albumService.CreateAlbum(newAlbum);
            int albumId = albumService.GetAlbumIdByName("new album");
            Assert.That(albumId != -1);
            //reset db after testing
            albumService.DeleteAlbum(albumId);
        }

        [Test]
        public void GetAlbumTest()
        {
            int albumId = albumService.GetAlbumIdByName("album");
            var foundAlbum = albumService.GetAlbum(albumId);
            Assert.That(foundAlbum != null && foundAlbum.Name == "album");
        }

        [Test]
        public void EditAlbumTest()
        {
            int albumId = albumService.GetAlbumIdByName("album");
            albumService.EditAlbum(new AlbumDTO { ID = albumId, Name = "different album" });
            string albumName = albumService.GetAlbum(albumId).Name;
            Assert.That(albumName == "different album");
        }

        [Test]
        public void DeleteAlbumTest()
        {
            int albumId = albumService.GetAlbumIdByName("album");
            albumService.DeleteAlbum(albumId);
            int id = albumService.GetAlbumIdByName("album");
            Assert.That(id == -1);
        }

        [Test]
        public void CreateArtistTest()
        {
            var newArtist = new ArtistDTO {Name = "new artist"};
            artistService.CreateArtist(newArtist);
            int artistId = artistService.GetArtistIdByName("new artist");
            Assert.That(artistId != -1);
            //reset db after testing
            artistService.DeleteArtist(artistId);
        }

        [Test]
        public void GetArtistTest()
        {
            int artistId = artistService.GetArtistIdByName("artist");
            var foundArtist = artistService.GetArtist(artistId);
            Assert.That(foundArtist != null && foundArtist.Name == "artist");
        }

        [Test]
        public void EditArtistTest()
        {
            int artistId = artistService.GetArtistIdByName("artist");
            artistService.EditArtist(new ArtistDTO { ID = artistId, Name = "different artist" });
            string artistName = artistService.GetArtist(artistId).Name;
            Assert.That(artistName == "different artist");
        }

        [Test]
        public void DeleteArtistTest()
        {
            int artistId = artistService.GetArtistIdByName("artist");
            artistService.DeleteArtist(artistId);
            int id = artistService.GetArtistIdByName("artist");
            Assert.That(id == -1);
        }

        [Test]
        public void CreateEventTest()
        {
            var newEvent = new EventDTO { Name = "new event" };
            eventService.CreateEvent(newEvent);
            int eventId = eventService.GetEventIdByName("new event");
            Assert.That(eventId != -1);
            //reset db after testing
            eventService.DeleteEvent(eventId);
        }

        [Test]
        public void GetEventTest()
        {
            int eventId = eventService.GetEventIdByName("event");
            var foundEvent = eventService.GetEvent(eventId);
            Assert.That(foundEvent != null && foundEvent.Name == "event");
        }

        [Test]
        public void EditEventTest()
        {
            int eventId = eventService.GetEventIdByName("event");
            eventService.EditEvent(new EventDTO { ID = eventId, Name = "different event" });
            string eventName = eventService.GetEvent(eventId).Name;
            Assert.That(eventName == "different event");
        }

        [Test]
        public void DeleteEventTest()
        {
            int eventId = eventService.GetEventIdByName("event");
            eventService.DeleteEvent(eventId);
            int id = eventService.GetEventIdByName("event");
            Assert.That(id == -1);
        }

        [Test]
        public void CreatePlaylistTest()
        {
            var newPlaylist = new PlaylistDTO { Name = "new playlist" };
            playlistService.CreatePlaylist(newPlaylist);
            int playlistId = playlistService.GetPlaylistIdByName("new playlist");
            Assert.That(playlistId != -1);
            //reset db after testing
            playlistService.DeletePlaylist(playlistId);
        }

        [Test]
        public void GetPlaylistTest()
        {
            int playlistId = playlistService.GetPlaylistIdByName("playlist");
            var foundPlaylist = playlistService.GetPlaylist(playlistId);
            Assert.That(foundPlaylist != null && foundPlaylist.Name == "playlist");
        }

        [Test]
        public void EditPlaylistTest()
        {
            int playlistId = playlistService.GetPlaylistIdByName("playlist");
            playlistService.EditPlaylist(new PlaylistDTO { ID = playlistId, Name = "different playlist" });
            string playlistName = playlistService.GetPlaylist(playlistId).Name;
            Assert.That(playlistName == "different playlist");
        }

        [Test]
        public void DeletePlaylistTest()
        {
            int playlistId = playlistService.GetPlaylistIdByName("playlist");
            playlistService.DeletePlaylist(playlistId);
            int id = playlistService.GetPlaylistIdByName("playlist");
            Assert.That(id == -1);
        }
        */
        [Test]
        public void CreateGenreTest()
        {
            var newGenre = new GenreDTO {Name = "new genre"};
            genreService.CreateGenre(newGenre);
            var allGenres = genreService.GetAllGenres();
            Assert.That(allGenres.Count() == 1 && allGenres.Any(x => x.Name == "new genre"));
            genreService.DeleteGenre(allGenres.First().ID);
        }
        /*
        [Test]
        public void GetGenreTest()
        {
            int genreId = genreService.GetGenreIdByName("genre");
            var foundGenre = genreService.GetGenre(genreId);
            Assert.That(foundGenre != null && foundGenre.Name == "genre");
        }

        [Test]
        public void EditGenreTest()
        {
            int genreId = genreService.GetGenreIdByName("genre");
            genreService.EditGenre(new GenreDTO { ID = genreId, Name = "different genre" });
            string genreName = genreService.GetGenre(genreId).Name;
            Assert.That(genreName == "different genre");
        }

        [Test]
        public void DeleteGenreTest()
        {
            int genreId = genreService.GetGenreIdByName("genre");
            genreService.DeleteGenre(genreId);
            int id = genreService.GetGenreIdByName("genre");
            Assert.That(id == -1);
        }
        */
        [TearDown]
        public void TearDown()
        {
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            
            Container.Dispose();
        }
    }
}
