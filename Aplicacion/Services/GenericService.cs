using System.Linq.Expressions;


namespace Aplicacion.Services
{
    public interface IGenericService<T> where T : class
    {
        Task<List<T>> GetAsync();

        Task<T> FindAsync(long Id);

        public T FindPivot(long AId, long BId);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> whereCondition = null,
                           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                           string includeProperties = "");
        //Task<bool> CreateRangeAsync(IEnumerable<T> lista);

        Task<bool> CreateAsync(T entity);

        Task<bool> UpdateAsync(long id, T entity);

        Task<bool> UpdatePivotAsync(long AId, long BId, T entity);

        Task<bool> DeleteAsync(long id);

        Task<bool> DeletePivotAsync(long AId, long BId);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> whereCondition = null);


    }

    public class GenericService<T> : IGenericService<T> where T : class
    {
        public IGenericRepository<T> _genericRepository { get; }

        public GenericService(IGenericRepository<T> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<T> FindAsync(long Id)
        {
            return await _genericRepository.FindAsync(Id);
        }

        public T FindPivot(long AId, long BId)
        {
            return _genericRepository.FindPivot(AId, BId);
        }

        public async Task<bool> CreateAsync(T entity)
        {
            return await _genericRepository.CreateAsync(entity);
        }


        public async Task<bool> DeleteAsync(long id)
        {
           return await _genericRepository.DeleteAsync(id);
        }

        public async Task<bool> DeletePivotAsync(long AId, long BId)
        {
            return await _genericRepository.DeletePivotAsync(AId,BId);
        }

        public async Task<List<T>> GetAsync()
        {
            return await _genericRepository.GetAsync();
        }

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> whereCondition = null,
                                  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                  string includeProperties = "")
        {
            return await _genericRepository.GetAsync(whereCondition,orderBy,includeProperties);
        }           

        public async Task<bool> UpdateAsync(long id,T entity)
        {
            return await _genericRepository.UpdateAsync(id,entity);
        }


        public async Task<bool> UpdatePivotAsync(long AId, long BId, T entity)
        {
            return await _genericRepository.UpdatePivotAsync(AId, BId, entity);
        }

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> whereCondition = null)
        {
            return _genericRepository.ExistsAsync(whereCondition);
        }
    }    

}