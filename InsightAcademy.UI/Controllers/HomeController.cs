using InsightAcademy.ApplicationLayer.Services;
using InsightAcademy.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace InsightAcademy.UI.Controllers
{
    public class HomeController : Controller
    {
		private readonly ITutorService _tutorService;
		private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger, ITutorService tutorService)
        {
            _tutorService = tutorService;
            _logger = logger;
        }

        public async Task <IActionResult> Index()
        {

			ViewBag.Subject = new SelectList(await _tutorService.GetAllSubject(), "Id", "Name");
			return View();
        }

        public IActionResult SearchListing()
        {
            return View();

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
