using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.interfaces;
using Repositories.repositories;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.services
{
    public class Service<T> : IService<T> where T : class
    {
        protected readonly IRepository<T> _repository;

        public Service(SchoolDbContext context)
        {
            _repository = new Repository<T>(new BaseDAO<T>(context));
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<T?> GetByIdAsync(object id)
            => await _repository.GetByIdAsync(id);

        public async Task<T?> GetByAsync(Expression<Func<T, bool>> predicate)
            => await _repository.GetByAsync(predicate);

        public async Task<IEnumerable<T>> GetAllByAsync(Expression<Func<T, bool>> predicate)
            => await _repository.GetAllByAsync(predicate);

        public async Task AddAsync(T entity)
            => await _repository.AddAsync(entity);

        public async Task UpdateAsync(T entity)
            => await _repository.UpdateAsync(entity);

        public async Task DeleteAsync(object id)
            => await _repository.DeleteAsync(id);

        public IQueryable<T> GetQueryable()
            => _repository.GetQueryable();
    }
}
