using BusinessObjects.Models;
using Repositories.interfaces;
using Repositories.Interfaces;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.services
{
    public class StudentServices : Service<User>, IStudentServices
    {
        private readonly IStudentRepository _studentRepository;

        public StudentServices(SchoolDbContext context, IStudentRepository studentRepository)
            : base(context)
        {
            _studentRepository = studentRepository;
        }

        public Task<List<User>> GetAllStudentsAsync()
            => _studentRepository.GetAllStudentsAsync();

        public Task<User?> GetStudentByIdAsync(Guid id)
            => _studentRepository.GetStudentByIdAsync(id);

        public Task<List<User>> GetStudentsByClassAsync(Guid classId, Guid academicYearId)
            => _studentRepository.GetStudentsByClassAsync(classId, academicYearId);

        public Task<List<User>> GetStudentsBySchoolAsync(Guid schoolId)
            => _studentRepository.GetStudentsBySchoolAsync(schoolId);

        public Task<List<User>> SearchStudentsByNameAsync(string keyword)
            => _studentRepository.SearchStudentsByNameAsync(keyword);
    }
}
