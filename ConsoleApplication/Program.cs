using System;
using System.Collections.Generic;
using System.Linq;
using BL.Base;
using BL.DTO;
using BL.Filters;
using BL.Services;
using Castle.Windsor;

namespace ConsoleApplication
{
    class Program
    {
        private static IAlbumService _albumService;
        private static ISongService _songService;
        private static IArtistService _artistService;
        private static IPlaylistService _playlistService;
        private static IEventService _eventService;
        private static IGenreService _genreService;

        private static readonly IWindsorContainer Container = new WindsorContainer();

        static void Main(string[] args)
        {
            Initialize();

            ResetTheMess();

            //odkomentovat až bude hotový zbytek
            //TestUserAccountService();
            //TestArtistServise();
            //TestGenreService();
            //TestSongService();
            //TestPlaylistService();
            //TestAlbumService();
            //TestEventService();
            var playlist = new PlaylistDTO()
            {
                Creator = Guid.Empty,
                Name = "testPlaylist",
                Songs = new List<string>()
            };
            _playlistService.CreatePlaylist(playlist);

            var selected = _playlistService.GetPlaylistsByAuthor(Guid.Empty).First();

            if (selected.Name == "testPlaylist")
            {
                _playlistService.AddSongToPlaylist(selected.ID, 33);

                selected = _playlistService.GetPlaylistsByAuthor(Guid.Empty).First();
                if (selected.Songs != null && selected.Songs.Contains("33"))
                {
                    Console.WriteLine("OK.");
                }
                else
                {
                    Console.WriteLine("nok.");
                }
            }



            //pokud se něco posere
            ResetTheMess();
        }

        private static void TestGenreService()
        {

            var genre1 = new GenreDTO
            {
                Name = "testgenre1"
            };

            var genre2 = new GenreDTO
            {
                Name = "testgenre2"
            };

            var genre3 = new GenreDTO
            {
                Name = "testgenre3"
            };

            _genreService.CreateGenre(genre1);
            _genreService.CreateGenre(genre2);
            _genreService.CreateGenre(genre3);

            var all = _genreService.GetAllGenres().ToList();

            if (all.All(x => x.Name != "testgenre1") 
                || all.All(x => x.Name != "testgenre2") 
                || all.All(x => x.Name != "testgenre3"))
            {
                Console.WriteLine("GenreService - Create + GetAll - NOK");
            }
            else
            {
                var edit = all.First(x => x.Name == "testgenre1");
                edit.Name = "testeditedgenre";
                _genreService.EditGenre(edit);

                if (_genreService.GetAllGenres().Count(x => x.Name.StartsWith("test")) != 3)
                {
                    Console.WriteLine("GenreService - Edit + GetAll - NOK");
                }
                else
                {

                    var filter = new GenreFilter
                    {
                        Name = "testeditedgenre"
                    };

                    if (_genreService.GetGenresByFilter(filter).Count(x => x.Name.StartsWith("test")) != 1)
                    {
                        Console.WriteLine("GenreService - ListByFilter - NOK");
                    }
                    else
                    {
                        foreach (GenreDTO dto in all)
                        {
                            if (_genreService.GetGenre(dto.ID).Name != dto.Name)
                            {
                                Console.WriteLine("GenreService - Delete + GetArtist - NOK");
                            }
                        }

                        Console.WriteLine(
                            _genreService.GetGenresByFilter(new GenreFilter { Name = "áýřžšáířž420šžřáíšě" }).Any()
                                ? "GenreService - EmptyFilter - NOK"
                                : "GenreService - OK");
                    }
                }
            }
        }

