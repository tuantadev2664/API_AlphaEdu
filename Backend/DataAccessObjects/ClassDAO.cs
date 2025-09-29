using BusinessObjects.Models;
using DataAccessObjects.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class ClassDAO : BaseDAO<Class>
    {
        public ClassDAO(SchoolDbContext context) : base(context) { }
        public async Task<ClassDetailsDto?> GetClassDetailsAsync(Guid classId, Guid academicYearId)
        {
            var classEntity = await _dbSet
                .Include(c => c.Grade)
                .Include(c => c.HomeroomTeacher)
                .Include(c => c.ClassEnrollments.Where(ce => ce.AcademicYearId == academicYearId))
                    .ThenInclude(ce => ce.Student)
                .Include(c => c.TeacherAssignments.Where(ta => ta.AcademicYearId == academicYearId))
                    .ThenInclude(ta => ta.Subject)
                .Include(c => c.TeacherAssignments.Where(ta => ta.AcademicYearId == academicYearId))
                    .ThenInclude(ta => ta.Teacher)
                .FirstOrDefaultAsync(c => c.Id == classId);

            if (classEntity == null) return null;

            return new ClassDetailsDto
            {
                ClassId = classEntity.Id,
                ClassName = classEntity.Name,
                GradeName = classEntity.Grade?.Level ?? string.Empty,

                HomeroomTeacherId = classEntity.HomeroomTeacher?.Id,
                HomeroomTeacherName = classEntity.HomeroomTeacher?.FullName,

                StudentCount = classEntity.ClassEnrollments.Count,
                Students = classEntity.ClassEnrollments
                    .Select(ce => new StudentDto
                    {
                        StudentId = ce.Student.Id,
                        FullName = ce.Student.FullName
                    })
                    .ToList(),

                Subjects = classEntity.TeacherAssignments
                    .Select(ta => new SubjectTeacherDto
                    {
                        SubjectId = ta.Subject.Id,
                        SubjectName = ta.Subject.Name,
                        TeacherId = ta.Teacher.Id,
                        TeacherName = ta.Teacher.FullName
                    })
                    .ToList()
            };
        }

        public async Task<ClassDetailsDto?> GetByIdAsync(Guid id)
        {
            // Lấy class bất kể academicYear (không filter năm)
            var classEntity = await _dbSet
                .Include(c => c.Grade)
                .Include(c => c.HomeroomTeacher)
                .Include(c => c.ClassEnrollments)
                    .ThenInclude(ce => ce.Student)
                .Include(c => c.TeacherAssignments)
                    .ThenInclude(ta => ta.Subject)
                .Include(c => c.TeacherAssignments)
                    .ThenInclude(ta => ta.Teacher)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classEntity == null) return null;

            return new ClassDetailsDto
            {
                ClassId = classEntity.Id,
                ClassName = classEntity.Name,
                GradeName = classEntity.Grade?.Level ?? string.Empty,

                HomeroomTeacherId = classEntity.HomeroomTeacher?.Id,
                HomeroomTeacherName = classEntity.HomeroomTeacher?.FullName,

                StudentCount = classEntity.ClassEnrollments.Count,
                Students = classEntity.ClassEnrollments
                    .Select(ce => new StudentDto
                    {
                        StudentId = ce.Student.Id,
                        FullName = ce.Student.FullName
                    })
                    .ToList(),

                Subjects = classEntity.TeacherAssignments
                    .Select(ta => new SubjectTeacherDto
                    {
                        SubjectId = ta.Subject.Id,
                        SubjectName = ta.Subject.Name,
                        TeacherId = ta.Teacher.Id,
                        TeacherName = ta.Teacher.FullName
                    })
                    .ToList()
            };
        }


    }
}
