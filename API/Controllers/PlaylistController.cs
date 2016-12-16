using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using BL.DTO;
using BL.Facades;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class PlaylistController : ApiController
    {
        public LibraryFacade LibraryFacade { get; set; }


        [Route("~/api/Playlist")]
        public IHttpActionResult Get()
        {
            return Content(HttpStatusCode.OK, LibraryFacade.GetPlaylists());
        }

        [Route("~/api/Playlist/{id}")]
        public IHttpActionResult Get(int id)
        {
            var result = id <= 0 ? null : LibraryFacade.GetPlaylist(id);
            return result == null
                ? (IHttpActionResult)NotFound()
                : Content(HttpStatusCode.OK, result);
        }

        // POST: api/Playlist
        public IHttpActionResult Post([FromBody]string value)
        {
            try
            {
                var playlist = JsonConvert.DeserializeObject<PlaylistDTO>(value);
                LibraryFacade.CreatePlaylist(playlist);
                return Content(HttpStatusCode.Created, playlist);
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

        // PUT: api/Playlist/5
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            try
            {
                var playlist = JsonConvert.DeserializeObject<PlaylistDTO>(value);
                if (id <= 0)
                {
                    return Content(HttpStatusCode.PreconditionFailed, "Product review ID must be greater than zero.");
                }
                playlist.ID = id;
                LibraryFacade.EditPlaylist(playlist);
                return Content(HttpStatusCode.OK, playlist);
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


        // DELETE: api/Playlist/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                LibraryFacade.DeletePlaylist(id);
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