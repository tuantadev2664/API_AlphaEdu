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
    public class StudentRepository : IStudentRepository
    {
        public Task<List<User>> GetAllStudentsAsync() => StudentDAO.GetAllStudentsAsync();

        public Task<User?> GetStudentByIdAsync(Guid id) => StudentDAO.GetStudentByIdAsync(id);

        public Task<List<User>> GetStudentsByClassAsync(Guid classId, Guid academicYearId)=>StudentDAO.GetStudentsByClassAsync(classId, academicYearId);

        public Task<List<User>> GetStudentsBySchoolAsync(Guid schoolId) => StudentDAO.GetStudentsBySchoolAsync
            (schoolId);

        public Task<List<User>> SearchStudentsByNameAsync(string keyword)=>StudentDAO.SearchStudentsByNameAsync(keyword);
    }
}
