using BusinessObjects.Models;
using DataAccessObjects.Dto;
using Repositories.interfaces;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services
{
    public class ClassService : Service<Class>, IClassService
    {
        private readonly IClassRepository _classRepository;

        public ClassService(SchoolDbContext context, IClassRepository classRepository)
            : base(context) 
        {
            _classRepository = classRepository;
        }

        public Task<ClassDetailsDto?> GetByIdAsync(Guid id)=>_classRepository.GetByIdAsync(id);

        public Task<ClassDetailsDto?> GetClassDetailsAsync(Guid classId, Guid academicYearId)=> _classRepository.GetClassDetailsAsync(classId, academicYearId);
    }
}
