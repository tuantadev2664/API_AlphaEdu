using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.interfaces
{
    public interface IStudentServices
    {
        Task<List<User>> GetAllStudentsAsync();

        Task<User?> GetStudentByIdAsync(Guid id);

        Task<List<User>> SearchStudentsByNameAsync(string keyword);

        Task<List<User>> GetStudentsByClassAsync(Guid classId, Guid academicYearId);

        Task<List<User>> GetStudentsBySchoolAsync(Guid schoolId);
    }
}
