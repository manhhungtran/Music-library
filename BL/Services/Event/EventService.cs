using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BL.DTO;
using BL.DTO.Event;
using BL.Filters;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Services
{
    public class EventService : MusicLibraryService, IEventService
    {

        private readonly EventRepository _eventRepository;
        private readonly EventListQuery _eventListQuery;
        private readonly ArtistRepository artistRepository;
        public int EventsPageSize = 20;

        public EventService(EventRepository eventRepository, EventListQuery eventListQuery, ArtistRepository artistRepository)
        {
            _eventRepository = eventRepository;
            _eventListQuery = eventListQuery;
            this.artistRepository = artistRepository;
        }

        public void CreateEvent(EventDTO eventDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var event_ = Mapper.Map<Event>(eventDto);
                if (eventDto.ArtistPId != null)
                {
                    event_.Artist = GetArtist(eventDto.ArtistPId);
                }
                _eventRepository.Insert(event_);
                uow.Commit();
            }
        }

        private Artist GetArtist(int? artistPId)
        {
            var artist = artistRepository.GetById(artistPId.Value);
            if (artist == null)
            {
                throw new NullReferenceException("Artist does not exist.");
            }
            return artist;
        }

        public void EditEvent(EventDTO eventDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var event_ = _eventRepository.GetById(eventDto.ID);
                Mapper.Map(eventDto, event_);
                _eventRepository.Update(event_);
                uow.Commit();
            }
        }

        public void DeleteEvent(int eventId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _eventRepository.Delete(eventId);
                uow.Commit();
            }
        }

        public EventDTO GetEvent(int eventId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var event_ = _eventRepository.GetById(eventId);
                return event_ == null ? null : Mapper.Map<EventDTO>(event_);
            }
        }

        public IEnumerable<EventDTO> GetAllEvents()
        {
            using (UnitOfWorkProvider.Create())
            {
                _eventListQuery.Filter = null;
                return _eventListQuery.Execute();
            }
        }

        public EventListQueryResultDTO ListEvents(int requestedPage, EventFilter filter = null)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = _eventListQuery;
                query.Filter = filter;
                query.Skip = Math.Max(0, requestedPage - 1) * EventsPageSize;
                query.Take = EventsPageSize;
                query.AddSortCriteria(x => x.Name, SortDirection.Ascending);

                return new EventListQueryResultDTO
                {
                    RequestedPage = requestedPage,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultsPage = query.Execute()
                };
            }
        }

        public IEnumerable<EventDTO> GetEventsByFilter(EventFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                _eventListQuery.Filter = filter;
                return _eventListQuery.Execute();
            }
        }

        public IEnumerable<EventDTO> GetEventsByArtist(int artistId)
        {
            using (UnitOfWorkProvider.Create())
            {
                _eventListQuery.Filter = new EventFilter { ArtistId = artistId };
                return _eventListQuery.Execute();
            }
        }
    }
}
