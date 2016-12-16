using System.Web.Mvc;
using BL.Facades;

namespace PL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}