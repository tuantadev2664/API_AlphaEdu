using BusinessObjects.Models;
using DataAccessObjects.Dto;
using Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    // Student repository kế thừa repository base
    public interface IStudentRepository : IRepository<User>
    {
        Task<List<User>> GetAllStudentsAsync();
        Task<StudentDetailDto?> GetStudentByIdAsync(Guid id);
        Task<List<User>> SearchStudentsByNameAsync(string keyword);
        //Task<List<User>> GetStudentsByClassAsync(Guid classId, Guid academicYearId);
        Task<List<object>> GetStudentsByClassAsync(Guid classId, Guid academicYearId, Guid termId);
        Task<List<User>> GetStudentsBySchoolAsync(Guid schoolId);
    }
}
