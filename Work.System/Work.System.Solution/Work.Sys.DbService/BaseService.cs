using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Work.Sys.DbHelper;
using Work.Sys.DbInterfaceService;
using Work.Sys.DbModel;

namespace Work.Sys.DbService
{
    /// <summary>
    /// 数据库操作基础类
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseService<TKey, TEntity> : IBaseService<TKey, TEntity> where TEntity : BaseEntity<Guid>
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity AddEntity(TEntity entity)
        {
            using (DbContext DbHelper = Contextfactory.GetContext(DbOpertionType.Write))
            {
                var NewEntity = DbHelper.Set<TEntity>().Add(entity);
                return DbHelper.SaveChanges() > 0 ? NewEntity : null;
            }
        }
        /// <summary>
        /// 异步添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> AddEntityAsy(TEntity entity)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Write))
            {
                TEntity newEntity = dbHelper.Set<TEntity>().Add(entity);
                int result = await dbHelper.SaveChangesAsync();
                if (result > 0)
                    return newEntity;
                return null;
            }
        }
        /// <summary>
        /// 添加实体集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual int AddEntity(List<TEntity> list)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Write))
            {
                dbHelper.Configuration.AutoDetectChangesEnabled = false;
                try
                {
                    dbHelper.Set<TEntity>().AddRange(list);
                    return dbHelper.SaveChanges();
                }
                finally
                {
                    dbHelper.Configuration.AutoDetectChangesEnabled = true;
                }
            }
        }
        /// <summary>
        /// 异步添加实体集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual async Task<int> AddEntityAys(List<TEntity> list)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Write))
            {
                dbHelper.Configuration.AutoDetectChangesEnabled = false;
                try
                {
                    dbHelper.Set<TEntity>().AddRange(list);
                    return await dbHelper.SaveChangesAsync();
                }
                finally
                {
                    dbHelper.Configuration.AutoDetectChangesEnabled = true;
                }
            }
        }
        /// <summary>
        /// 添加表格数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public virtual int AddTable(string tableName, DataTable table)
        {
            if (string.IsNullOrEmpty(tableName) || table == null || table.Rows.Count < 0)
                return -1;
            else
            {
                using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Write))
                {
                    try
                    {
                        if (dbHelper.Database.Connection.State != ConnectionState.Open)
                        {
                            dbHelper.Database.Connection.Open();
                            var bulkCopy = new SqlBulkCopy((SqlConnection)dbHelper.Database.Connection);
                            bulkCopy.DestinationTableName = tableName;
                            bulkCopy.WriteToServer(table);
                        }
                        return table.Rows.Count;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        if (dbHelper.Database.Connection.State != ConnectionState.Closed)
                        {
                            dbHelper.Database.Connection.Close();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 异步添加表格
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public virtual async Task<int> AddTableAsy(string tableName, DataTable table)
        {
            if (string.IsNullOrEmpty(tableName) || table == null || table.Rows.Count < 0)
                return -1;
            else
            {
                using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Write))
                {
                    try
                    {
                        if (dbHelper.Database.Connection.State != ConnectionState.Open)
                        {
                            dbHelper.Database.Connection.Open();
                            var bulkCopy = new SqlBulkCopy((SqlConnection)dbHelper.Database.Connection);
                            bulkCopy.DestinationTableName = tableName;
                            await bulkCopy.WriteToServerAsync(table);
                        }
                        return table.Rows.Count;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        if (dbHelper.Database.Connection.State != ConnectionState.Closed)
                        {
                            dbHelper.Database.Connection.Close();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity UpdateEntity(TEntity entity)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Write))
            {
                DbSet<TEntity> dbSet = dbHelper.Set<TEntity>();
                DbEntityEntry<TEntity> entry = dbHelper.Entry<TEntity>(entity);
                if (entry.State == System.Data.Entity.EntityState.Detached)
                {
                    dbSet.Attach(entity);
                    entry.State = System.Data.Entity.EntityState.Modified;
                }
                if (dbHelper.SaveChanges() >= 0)
                    return entity;
                return null;
            }
        }
        /// <summary>
        /// 异步更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> UpdateEntityAsy(TEntity entity)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Write))
            {
                DbSet<TEntity> dbSet = dbHelper.Set<TEntity>();
                DbEntityEntry<TEntity> entry = dbHelper.Entry<TEntity>(entity);
                if (entry.State == System.Data.Entity.EntityState.Detached)
                {
                    dbSet.Attach(entity);
                    entry.State = System.Data.Entity.EntityState.Modified;
                }
                int result = await dbHelper.SaveChangesAsync();
                if (result >= 0)
                    return entity;
                return null;
            }
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int DelEntity(TEntity entity)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Write))
            {
                dbHelper.Set<TEntity>().Remove(entity);
                return dbHelper.SaveChanges();
            }
        }
        /// <summary>
        /// 异步删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> DelEntityAsy(TEntity entity)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Write))
            {
                dbHelper.Set<TEntity>().Remove(entity);
                return await dbHelper.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual int DelEntity(TKey key, bool IsUpdate = false)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Write))
            {
                var entity = dbHelper.Set<TEntity>().Find(key);
                if (IsUpdate)
                {
                    entity.IsDel = true;
                }
                else
                {
                    dbHelper.Set<TEntity>().Remove(entity);
                }
                return dbHelper.SaveChanges();
            }
        }
        /// <summary>
        /// 根据主键异步删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<int> DelEntityAsy(TKey key, bool IsUpdate = false)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Write))
            {
                var entity = await dbHelper.Set<TEntity>().FindAsync(key);
                if (IsUpdate)
                {
                    entity.IsDel = true;
                }
                else
                {
                    dbHelper.Set<TEntity>().Remove(entity);
                }
                return await dbHelper.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 通过主键查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TEntity GetEntityByID(TKey key)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Read))
            {
                var entity = dbHelper.Set<TEntity>().Find(key);
                return entity;
            }
        }
        /// <summary>
        /// 异步通过主键查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetEntityByIDAsy(TKey key)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Read))
            {
                return await dbHelper.Set<TEntity>().FindAsync(key);
            }
        }
        /// <summary>
        /// 根据Lamuda查询
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        public virtual TEntity GetEntityByWhere(Expression<Func<TEntity, bool>> fun)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Read))
            {
                var entity = dbHelper.Set<TEntity>().AsNoTracking().Where(fun).FirstOrDefault();
                return entity;
            }
        }
        /// <summary>
        /// 异步根据Lamuda查询
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetEntityByWhereAsy(Expression<Func<TEntity, bool>> fun)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Read))
            {
                return await dbHelper.Set<TEntity>().AsNoTracking().Where(fun).FirstOrDefaultAsync();
            }
        }
        /// <summary>
        /// sql语句查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual List<TEntity> GetEntityQuerySql(string sql, params object[] obj)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Read))
            {
                var entity = dbHelper.Set<TEntity>().SqlQuery(sql, obj).ToList();
                return entity;
            }
        }
        /// <summary>
        ///  sql语句异步查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetEntityQuerySqlAsy(string sql, params object[] obj)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Read))
            {
                return await dbHelper.Set<TEntity>().SqlQuery(sql, obj).ToListAsync();
            }
        }
        /// <summary>
        /// Lamuda查询
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        public virtual List<TEntity> GetEntityLisrByWhere(Expression<Func<TEntity, bool>> fun)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Read))
            {
                var entity = dbHelper.Set<TEntity>().AsNoTracking().Where(fun).ToList();
                return entity;
            }
        }
        /// <summary>
        /// 异步 Lamuda查询
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetEntityLisrByWhereAsy(Expression<Func<TEntity, bool>> fun)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Read))
            {
                return await dbHelper.Set<TEntity>().AsNoTracking().Where(fun).ToListAsync();
            }
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<int> DelEntityListAsy(List<Guid> list)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Write))
            {
                var obj = await dbHelper.Set<TEntity>().Where(it => list.Contains(it.ID)).ToArrayAsync();
                dbHelper.Set<TEntity>().RemoveRange(obj);
                return await dbHelper.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 异步根据条件查询数量
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        public async Task<int> GetWhereCountAsy(Expression<Func<TEntity, bool>> fun)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Read))
            {
                return await dbHelper.Set<TEntity>().AsNoTracking().Where(fun).CountAsync();
            }
        }
        /// <summary>
        /// 根据条件查询数量
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        public int GetWhereCount(Expression<Func<TEntity, bool>> fun)
        {
            using (DbContext dbHelper = Contextfactory.GetContext(DbOpertionType.Read))
            {
                return dbHelper.Set<TEntity>().AsNoTracking().Where(fun).Count();
            }
        }
    }
}
