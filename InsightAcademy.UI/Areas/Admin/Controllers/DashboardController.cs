using InsightAcademy.ApplicationLayer.Services;
using InsightAcademy.DomainLayer.Entities;
using InsightAcademy.InfrastructureLayer.Data;
using Microsoft.AspNetCore.Mvc;

namespace InsightAcademy.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ITutorService _tutorService;
        private readonly AppDbContext _context;
        public DashboardController(ITutorService tutorService,AppDbContext context)
        {
            _tutorService = tutorService;
            _context = context;
            
        }
        public async Task<IActionResult> Index()
        {
            var tutor =  await _tutorService.TutorList();
            return View(tutor);
        }
        [HttpPost]
        public async Task< IActionResult> Approve(Guid tutorId)
        {
           
            var result =  await _tutorService.ApproveTutor(tutorId);
            if (result == true)
            {
                return Json(new { success = true });
            }
            
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task <IActionResult> Unapprove(Guid tutorId)
        {
          
            var result = await _tutorService.UnapproveTutor(tutorId);
            if (result == true)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
    }
}
