using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly UserDAO _userDAO;
        private readonly SchoolDbContext _context;
        //public UserRepository(SchoolDbContext context)
        // : base(new BaseDAO<User>(context))  
        //{
        //    _context = context;
        //}

        public UserRepository(SchoolDbContext context) : base(new UserDAO(context))
        {
            _userDAO = new UserDAO(context);
        }


        public Task<User?> LoginAsync(string email, string password)
            => _userDAO.LoginAsync(email, password);

        public Task<List<User>> GetUsersByRoleAsync(string role)
            => _userDAO.GetUsersByRoleAsync(role);

        public Task<List<User>> GetStudentsByClassAsync(Guid classId)
            => _userDAO.GetStudentsByClassAsync(classId);

        public Task<User?> GetHomeroomTeacherAsync(Guid classId)
            => _userDAO.GetHomeroomTeacherAsync(classId);

        public Task<List<User>> GetParentsByStudentAsync(Guid studentId)
            => _userDAO.GetParentsByStudentAsync(studentId);

        public Task<List<User>> GetChildrenByParentAsync(Guid parentId)
            => _userDAO.GetChildrenByParentAsync(parentId);

        public Task<bool> EmailExistsAsync(string email)
            => _userDAO.EmailExistsAsync(email);

        public Task UpdateUserRoleAsync(Guid userId, string newRole)
            => _userDAO.UpdateUserRoleAsync(userId, newRole);
    }
}
