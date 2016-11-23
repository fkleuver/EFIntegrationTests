using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EFIntegrationTests
{
    public abstract class  Repository<TContext, TEntity> 
        where TEntity : class, new()
        where TContext : DbContext
    {
        private readonly TContext _ctx;

        public Repository(TContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper, CancellationToken cancellationToken)
        {
            var query = queryShaper(_ctx.Set<TEntity>());
            return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            var output = _ctx.Set<TEntity>();
            var result = output.Add(entity);
            await _ctx.SaveChangesAsync().ConfigureAwait(false);
            return result;
        }

        public async Task<bool> Delete(TEntity entity)
        {
            _ctx.Set<TEntity>().Attach(entity);
            _ctx.Entry(entity).State = EntityState.Deleted;
            await _ctx.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public async Task<bool> Update(TEntity entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
            await _ctx.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
    }
}