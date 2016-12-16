using System;
using System.Data.Entity;
using System.Linq;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTO;
using BL.Filters;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class EventListQuery : AppQuery<EventDTO>
    {
        public EventFilter Filter { get; set; }

        public EventListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<EventDTO> GetQueryable()
        {
            IQueryable<Event> query = Context.Event;

            if (Filter == null)
            {
                return query
                    .ProjectTo<EventDTO>();
            }

            if (!String.IsNullOrEmpty(Filter.Name))
            {
                query = query.Where(
                    e => (e.Name.Contains(Filter.Name)));
            }

            if (!String.IsNullOrEmpty(Filter.Place))
            {
                query = query.Where(
                    e => (e.Place.Contains(Filter.Place)));
            }

            if (!String.IsNullOrEmpty(Filter.Artist))
            {
                query = query.Where(
                    e => (e.Artist.Name.Contains(Filter.Artist)));
            }

            if (Filter.ArtistId != null && Filter.ArtistId.Value > 0)
            {
                query = query.Where(e => e.Artist.ID == Filter.ArtistId.Value);
            }

            return query
                .ProjectTo<EventDTO>();
        }
    }
}
