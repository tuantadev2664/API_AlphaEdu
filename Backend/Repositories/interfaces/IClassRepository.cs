using BusinessObjects.Models;
using DataAccessObjects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.interfaces
{
    public interface IClassRepository : IRepository<Class>
    {
        Task<ClassDetailsDto?> GetClassDetailsAsync(Guid classId, Guid academicYearId);
        Task<ClassDetailsDto?> GetByIdAsync(Guid id);
    }
}
