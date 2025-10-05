using BusinessObjects.Models;
using DataAccessObjects;
using DataAccessObjects.Dto;
using Repositories.Interfaces;
using Repositories.repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class StudentRepository : Repository<User>, IStudentRepository
    {
        private readonly StudentDAO _studentDAO;

        public StudentRepository(SchoolDbContext context)
            : base(new StudentDAO(context))
        {
            _studentDAO = new StudentDAO(context);
        }

        public async Task<List<User>> GetAllStudentsAsync()
            => await _studentDAO.GetAllStudentsAsync();

       

        public async Task<List<User>> SearchStudentsByNameAsync(string keyword)
            => await _studentDAO.SearchStudentsByNameAsync(keyword);

        //public async Task<List<User>> GetStudentsByClassAsync(Guid classId, Guid academicYearId)
        //    => await _studentDAO.GetStudentsByClassAsync(classId, academicYearId);

        public async Task<List<User>> GetStudentsBySchoolAsync(Guid schoolId)
            => await _studentDAO.GetStudentsBySchoolAsync(schoolId);

        public Task<StudentDetailDto?> GetStudentByIdAsync(Guid id)=> _studentDAO.GetStudentByIdAsync(id);

        public Task<List<object>> GetStudentsByClassAsync(Guid classId, Guid academicYearId, Guid termId) => _studentDAO.GetStudentsByClassAsync(classId,academicYearId, termId);
    }
}
