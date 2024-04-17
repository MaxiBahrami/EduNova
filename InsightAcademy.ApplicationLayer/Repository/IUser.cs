using InsightAcademy.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsightAcademy.ApplicationLayer.Repository
{
    public interface IUser
    {
        Task<Guid> GetTutorId(string userId);
        Task<string> GetTutorProfileImage(string userId);
        Task<bool> UploadProfileImage(string userId, string imageUrl);
  


    }
}
