using InsightAcademy.ApplicationLayer.Models;
using InsightAcademy.ApplicationLayer.Repository;
using InsightAcademy.ApplicationLayer.Services;
using InsightAcademy.DomainLayer.Entities;
using InsightAcademy.UI.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InsightAcademy.UI.Controllers
{
    [Authorize(Roles = "Teacher,SuperAdmin")]
    public class TutorController : Controller
    {
        private readonly ITutorService _tutorService;
        private readonly IWishList _wishListService;
        private readonly IUser _user;
        private readonly FileUploader _fileUploader;
        //public Pager<TutorSubject> Pager;
        public Pager<Tutor> Pager;
        public TutorController(ITutorService tutorService, IUser user, FileUploader fileUploader, IWishList wishListService)
        {
            _tutorService = tutorService;
            _user = user;
            _fileUploader = fileUploader;
            _wishListService = wishListService;
        }
        public async Task<IActionResult> PersonalDetails()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return View(new Tutor());
            }

            var tutor = await _tutorService.GetPersonalDetail(userId);
            if (tutor == null)
            {
                tutor = new Tutor();
            }

            return View(tutor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PersonalDetails(Tutor profile)
        {
            if (ModelState.IsValid)
            {
                if (profile.Id == Guid.Empty)
                {
                    profile.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    await _tutorService.AddPersonalDetialAsync(profile);
                }
                else
                {

                    await _tutorService.EditPersonalDetails(profile);


                    // Update existing tutor profile


                }

                return RedirectToAction("ContactDetail", "Tutor");
            }

            // If ModelState is not valid, return to the view with validation errors
            return View(profile);
        }

        public async Task<IActionResult> ContactDetail()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tutorId = await _user.GetTutorId(userId);

            if (tutorId == Guid.Empty)
            {
                return View(new Contact());
            }

            var contact = await _tutorService.GetContactDetails(tutorId);

            if (contact == null)
            {
                return View(new Contact());
            }

            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactDetail(Contact contact)
        {
            if (ModelState.IsValid)
            {
                if (contact.Id == Guid.Empty)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var tutorId = await _user.GetTutorId(userId);
                    contact.TutorId = tutorId;
                    await _tutorService.AddContactDetialAsync(contact);
                }
                else
                {
                    await _tutorService.EditContactDetails(contact);
                }

                return RedirectToAction("Education", "Tutor");
            }

            // If ModelState is not valid, return to the view with validation errors
            return View(contact);
        }
        public async Task<IActionResult> EducationById(Guid id)
        {
            var education = await _tutorService.GetEducationById(id);
            return Json(education);
        }
        public async Task<IActionResult> Education()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tutorId = await _user.GetTutorId(userId);

            if (tutorId == Guid.Empty)
            {
                return View(new Education());
            }

            var education = await _tutorService.GetEducationDetails(tutorId);
            ViewBag.Education = education;
            //if (education == null)
            //{
            //    return View(new Education());
            //}

            return View(new Education());
        }
        [HttpPost]
        public async Task<IActionResult> Education(Education education)
        {
            if (ModelState.IsValid)
            {
                if (education.Id == Guid.Empty)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var tutorId = await _user.GetTutorId(userId);
                    education.TutorId = tutorId;
                    await _tutorService.AddEducationDetialAsync(education);
                }
                else
                {
                    await _tutorService.EditEducationDetails(education);
                }

                return RedirectToAction("Education", "Tutor");
            }

            return View(education);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteEducation(Guid id)
        {
            await _tutorService.DeleteEducation(id);
            return Json(true);
        }

        public IActionResult Media()
        {
            return View();
        }
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tutorId = await _user.GetTutorId(userId);
            var tutor = await _tutorService.tutorProfile(tutorId);
            return View(tutor);
        }
        [AllowAnonymous]
        [Route("[Controller]/Detail")]
        public async Task<IActionResult> ProfileView(Guid tutorId)
        {
            var tutor = await _tutorService.tutorProfile(tutorId);
            return View("Profile", tutor);
        }

        public IActionResult Subjects()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Subjects(Subject subject)
        {
            if (ModelState.IsValid)
            {
                if (subject.Id == Guid.Empty)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var tutorId = await _user.GetTutorId(userId);
                    await _tutorService.AddSubjectAsync(subject);
                }
                else
                {
                    //Edit subject
                }

                return RedirectToAction("Media", "Tutor");
            }
            return View(subject);
        }


        [HttpPost]
        public async Task<IActionResult> UploadProfilePic(IFormFile profilePic)
        {
            if (profilePic != null)
            {
                var result = await _fileUploader.UploadFile(profilePic);
                if (result.status)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var status = await _user.UploadProfileImage(userId, result.url);
                    if (status)
                    {
                        return Json(new { status = status, Url = result.url });
                    }
                }
                else
                {
                    return Json(new { status = false, Url = "" });
                }
            }
            return Json(new { status = false, Url = "" });
        }
        //public async Task<IActionResult> SearchTutor(Guid subject, int pageNumber = 1)
        //{
        //    if (subject != Guid.Empty)
        //    {

        //        var tutorSubject = await _tutorService.GetTutorsBySubject(subject);
        //        ViewBag.Degree = new SelectList(await _tutorService.GetAllDegree(), "Degree", "Degree");
        //        ViewBag.Subject = new SelectList(await _tutorService.GetAllSubject(), "Id", "Name");

        //        Pager = new Pager<TutorSubject>(tutorSubject, pageNumber, 10, subject.ToString());
        //        return View(Pager);
        //    }

        //    return View();
        //}

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddWishList(Guid tutorId)
        {
            if (tutorId != Guid.Empty)
            {
                var status = await _wishListService.AddWishList(tutorId);
                if (status)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }


            }
            return Json(false);
        }

        [HttpPost]
        public async Task<IActionResult> Filter(TutorFilter filter)
        {
            var tutors = await _tutorService.Filter(filter);
            return PartialView("_FilterTutor", tutors);
        }
        [ActionName("list")]
        public async Task<IActionResult> AllTutor(TutorFilter filter, int pageNumber = 1)
        {

            ViewBag.Degree = new SelectList(await _tutorService.GetAllDegree(), "Degree", "Degree");
            ViewBag.Subject = new SelectList(await _tutorService.GetAllSubject(), "Id", "Name");
            var TutorList = await _tutorService.Filter(filter);
            Pager = new Pager<Tutor>(TutorList, pageNumber, 10);
            return View("AllTutor", Pager);
        }

        public async Task<IActionResult> TutorGridView()
        {
            var tutorList = await _tutorService.TutorList();
            return PartialView("_TutorList2", tutorList);
        }

    }
}