        private static void TestArtistServise()
        {
            var artist1 = new ArtistDTO
            {
                Name = "testartist1"
            };
            var artist2 = new ArtistDTO
            {
                Name = "testartist2"
            };
            var artist3 = new ArtistDTO
            {
                Name = "testartist3"
            };

            _artistService.CreateArtist(artist1);
            _artistService.CreateArtist(artist2);
            _artistService.CreateArtist(artist3);

            var all = _artistService.GetAllArtists().ToList();

            if (all.All(x => x.Name != "testartist1")
                || all.All(x => x.Name != "testartist2")
                || all.All(x => x.Name != "testartist3"))
            {
                Console.WriteLine("ArtistService-Create+GetAll - NOK");
            }
            else
            {
                var edit = all.First(x => x.Name == "testartist1");
                edit.Name = "testeditedArtist";
                _artistService.EditArtist(edit);

                if (_artistService.GetAllArtists().Count(x => x.Name.StartsWith("test")) != 3)
                {
                    Console.WriteLine("ArtistService - Edit + GetAll - NOK");
                }
                else
                {

                    var filter = new ArtistFilter
                    {
                        Name = "testeditedArtist"
                    };

                    if (_artistService.GetArtistsByFilter(filter).Count(x => x.Name.StartsWith("test")) != 1)
                    {
                        Console.WriteLine("ArtistService - ListByFilter - NOK");
                    }
                    else
                    {
                        foreach (ArtistDTO dto in all)
                        {
                            if (_artistService.GetArtist(dto.ID).Name != dto.Name)
                            {
                                Console.WriteLine("ArtistService - Delete + GetArtist - NOK");
                            }
                        }

                        Console.WriteLine(
                            _artistService.GetArtistsByFilter(new ArtistFilter {Name = "áýřžšáířž420šžřáíšě"}).Any()
                                ? "ArtistService - EmptyFilter - NOK"
                                : "ArtistService - OK");
                    }
                }
            }


        }

        private static void TestSongService()
        {

            var song1 = new SongDTO
            {
                Name = "testsong1"
            };

            var song2 = new SongDTO
            {
                Name = "testsong2"
            };

            var song3 = new SongDTO
            {
                Name = "testsong3"
            };

            _songService.CreateSong(song1);
            _songService.CreateSong(song2);
            _songService.CreateSong(song3);

            var all = _songService.GetAllSongs().ToList();

            if (all.All(x => x.Name != "testsong1")
                || all.All(x => x.Name != "testsong2")
                || all.All(x => x.Name != "testsong3"))
            {
                Console.WriteLine("SongService - Create + GetAll - NOK");
            }
            else
            {
                var edit = all.First(x => x.Name == "testsong1");
                edit.Name = "testeditedsong";
                _songService.EditSong(edit);

                if (_songService.GetAllSongs().Count(x => x.Name.StartsWith("test")) != 3)
                {
                    Console.WriteLine("SongService - Edit + GetAll - NOK");
                }
                else
                {

                    var filter = new SongFilter
                    {
                        Name = "testeditedsong"
                    };

                    if (_songService.GetSongsByFilter(filter).Count(x => x.Name.StartsWith("test")) != 1)
                    {
                        Console.WriteLine("SongService - ListByFilter - NOK");
                    }
                    else
                    {
                        foreach (SongDTO dto in all)
                        {
                            if (_songService.GetSong(dto.ID).Name != dto.Name)
                            {
                                Console.WriteLine("SongService - Delete + GetArtist - NOK");
                            }
                        }

                        Console.WriteLine(
                            _songService.GetSongsByFilter(new SongFilter { Name = "áýřžšáířž420šžřáíšě" }).Any()
                                ? "SongService - EmptyFilter - NOK"
                                : "SongService - OK");
                    }
                }
            }
        }

