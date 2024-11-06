using Microsoft.AspNetCore.Mvc;

namespace GatePassCancellation.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
