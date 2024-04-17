using InsightAcademy.ApplicationLayer.Models;
using InsightAcademy.ApplicationLayer.Repository;
using InsightAcademy.ApplicationLayer.Services;
using InsightAcademy.DomainLayer.Entities;
using InsightAcademy.DomainLayer.Entities.Identity;
using InsightAcademy.InfrastructureLayer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InsightAcademy.InfrastructureLayer.Implementation
{
    public class TutorRepository : ITutor
    {
        private readonly AppDbContext _contaxt;
        private readonly IApplicationEmailSender _emailSender;
        public TutorRepository(AppDbContext context, IApplicationEmailSender emailSender)
        {
            _contaxt = context;
            _emailSender = emailSender;


        }
        public async Task<bool> AddContactDetialAsync(Contact contact)
        {
            await _contaxt.Contact.AddAsync(contact);
            await _contaxt.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddEducationDetialAsync(Education education)
        {
            _contaxt.Education.Add(education);
            return await _contaxt.SaveChangesAsync() > 0;
        }

        public Task<bool> AddGalleryDetialAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> AddPersonalDetialAsync(Tutor profile)
        {
            await _contaxt.Tutor.AddAsync(profile);
            await _contaxt.SaveChangesAsync();
            return profile.Id;
        }

        public Task<bool> AddSubjectDetialAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Contact> GetContactDetails(Guid tutorId)
        {
            return await _contaxt.Contact.FirstOrDefaultAsync(z => z.TutorId == tutorId);
        }

        public async Task<Tutor> GetProfileDetail(string userId)
        {
            return await _contaxt.Tutor.FirstOrDefaultAsync(z => z.ApplicationUserId == userId);
        }
        public async Task<bool> EditPersonalDetails(Tutor tutorObj)
        {
            var tutor = await _contaxt.Tutor.FindAsync(tutorObj.Id);

            tutor.EmailAddress = tutorObj.EmailAddress;
            tutor.FirstName = tutorObj.FirstName;
            tutor.LastName = tutorObj.LastName;
            tutor.Introduction = tutorObj.Introduction;
            tutor.PhoneNumber = tutorObj.PhoneNumber;
            tutor.City = tutorObj.City;
            tutor.Country = tutorObj.Country;
            tutor.Website = tutorObj.Website;
            tutor.HourlyRate = tutorObj.HourlyRate;
            tutor.SkypeId = tutorObj.SkypeId;
            tutor.Services = tutorObj.Services;
            return await _contaxt.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditContactDetails(Contact contact)
        {


            var oldTuor = await _contaxt.Contact.FindAsync(contact.Id);


            oldTuor.PhoneNumber = contact.PhoneNumber;
            oldTuor.Email = contact.Email;
            oldTuor.SkypeId = contact.SkypeId;
            oldTuor.WhatsappNumber = contact.WhatsappNumber;
            oldTuor.Website = contact.Website;
            return await _contaxt.SaveChangesAsync() > 0;
        }
        public async Task<bool> EditEducationDetails(Education education)
        {


            var oldEducation = await _contaxt.Education.FindAsync(education.Id);


            oldEducation.Degree = education.Degree;
            oldEducation.University = education.University;
            oldEducation.Location = education.Location;
            oldEducation.StartDate = education.StartDate;
            oldEducation.EndDate = education.EndDate;
            oldEducation.Description = education.Description;
            return await _contaxt.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Education>> GetEducationDetails(Guid tutorId)
        {
            return await _contaxt.Education.Where(z => z.TutorId == tutorId).ToListAsync();
        }

        public async Task<Education> GetEducationById(Guid id)
        {
            return await _contaxt.Education.FindAsync(id);
        }

        public async Task<bool> DeleteEducation(Guid id)
        {
            var education = await _contaxt.Education.FindAsync(id);
            _contaxt.Education.Remove(education);
            return await _contaxt.SaveChangesAsync() > 0;
        }

        public async Task<Tutor> tutorProfile(Guid id)
        {
            var tutor = await _contaxt.Tutor.Include(x => x.WishLists).Include(z => z.Contact).Include(z => z.TutorSubject).ThenInclude(x => x.Subject).Include(z => z.ApplicationUser).Include(z => z.Education).FirstOrDefaultAsync(x => x.Id == id);
            return tutor;


        }



        public async Task<bool> AddSubjectAsync(Subject subject)
        {
            await _contaxt.Subject.AddAsync(subject);
            return await _contaxt.SaveChangesAsync() > 0;

        }
        //public async Task<IEnumerable<Subject>> SubjectsList(Guid subjectId)
        //      {
        //         return await _contaxt.Subject.Where(z => z.Id == subjectId).ToListAsync();

        //}
        public async Task<List<Subject>> GetAllSubject()
        {

            var result = await _contaxt.Subject.ToListAsync();
            return result;
        }
        public async Task<IEnumerable<TutorSubject>> GetTutorsBySubject(Guid subject)
        {
            var result = await _contaxt.TutorSubject.Include(x => x.Tutor).ThenInclude(x => x.WishLists).Include(z => z.Tutor).ThenInclude(x => x.ApplicationUser).Include(x => x.Subject).Where(x => x.SubjectId == subject).ToListAsync();

            return result;
        }
        public async Task<List<Education>> GetAllDegree()
        {
            var result = await _contaxt.Education.ToListAsync();
            return result;
        }
        public async Task<List<Tutor>> TutorList()
        {
            var tutorList = await _contaxt.Tutor
        .Include(tutor => tutor.Contact)
        .Include(tutor => tutor.ApplicationUser)
        .Include(tutor => tutor.WishLists)
        .Select(tutor => new Tutor
        {
            Id = tutor.Id,
            ApplicationUser = tutor.ApplicationUser,
            FirstName = tutor.FirstName,
            WishLists = tutor.WishLists,
            LastName = tutor.LastName,
            EmailAddress = tutor.EmailAddress,
            HourlyRate = tutor.HourlyRate,
            Website = tutor.Contact.Website,
            Country = tutor.Country,
            PhoneNumber = tutor.Contact.WhatsappNumber,
            SkypeId = tutor.Contact.SkypeId,
            IsVerified = tutor.IsVerified

        })
        .ToListAsync();

            return tutorList;
        }

        public async Task<bool> ApproveTutor(Guid tutorId)
        {
            var tutor = _contaxt.Tutor.FirstOrDefault(x => x.Id == tutorId);

            if (tutor != null)
            {
                tutor.IsVerified = true;
                await _contaxt.SaveChangesAsync();

                await _emailSender.SendTutorVarifyEmail(tutor.FullName, tutor.EmailAddress);

                return true;
            }
            return false;
        }
        public async Task<bool> UnapproveTutor(Guid tutorId)
        {
            var tutor = _contaxt.Tutor.FirstOrDefault(x => x.Id == tutorId);

            if (tutor != null)
            {
                tutor.IsVerified = false;
                await _contaxt.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Tutor>> Filter(TutorFilter filter)
        {
            IQueryable<Tutor> query = _contaxt.Tutor
                                            .Include(x => x.ApplicationUser)
                                            .Include(x => x.WishLists)
                                            .Include(x => x.Contact)
                                            .Include(z => z.TutorSubject);

            if (filter.SubjectId != Guid.Empty)
            {
                query = query.Where(t => t.TutorSubject.Any(ts => ts.SubjectId == filter.SubjectId));
            }
            if (filter.Order == "ASC")
            {
                query = query.OrderBy(z => z.HourlyRate);
            }
            else
            {
                query = query.OrderByDescending(z => z.HourlyRate);
            }
            if (!string.IsNullOrEmpty(filter.Location))
            {
                query = query.Where(z => z.City.Contains(filter.Location));
            }
            if (filter.MinPrice > 0 && filter.MaxPrice > 0)
            {
                query = query.Where(z => z.HourlyRate >= filter.MinPrice && z.HourlyRate <= filter.MaxPrice);
            }

            if (!string.IsNullOrEmpty(filter.service))
            {
                query = query.Where(z => z.Services.Contains(filter.service));
            }

            if (filter.SubjectIds.Count>0)
            {
                query = query.Where(t => t.TutorSubject.Any(ts =>filter.SubjectIds.Contains(filter.SubjectId)));
            }
            return await query.ToListAsync();
        }
    }
}
