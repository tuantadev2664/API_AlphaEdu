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
    public class StudentServices : IStudentServices
    {
        private readonly IStudentRepository repo;

        public StudentServices(IStudentRepository repository) { repo = repository; }
        public Task<List<User>> GetAllStudentsAsync()=> repo.GetAllStudentsAsync();

        public Task<User?> GetStudentByIdAsync(Guid id)=>repo.GetStudentByIdAsync(id);

        public Task<List<User>> GetStudentsByClassAsync(Guid classId, Guid academicYearId)=>repo.GetStudentsByClassAsync(classId, academicYearId);

        public Task<List<User>> GetStudentsBySchoolAsync(Guid schoolI) => repo.GetStudentsBySchoolAsync(schoolI);

        public Task<List<User>> SearchStudentsByNameAsync(string keyword)=> repo.SearchStudentsByNameAsync(keyword);
    }
}
