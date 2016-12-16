using System;
using System.Diagnostics;
using System.Net;
using System.Web.Http;
using BL.DTO;
using BL.Facades;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class SongController : ApiController
    {
        public LibraryFacade LibraryFacade { get; set; }


        [Route("~/api/Song")]
        public IHttpActionResult Get()
        {
            return Content(HttpStatusCode.OK, LibraryFacade.GetSongs());
        }

        [Route("~/api/Song/{id}")]
        public IHttpActionResult Get(int id)
        {
            var result = id <= 0 ? null : LibraryFacade.GetSong(id);
            return result == null
                ? (IHttpActionResult)NotFound()
                : Content(HttpStatusCode.OK, result);
        }

        // POST: api/Song
        public IHttpActionResult Post([FromBody]string value)
        {
            try
            {
                var song = JsonConvert.DeserializeObject<SongDTO>(value);
                LibraryFacade.CreateSong(song);
                return Content(HttpStatusCode.Created, song);
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

        // PUT: api/Song/5
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            try
            {
                var song = JsonConvert.DeserializeObject<SongDTO>(value);
                if (id <= 0)
                {
                    return Content(HttpStatusCode.PreconditionFailed, "Product review ID must be greater than zero.");
                }
                song.ID = id;
                LibraryFacade.EditSong(song);
                return Content(HttpStatusCode.OK, song);
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


        // DELETE: api/Song/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                LibraryFacade.DeleteSong(id);
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
