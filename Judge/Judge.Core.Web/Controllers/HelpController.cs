using Microsoft.AspNetCore.Mvc;

namespace Judge.Web.Controllers
{
    public class HelpController : Controller
    {
        public ActionResult Index()
        {
            var text = System.IO.File.ReadAllText("wwwroot/content/help/ru.md");
            return View((object)text);
        }

        public ActionResult About()
        {
            var text = System.IO.File.ReadAllText("wwwroot/content/about/ru.md");
            return View((object)text);
        }
    }
}