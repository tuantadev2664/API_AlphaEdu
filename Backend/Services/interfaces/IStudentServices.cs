using BusinessObjects.Models;
using DataAccessObjects.Dto;

namespace Services.interfaces
{
    public interface IStudentServices : IService<User>
    {
        Task<List<User>> GetAllStudentsAsync();

        Task<StudentDetailDto?> GetStudentByIdAsync(Guid id);

        Task<List<User>> SearchStudentsByNameAsync(string keyword);

        Task<List<User>> GetStudentsByClassAsync(Guid classId, Guid academicYearId);

        Task<List<User>> GetStudentsBySchoolAsync(Guid schoolId);
    }
}
