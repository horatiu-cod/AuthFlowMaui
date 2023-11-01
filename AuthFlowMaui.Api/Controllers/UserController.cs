using Microsoft.AspNetCore.Mvc;

namespace AuthFlowMaui.Api.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
