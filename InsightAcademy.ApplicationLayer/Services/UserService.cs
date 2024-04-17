using InsightAcademy.ApplicationLayer.Repository;
using InsightAcademy.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsightAcademy.ApplicationLayer.Services
{
    public class UserService : IUserService
    {
       private readonly IUser _user;
        public UserService(IUser user)
        {
                _user = user;
        }
        public async Task<Guid> GetTutorId(string userId)
        {
           return await _user.GetTutorId(userId);
        }

        public async Task<string> GetTutorProfileImage(string userId)
        {
          return await _user.GetTutorProfileImage(userId);
        }

        public async Task<bool> UploadProfileImage(string userId, string imageUrl)
        {
            return await _user.UploadProfileImage(userId, imageUrl);
        }
      
     
    }
}
