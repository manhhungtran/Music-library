using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BL.DTO;
using BL.DTO.Genre;
using BL.Filters;
using BL.Queries;
using BL.Repositories;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Services
{
    public class GenreService : MusicLibraryService, IGenreService
    {

        private readonly GenreRepository genreRepository;
        private readonly GenreListQuery genreListQuery;
        public int GenresPageSize = 20;

        public GenreService(GenreRepository genreRepository, GenreListQuery genreListQuery)
        {
            this.genreRepository = genreRepository;
            this.genreListQuery = genreListQuery;
        }

        public void CreateGenre(GenreDTO genreDto)
        {
            // if there is already genre with same name do nothing
            if (GetAllGenres().Any(genre => genre.Name == genreDto.Name))
            {
                return;
            }

            using (var uow = UnitOfWorkProvider.Create())
            {
                var genre = Mapper.Map<DAL.Entities.Genre>(genreDto);
                genreRepository.Insert(genre);
                uow.Commit();
            }
        }

        public void EditGenre(GenreDTO genreDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var genre = genreRepository.GetById(genreDto.ID);
                Mapper.Map(genreDto, genre);
                genreRepository.Update(genre);
                uow.Commit();
            }
        }

        public void DeleteGenre(int genreId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                genreRepository.Delete(genreId);
                uow.Commit();
            }
        }

        public GenreDTO GetGenre(int genreId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var genre = genreRepository.GetById(genreId);
                return genre == null ? null : Mapper.Map<GenreDTO>(genre);
            }
        }

        public IEnumerable<GenreDTO> GetAllGenres()
        {
            using (UnitOfWorkProvider.Create())
            {
                genreListQuery.Filter = null;
                return genreListQuery.Execute();
            }
        }

        public GenreListQueryResultDTO ListGenres(int requestedPage, GenreFilter filter = null)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = genreListQuery;
                query.Filter = filter;
                query.Skip = Math.Max(0, requestedPage - 1)*GenresPageSize;
                query.Take = GenresPageSize;
                query.AddSortCriteria(x => x.Name, SortDirection.Ascending);

                return new GenreListQueryResultDTO
                {
                    RequestedPage = requestedPage,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultsPage = query.Execute()
                };
            }
        }

        public IEnumerable<GenreDTO> GetGenresByFilter(GenreFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                genreListQuery.Filter = filter;
                return genreListQuery.Execute() ?? new List<GenreDTO>();
            }
        }

        public List<string> FormGenres(IEnumerable<string> genres)
        {
            List<string> result = new List<string>();
            foreach (string genre in genres)
            {
                if (!String.IsNullOrWhiteSpace(genre))
                {
                    var selectedGenres = FindMeAnyGenre(genre);
                    while (!selectedGenres.Any())
                    {
                        var newGenre = new GenreDTO {Name = genre};
                        CreateGenre(newGenre);
                        selectedGenres = FindMeAnyGenre(genre);
                    }
                    result.Add(selectedGenres.First().ID.ToString());
                }
            }

            return result;
        }

        public List<string> GetGenreNames(List<string> modelGenres)
        {
            List<string> result = new List<string>();
            foreach (string genre in modelGenres)
            {
                if (!String.IsNullOrWhiteSpace(genre))
                {
                    result.Add(GetGenre(Convert.ToInt32(genre)).Name);
                }
            }
            return result;
        }

        private List<GenreDTO> FindMeAnyGenre(string genre)
        {
            var filter = new GenreFilter()
            {
                Name = genre
            };
            return GetGenresByFilter(filter).ToList();
        }
    }
}
