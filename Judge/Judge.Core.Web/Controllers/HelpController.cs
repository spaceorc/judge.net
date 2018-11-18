using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Judge.Web.Controllers
{
    public class HelpController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public HelpController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult Index()
        {
            return Content(_hostingEnvironment.ContentRootPath + "/Content/Help/ru.md");
        }

        public ActionResult About()
        {
            return Content(_hostingEnvironment.ContentRootPath + "/Content/About/ru.md");
        }
    }
}