using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.GeoJsonObjectModel;

namespace WebScraping.Controllers
{
    public class arama_ekran : Controller
    {
        public IActionResult Index()
        {
            LinkHelper.Links.Clear();


            return View();
        }
    }
}
