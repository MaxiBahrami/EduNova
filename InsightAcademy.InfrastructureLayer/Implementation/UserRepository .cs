using InsightAcademy.ApplicationLayer.Repository;
using InsightAcademy.DomainLayer.Entities;
using InsightAcademy.InfrastructureLayer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InsightAcademy.InfrastructureLayer.Implementation
{
    public class UserRepository : IUser
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid> GetTutorId(string userId)
        {
            var tutor = await _context.Tutor.FirstOrDefaultAsync(z => z.ApplicationUserId == userId);
            return tutor.Id;
        }

        public async Task<string> GetTutorProfileImage(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(z => z.Id == userId);
            return user?.ProfileImageUrl;
        }

        public async Task<bool> UploadProfileImage(string userId, string imageUrl)
        {
            var user = await _context.Users.FirstOrDefaultAsync(z => z.Id == userId);
            user.ProfileImageUrl = imageUrl;
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> AddWishList(Guid tutorId)
        {
            var UserId = _httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);

          
            var WishList = new WishList
            {
                TutorId = tutorId,
                ApplicationUserId = UserId,

            };
            await _context.WishList.AddAsync(WishList);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
