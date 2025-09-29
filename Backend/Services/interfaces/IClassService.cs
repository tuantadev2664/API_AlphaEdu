using BusinessObjects.Models;
using DataAccessObjects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.interfaces
{
    public interface IClassService : IService<Class>
    {
        Task<ClassDetailsDto?> GetClassDetailsAsync(Guid classId, Guid academicYearId);
        Task<ClassDetailsDto?> GetByIdAsync(Guid id);
    }
}
