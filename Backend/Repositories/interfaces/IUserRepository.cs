using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> LoginAsync(string email, string password);
        Task<List<User>> GetUsersByRoleAsync(string role);
        Task<List<User>> GetStudentsByClassAsync(Guid classId);
        Task<User?> GetHomeroomTeacherAsync(Guid classId);
        Task<List<User>> GetParentsByStudentAsync(Guid studentId);
        Task<List<User>> GetChildrenByParentAsync(Guid parentId);
        Task<bool> EmailExistsAsync(string email);
        Task UpdateUserRoleAsync(Guid userId, string newRole);
    }
}
