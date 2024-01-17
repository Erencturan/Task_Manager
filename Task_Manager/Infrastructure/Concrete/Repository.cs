using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Task_Manager.Core.Abstract.Services;

namespace Task_Manager.Infrastructure.Concrete
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }


        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }


        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
             _context.SaveChangesAsync();

        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
             _context.SaveChangesAsync();

        }


    }
}
