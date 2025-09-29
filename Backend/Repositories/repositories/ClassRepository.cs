using BusinessObjects.Models;
using DataAccessObjects;
using DataAccessObjects.Dto;
using Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.repositories
{
    public class ClassRepository : Repository<Class>, IClassRepository
    {
        private readonly ClassDAO _classDAO;

        public ClassRepository(SchoolDbContext context) : base(new ClassDAO(context))
        {
            _classDAO = new ClassDAO(context);
        }

        public Task<ClassDetailsDto?> GetByIdAsync(Guid id)=> _classDAO.GetByIdAsync(id);   

        public Task<ClassDetailsDto?> GetClassDetailsAsync(Guid classId, Guid academicYearId)=> _classDAO.GetClassDetailsAsync(classId, academicYearId);
    }
}
