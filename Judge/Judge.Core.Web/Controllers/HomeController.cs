using Microsoft.AspNetCore.Mvc;

namespace Judge.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}