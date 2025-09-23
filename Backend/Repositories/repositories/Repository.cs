using DataAccessObjects;
using Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly BaseDAO<T> _baseDAO;
        public Repository(BaseDAO<T> dao)
        {
            _baseDAO = dao;
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _baseDAO.GetAllAsync();
        public async Task<T?> GetByIdAsync(object id) => await _baseDAO.GetByIdAsync(id);
        public async Task<T?> GetByAsync(Expression<Func<T, bool>> predicate) => await _baseDAO.GetByAsync(predicate);
        public async Task AddAsync(T entity) => await _baseDAO.AddAsync(entity);
        public async Task UpdateAsync(T entity) => await _baseDAO.UpdateAsync(entity);
        public async Task DeleteAsync(object id) => await _baseDAO.DeleteAsync(id);
        public async Task<IEnumerable<T>> GetAllByAsync(Expression<Func<T, bool>> predicate) => await _baseDAO.GetAllByAsync(predicate);
        public IQueryable<T> GetQueryable() => _baseDAO.GetQueryable();
    }
}
