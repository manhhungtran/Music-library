using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BL.DTO;
using BL.DTO.Artist;
using BL.Filters;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Services
{
    public class ArtistService : MusicLibraryService, IArtistService
    {

        private readonly ArtistRepository artistRepository;
        private readonly ArtistListQuery artistListQuery;
        public int ArtistsPageSize = 20;

        public ArtistService(ArtistRepository artistRepository, ArtistListQuery artistListQuery)
        {
            this.artistRepository = artistRepository;
            this.artistListQuery = artistListQuery;
        }

        public void CreateArtist(ArtistDTO artistDto)
        {
            if (GetAllArtists().Any(x => x.Name == artistDto.Name))
            {
                return;
            }

            using (var uow = UnitOfWorkProvider.Create())
            {
                var artist = Mapper.Map<Artist>(artistDto);
                artistRepository.Insert(artist);
                uow.Commit();
            }
        }

        public void EditArtist(ArtistDTO artistDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                Artist artist = artistRepository.GetById(artistDto.ID);
                Mapper.Map(artistDto, artist);
                artistRepository.Update(artist);
                uow.Commit();
            }
        }

        public void DeleteArtist(int artistId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                artistRepository.Delete(artistId);
                uow.Commit();
            }
        }

        public ArtistDTO GetArtist(int artistId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var artist = artistRepository.GetById(artistId);
                return artist == null ? null : Mapper.Map<ArtistDTO>(artist);
            }
        }

        public IEnumerable<ArtistDTO> GetAllArtists()
        {
            using (UnitOfWorkProvider.Create())
            {
                artistListQuery.Filter = null;
                return artistListQuery.Execute() ?? new List<ArtistDTO>();
            }
        }

        public ArtistListQueryResultDTO ListArtists(int requestedPage, ArtistFilter filter = null)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = artistListQuery;
                query.Filter = filter;
                query.Skip = Math.Max(0, requestedPage - 1) * ArtistsPageSize;
                query.Take = ArtistsPageSize;
                query.AddSortCriteria(x => x.Name, SortDirection.Ascending);

                return new ArtistListQueryResultDTO
                {
                    RequestedPage = requestedPage,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultsPage = query.Execute(),
                };
            }
        }

        public IEnumerable<ArtistDTO> GetArtistsByFilter(ArtistFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                artistListQuery.Filter = filter;
                return artistListQuery.Execute() ?? new List<ArtistDTO>();
            }
        }

        public IEnumerable<SelectListItem> GetArtistBasicsByFilter(ArtistFilter filter)
        {
            var result = GetArtistsByFilter(filter);
            return result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.ID.ToString()
            });
        }
    }
}
