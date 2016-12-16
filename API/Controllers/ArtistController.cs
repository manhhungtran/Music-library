using System;
using System.Diagnostics;
using System.Net;
using System.Web.Http;
using BL.DTO;
using BL.Facades;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class ArtistController : ApiController
    {
        public LibraryFacade LibraryFacade { get; set; }

        [Route("~/api/Artist")]
        public IHttpActionResult Get()
        {
            return Content(HttpStatusCode.OK, LibraryFacade.GetArtists());
        }

        [Route("~/api/Artist/{id}")]
        public IHttpActionResult Get(int id)
        {
            var result = id <= 0 ? null : LibraryFacade.GetArtist(id);
            return result == null
                ? (IHttpActionResult) NotFound()
                : Content(HttpStatusCode.OK, result);
        }

        // POST: api/Artist
        public IHttpActionResult Post([FromBody]string value)
        {
            try
            {
                var artist = JsonConvert.DeserializeObject<ArtistDTO>(value);
                LibraryFacade.CreateArtist(artist);
                return Content(HttpStatusCode.Created, artist);
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

        // PUT: api/Artist/5
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            try
            {
                var artist = JsonConvert.DeserializeObject<ArtistDTO>(value);
                if (id <= 0)
                {
                    return Content(HttpStatusCode.PreconditionFailed, "Product review ID must be greater than zero.");
                }
                artist.ID = id;
                LibraryFacade.EditArtist(artist);
                return Content(HttpStatusCode.OK, artist);
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


        // DELETE: api/Artist/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                LibraryFacade.DeleteArtist(id);
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
