using BusinessObjects.Models;
using Repositories.interfaces;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(SchoolDbContext context, IUserRepository userRepository)
            : base(context)
        {
            _userRepository = userRepository;
        }

        public Task<User?> LoginAsync(string email, string password)
            => _userRepository.LoginAsync(email, password);

        public Task<List<User>> GetUsersByRoleAsync(string role)
            => _userRepository.GetUsersByRoleAsync(role);

        public Task<List<User>> GetStudentsByClassAsync(Guid classId)
            => _userRepository.GetStudentsByClassAsync(classId);

        public Task<User?> GetHomeroomTeacherAsync(Guid classId)
            => _userRepository.GetHomeroomTeacherAsync(classId);

        public Task<List<User>> GetParentsByStudentAsync(Guid studentId)
            => _userRepository.GetParentsByStudentAsync(studentId);

        public Task<List<User>> GetChildrenByParentAsync(Guid parentId)
            => _userRepository.GetChildrenByParentAsync(parentId);

        public Task<bool> EmailExistsAsync(string email)
            => _userRepository.EmailExistsAsync(email);

        public Task UpdateUserRoleAsync(Guid userId, string newRole)
            => _userRepository.UpdateUserRoleAsync(userId, newRole);
    }
}
