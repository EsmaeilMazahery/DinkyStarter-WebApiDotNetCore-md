
using System;
using System.Data;
using System.Data.Common;
using Dinky.DataLayer.Mappings;
using Dinky.Domain.DbModels;
using Dinky.Domain.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dinky.DataLayer.Context
{
    public class DatabaseContext : DbContext, IUnitOfWork
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> Builder) : base(Builder)
        {

        }
        
       // public DbSet<User> Users { get; set; }
       // public DbSet<Media> Medias { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        //    modelBuilder.ApplyConfiguration(new UserMapping());
        //    modelBuilder.ApplyConfiguration(new MediaMapping());
        }

        public int Save()
        {
            try
            {
                return SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IDbContextTransaction transaction = null;

        public IDbContextTransaction BeginTransaction()
        {
            if (null == transaction)
            {
                this.transaction = Database.BeginTransaction();
            }
            return transaction;
        }

        public void CommitTransaction()
        {
            if (null == transaction)
                throw new Exception("Transaction Not Began");
            transaction.Commit();
            transaction = null;
        }

        public void RollbackTransaction()
        {
            if (null == transaction)
                throw new Exception("Transaction Not Began");
            transaction.Rollback();
            transaction = null;
        }


        public ChangeTracker getChangeTracker()
        {
            return ChangeTracker;
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {

            return base.Set<TEntity>();
        }

        public EntityEntry<TEntity> AddEntity<TEntity>(TEntity entity) where TEntity : class
        {
            return Add(entity);
        }
    }
}
