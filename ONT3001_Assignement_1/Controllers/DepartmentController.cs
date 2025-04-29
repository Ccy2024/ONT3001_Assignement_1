using Microsoft.AspNetCore.Mvc;

namespace ONT3001_Assignement_1.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
