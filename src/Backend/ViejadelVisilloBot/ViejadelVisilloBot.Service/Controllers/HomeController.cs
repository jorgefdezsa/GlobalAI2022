namespace ViejadelVisillo.Services.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ViejadelVisilloBot.Model.Constants;

    public class HomeController : Controller
    {

        [HttpGet]
        [Route(HttpRouteConstants.Home)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
