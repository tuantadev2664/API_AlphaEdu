using BusinessObjects.Models;

namespace Services.interfaces
{
    public interface IStudentServices : IService<User>
    {
        Task<List<User>> GetAllStudentsAsync();

        Task<User?> GetStudentByIdAsync(Guid id);

        Task<List<User>> SearchStudentsByNameAsync(string keyword);

        Task<List<User>> GetStudentsByClassAsync(Guid classId, Guid academicYearId);

        Task<List<User>> GetStudentsBySchoolAsync(Guid schoolId);
    }
}
