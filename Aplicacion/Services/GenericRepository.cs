using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Persistencia.Context;


namespace Aplicacion.Services
{
    public interface IGenericRepository<T> where T : class
    {       
        Task<List<T>> GetAsync();
        Task<T> FindAsync(long Id);

        T FindPivot(long AId, long BId);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> whereCondition = null,
                           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                           string includeProperties = "");
        Task<bool> CreateAsync(T entity);    
        Task<bool> UpdateAsync(long id,T entity);

        Task<bool> UpdatePivotAsync(long AId, long BId, T entity);

        Task<bool> DeleteAsync(long id);

        Task<bool> DeletePivotAsync(long AId, long BId);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> whereCondition = null);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly SqlLIteContext _context;
        public GenericRepository(SqlLIteContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAsync()
        {
            //return await _context.Actividad.ToListAsync();
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> FindAsync(long Id)
        {
            T entity;
            entity = await _context.Set<T>().FindAsync(Id);
            if(entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public  T FindPivot(long AId, long BId)
        {
            T entity;
            entity = _context.Set<T>().Find(AId,BId);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> whereCondition = null,
                                  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                  string includeProperties = "")
        {           
            IQueryable<T> query = _context.Set<T>();

            if (whereCondition != null)
            {
                query = query.Where(whereCondition);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<bool> CreateAsync(T entity)
        {
            bool created = false;
            try
            {
                var save = await _context.Set<T>().AddAsync(entity);

                if (save != null)                                  
                    created = true;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;

            }                        
            return created;
        }


        public async Task<bool> UpdateAsync(long id,T entity)
        { 
            bool edited = false;
            
            try
            {
                var entityDB = await FindAsync(id);
                if (entityDB == null)
                    return edited;

                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                edited = true;
            }
            catch (Exception ex)
            {
                throw;
            }   
            return edited;         
        }

        public async Task<bool> UpdatePivotAsync(long AId, long BId, T entity)
        {
            bool edited = false;

            try
            {
                var entityDB =FindPivot(AId, BId);
                if (entityDB == null)
                    return edited;

                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                edited = true;
            }
            catch (Exception ex)
            {
                throw;
            }
            return edited;
        }

        public async Task<bool> DeleteAsync(long id)
        { 
            bool deleted = false;
            try
            {
                var entityDB = await FindAsync(id);
                if (entityDB == null)
                    return deleted;

                var removeEntity = _context.Set<T>().Remove(entityDB);
                if (removeEntity != null)
                {
                    deleted = true;
                }
                await  _context.SaveChangesAsync();
            } 
            catch (Exception)
            {
                throw;
            }
            return deleted;
        }


        public async Task<bool> DeletePivotAsync(long AId, long BId)
        {
            bool deleted = false;
            try
            {
                var entityDB = FindPivot(AId, BId);
                if (entityDB == null)
                    return deleted;

                var removeEntity = _context.Set<T>().Remove(entityDB);
                if (removeEntity != null)
                {
                    deleted = true;
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return deleted;
        }


        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> whereCondition = null)
        {
            //p=>p.Id != null
            var count = _context.Set<T>().Where(whereCondition).Count();
            return count > 0;        
        }
    }
}