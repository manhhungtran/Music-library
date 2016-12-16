using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BL.DTO;
using BL.Facades;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class EventController : ApiController
    {
        public PremiumFacade PremiumFacade { get; set; }

        [Route("~/api/Event")]
        public IHttpActionResult Get()
        {
            return Content(HttpStatusCode.OK, PremiumFacade.GetEvents());
        }

        [Route("~/api/Event/{id}")]
        public IHttpActionResult Get(int id)
        {
            var result = id <= 0 ? null : PremiumFacade.GetEvent(id);
            return result == null
                ? (IHttpActionResult) NotFound()
                : Content(HttpStatusCode.OK, result);
        }

        // POST: api/Event
        public IHttpActionResult Post([FromBody] string value)
        {
            try
            {
                var eventDto = JsonConvert.DeserializeObject<EventDTO>(value);
                PremiumFacade.CreateEvent(eventDto);
                return Content(HttpStatusCode.Created, eventDto);
            }
            catch (JsonException)
            {
                Debug.WriteLine($"MusicLib API - Post(...) - failed to deserialize value: {value}");
                return StatusCode(HttpStatusCode.BadRequest);
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(HttpStatusCode.NotFound);
            }
        }

        // PUT: api/Event/5
        public IHttpActionResult Put(int id, [FromBody] string value)
        {
            try
            {
                var eventDto = JsonConvert.DeserializeObject<EventDTO>(value);
                if (id <= 0)
                {
                    return Content(HttpStatusCode.PreconditionFailed, "Product review ID must be greater than zero.");
                }
                eventDto.ID = id;
                PremiumFacade.EditEvent(eventDto);
                return Content(HttpStatusCode.OK, eventDto);
            }
            catch (JsonException)
            {
                Debug.WriteLine($"MusicLib API - Put(...) - failed to deserialize value: {value}");
                return StatusCode(HttpStatusCode.BadRequest);
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(HttpStatusCode.NotFound);
            }
        }


        // DELETE: api/Event/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                PremiumFacade.DeleteEvent(id);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(HttpStatusCode.NotFound);
            }
        }
    }
}
    