        private static void TestPlaylistService()
        {

            var playlist1 = new PlaylistDTO
            {
                Name = "testplaylist1"
            };

            var playlist2 = new PlaylistDTO
            {
                Name = "testplaylist2"
            };

            var playlist3 = new PlaylistDTO
            {
                Name = "testplaylist3"
            };

            _playlistService.CreatePlaylist(playlist1);
            _playlistService.CreatePlaylist(playlist2);
            _playlistService.CreatePlaylist(playlist3);

            var all = _playlistService.GetAllPlaylists().ToList();

            if (all.All(x => x.Name != "testplaylist1")
                || all.All(x => x.Name != "testplaylist2")
                || all.All(x => x.Name != "testplaylist3"))
            {
                Console.WriteLine("PlaylistService - Create + GetAll - NOK");
            }
            else
            {
                var edit = all.First(x => x.Name == "testplaylist1");
                edit.Name = "testeditedplaylist";
                _playlistService.EditPlaylist(edit);

                if (_playlistService.GetAllPlaylists().Count(x => x.Name.StartsWith("test")) != 3)
                {
                    Console.WriteLine("PlaylistService - Edit + GetAll - NOK");
                }
                else
                {
                    var filter = new PlaylistFilter
                    {
                        Name = "testeditedplaylist"
                    };

                    if (_playlistService.GetPlaylistsByFilter(filter).Count(x => x.Name.StartsWith("test")) != 1)
                    {
                        Console.WriteLine("PlaylistService - ListByFilter - NOK");
                    }
                    else
                    {
                        foreach (PlaylistDTO dto in all)
                        {
                            if (_playlistService.GetPlaylist(dto.ID).Name != dto.Name)
                            {
                                Console.WriteLine("PlaylistService - Delete + GetArtist - NOK");
                            }
                        }

                        Console.WriteLine(
                            _playlistService.GetPlaylistsByFilter(new PlaylistFilter { Name = "áýřžšáířž420šžřáíšě" }).Any()
                                ? "PlaylistService - EmptyFilter - NOK"
                                : "PlaylistService - OK");
                    }
                }
            }
        }

        private static void TestAlbumService()
        {

            var album1 = new AlbumDTO
            {
                Name = "testalbum1",
                PublishDate = DateTime.Now
            };

            var album2 = new AlbumDTO
            {
                Name = "testalbum2",
                PublishDate = DateTime.Now
            };

            var album3 = new AlbumDTO
            {
                Name = "testalbum3",
                PublishDate = DateTime.Now
            };

            _albumService.CreateAlbum(album1);
            _albumService.CreateAlbum(album2);
            _albumService.CreateAlbum(album3);

            var all = _albumService.GetAllAlbums().ToList();

            if (all.All(x => x.Name != "testalbum1")
                || all.All(x => x.Name != "testalbum2")
                || all.All(x => x.Name != "testalbum3"))
            {
                Console.WriteLine("AlbumService - Create + GetAll - NOK");
            }
            else
            {
                var edit = all.First(x => x.Name == "testalbum1");
                edit.Name = "testeditedalbum";
                _albumService.EditAlbum(edit);

                if (_albumService.GetAllAlbums().Count(x => x.Name.StartsWith("test")) != 3)
                {
                    Console.WriteLine("AlbumService - Edit + GetAll - NOK");
                }
                else
                {

                    var filter = new AlbumFilter
                    {
                        Name = "testeditedalbum"
                    };

                    if (_albumService.GetAlbumsByFilter(filter).Count(x => x.Name.StartsWith("test")) != 1)
                    {
                        Console.WriteLine("AlbumService - ListByFilter - NOK");
                    }
                    else
                    {
                        foreach (AlbumDTO dto in all)
                        {
                            if (_albumService.GetAlbum(dto.ID).Name != dto.Name)
                            {
                                Console.WriteLine("AlbumService - Delete + GetArtist - NOK");
                            }
                        }

                        Console.WriteLine(
                            _albumService.GetAlbumsByFilter(new AlbumFilter { Name = "áýřžšáířž420šžřáíšě" }).Any()
                                ? "AlbumService - EmptyFilter - NOK"
                                : "AlbumService - OK");
                    }
                }
            }
        }

