using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BL.DTO;
using BL.DTO.Event;
using BL.Filters;
using BL.Services;

namespace BL.Facades
{
    public class PremiumFacade
    {
        public int PageSize = 20;

        private readonly IEventService _eventService;
        private readonly IPlaylistService _playlistService;

        public PremiumFacade(IEventService eventService, IPlaylistService playlistService)
        {
            _eventService = eventService;
            _playlistService = playlistService;
        }

        #region CRUD Operations

        public EventDTO GetEvent(int id)
        {
            return _eventService.GetEvent(id);
        }

        public EventListQueryResultDTO GetEvents(EventFilter filter = null, int requiredPage = 1)
        {
            return _eventService.ListEvents(requiredPage, filter);
        }

        public void EditEvent(EventDTO @event)
        {
            _eventService.EditEvent(@event);
        }

        public void CreateEvent(EventDTO @event)
        {
            _eventService.CreateEvent(@event);
        }

        public void DeleteEvent(int eventId)
        {
            _eventService.DeleteEvent(eventId);
        }

        #endregion

        public IEnumerable<PlaylistDTO> GetRecommendedPlaylists(PlaylistDTO playlist)
        {
            return _playlistService.GetRecommendedPlaylists(playlist);
        }

        public void AddSongToPlaylist(int playlist, int song)
        {
            _playlistService.AddSongToPlaylist(playlist, song);
        }

        public void RemoveSongFromPlaylist(int playlist, int song)
        {
            _playlistService.RemoveSongFromPlaylist(playlist, song);
        }

        public IEnumerable<SelectListItem> GetSelectedItems(Guid getGuidByEmail)
        {
            return _playlistService.GetSelectedItems(getGuidByEmail);
        }
    }
}
