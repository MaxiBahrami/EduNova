using InsightAcademy.ApplicationLayer.Repository;
using InsightAcademy.DomainLayer.Entities;
using InsightAcademy.InfrastructureLayer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InsightAcademy.InfrastructureLayer.Implementation
{
    public class WishListRepository : IWishList
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WishListRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
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
        public async Task<IEnumerable<WishList>> GetAllWishList()
        {
            var UserId = _httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _context.WishList.Include(x=>x.Tutor).Include(x=>x.ApplicationUser).Where(x=>x.ApplicationUserId==UserId).ToListAsync();

         
            return result;

        }
    }
}