        private static void TestEventService()
        {

            var event1 = new EventDTO
            {
                Name = "testevent1",
                Place = "aa",
                Start = DateTime.Now,
                End = DateTime.Now
            };

            var event2 = new EventDTO
            {
                Name = "testevent2",
                Place = "a",
                Start = DateTime.Now,
                End = DateTime.Now
            };

            var event3 = new EventDTO
            {
                Name = "testevent3",
                Place = "asfas",
                Start = DateTime.Now,
                End = DateTime.Now,
            };

            _eventService.CreateEvent(event1);
            _eventService.CreateEvent(event2);
            _eventService.CreateEvent(event3);

            var all = _eventService.GetAllEvents().ToList();

            if (all.All(x => x.Name != "testevent1")
                || all.All(x => x.Name != "testevent2")
                || all.All(x => x.Name != "testevent3"))
            {
                Console.WriteLine("EventService - Create + GetAll - NOK");
            }
            else
            {
                var edit = all.First(x => x.Name == "testevent1");
                edit.Name = "testeditedevent";
                _eventService.EditEvent(edit);

                if (_eventService.GetAllEvents().Count(x => x.Name.StartsWith("test")) != 3)
                {
                    Console.WriteLine("EventService - Edit + GetAll - NOK");
                }
                else
                {

                    var filter = new EventFilter
                    {
                        Name = "testeditedevent"
                    };

                    if (_eventService.GetEventsByFilter(filter).Count(x => x.Name.StartsWith("test")) != 1)
                    {
                        Console.WriteLine("EventService - ListByFilter - NOK");
                    }
                    else
                    {
                        foreach (EventDTO dto in all)
                        {
                            if (_eventService.GetEvent(dto.ID).Name != dto.Name)
                            {
                                Console.WriteLine("EventService - Delete + GetArtist - NOK");
                            }
                        }

                        Console.WriteLine(
                            _eventService.GetEventsByFilter(new EventFilter { Name = "áýřžšáířž420šžřáíšě" }).Any()
                                ? "EventService - EmptyFilter - NOK"
                                : "EventService - OK");
                    }
                }
            }
        }

        private static void ResetTheMess()
        {
            foreach (ArtistDTO dto in _artistService.GetAllArtists().Where(x => x.Name.StartsWith("test", StringComparison.InvariantCultureIgnoreCase)))
            {
                _artistService.DeleteArtist(dto.ID);
            }

            foreach (GenreDTO dto in _genreService.GetAllGenres().Where(x => x.Name.StartsWith("test", StringComparison.InvariantCultureIgnoreCase)))
            {
                _genreService.DeleteGenre(dto.ID);
            }

            foreach (AlbumDTO dto in _albumService.GetAllAlbums().Where(x => x.Name.StartsWith("test", StringComparison.InvariantCultureIgnoreCase)))
            {
                _albumService.DeleteAlbum(dto.ID);
            }

            foreach (EventDTO dto in _eventService.GetAllEvents().Where(x => x.Name.StartsWith("test", StringComparison.InvariantCultureIgnoreCase)))
            {
                _eventService.DeleteEvent(dto.ID);
            }

            foreach (SongDTO dto in _songService.GetAllSongs().Where(x => x.Name.StartsWith("test", StringComparison.InvariantCultureIgnoreCase)))
            {
                _songService.DeleteSong(dto.ID);
            }

            foreach (PlaylistDTO dto in _playlistService.GetAllPlaylists().Where(x => x.Name.StartsWith("test", StringComparison.InvariantCultureIgnoreCase)))
            {
                _playlistService.DeletePlaylist(dto.ID);
            }

            Console.WriteLine("Uklizeno.");
        }

        private static void Initialize()
        {
            Container.Install(new BussinessLayerInstaller());

            //tato sracka zatim nefunguje
            //_userAccountService = Container.Resolve<UserAccountService<Riganti.Utils.Infrastructure.EntityFramework.UserAccount>>();
            _genreService = Container.Resolve<IGenreService>();
            _artistService = Container.Resolve<IArtistService>();
            _albumService = Container.Resolve<IAlbumService>();
            _eventService = Container.Resolve<IEventService>();
            _playlistService = Container.Resolve<IPlaylistService>();
            _songService = Container.Resolve<ISongService>();

            MappingInit.ConfigureMapping();
        }
    }
}
