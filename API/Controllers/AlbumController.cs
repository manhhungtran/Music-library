using System;
using System.Diagnostics;
using System.Net;
using System.Web.Http;
using BL.DTO;
using BL.Facades;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class AlbumController : ApiController
    {
        public LibraryFacade LibraryFacade { get; set; }


        [Route("~/api/Album")]
        public IHttpActionResult Get()
        {
            return Content(HttpStatusCode.OK, LibraryFacade.GetAlbums());
        }

        [Route("~/api/Album/{id}")]
        public IHttpActionResult Get(int id)
        {
            var result = id <= 0 ? null : LibraryFacade.GetAlbum(id);
            return result == null
                ? (IHttpActionResult)NotFound()
                : Content(HttpStatusCode.OK, result);
        }

        // POST: api/Album
        public IHttpActionResult Post([FromBody]string value)
        {
            try
            {
                var album = JsonConvert.DeserializeObject<AlbumDTO>(value);
                LibraryFacade.CreateAlbum(album);
                return Content(HttpStatusCode.Created, album);
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

        // PUT: api/Album/5
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            try
            {
                var album = JsonConvert.DeserializeObject<AlbumDTO>(value);
                if (id <= 0)
                {
                    return Content(HttpStatusCode.PreconditionFailed, "Product review ID must be greater than zero.");
                }
                album.ID = id;
                LibraryFacade.EditAlbum(album);
                return Content(HttpStatusCode.OK, album);
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


        // DELETE: api/Album/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                LibraryFacade.DeleteAlbum(id);
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