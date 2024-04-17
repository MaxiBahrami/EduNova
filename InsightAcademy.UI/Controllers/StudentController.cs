using InsightAcademy.ApplicationLayer.Repository;
using InsightAcademy.ApplicationLayer.Services;
using InsightAcademy.UI.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InsightAcademy.UI.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {

        private readonly IWishList _wishListService;
        private readonly ITutorService _tutorService;

        public StudentController(IWishList wishListService, ITutorService tutorService)
        {

            _wishListService = wishListService;
            _tutorService = tutorService;
        }
        [ActionName("MyWishList")]
        public async Task<IActionResult> GetAllWishList()
        {
            var wishList = await _wishListService.GetAllWishList();
            return View("GetAllWishList", wishList);

        }
      
       
    }
}
