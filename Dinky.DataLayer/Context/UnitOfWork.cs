
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dinky.DataLayer.Context
{
    public interface IUnitOfWork
    {
        int Save();
        IDbContextTransaction BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        ChangeTracker getChangeTracker();

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        EntityEntry<TEntity> AddEntity<TEntity>(TEntity entity)where TEntity : class;
    }
}
