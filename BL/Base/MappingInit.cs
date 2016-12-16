using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BL.DTO;
using BL.DTO.UserAccount;
using DAL.Entities;

namespace BL.Base
{
    public static class MappingInit
    {
        public static void ConfigureMapping()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Genre, GenreDTO>().ReverseMap();
                config.CreateMap<Artist, ArtistDTO>().ReverseMap();
                config.CreateMap<Album, AlbumDTO>().ReverseMap();
                config.CreateMap<Song, SongDTO>()
                    .ConstructUsing(ConstructDestination)
                    .ForMember(x => x.Genres, option => option.Ignore());
                config.CreateMap<SongDTO, Song>()
                    .ConstructUsing(ConstructDestination)
                    .ForMember(x => x.Genres, option => option.Ignore());
                config.CreateMap<Playlist, PlaylistDTO>()
                    .ConstructUsing(ConstructDestination)
                    .ForMember(x => x.Songs, option => option.Ignore());
                config.CreateMap<PlaylistDTO, Playlist>()
                    .ConstructUsing(ConstructDestination)
                    .ForMember(x => x.Songs, option => option.Ignore())
                    .ForMember(x => x.Genres, option => option.Ignore());
                config.CreateMap<Event, EventDTO>().ReverseMap();
                config.CreateMap<UserAccountDTO, UserAccount>().ReverseMap();

                config.CreateMap<UserRegistrationDTO, UserAccount>();
                config.CreateMap<VipCodes, VipCodesDTO>().ReverseMap();
            });
        }

        #region ConstructDestination

        private static Playlist ConstructDestination(PlaylistDTO src)
        {
            return new Playlist
            {
                Songs = String.Join(";", src.Songs ?? new List<string>())
            };
        }

        private static PlaylistDTO ConstructDestination(Playlist src)
        {
            return new PlaylistDTO
            {
                Songs = src.Songs?.Split(';').ToList()
            };
        }

        private static Song ConstructDestination(SongDTO src)
        {
            return new Song
            {
                Genres = String.Join(";", src.Genres ?? new List<string>())
            };
        }

        private static SongDTO ConstructDestination(Song src)
        {
            return new SongDTO
            {
                Genres = src.Genres?.Split(';').ToList()
            };
        }

        #endregion
    }
}
