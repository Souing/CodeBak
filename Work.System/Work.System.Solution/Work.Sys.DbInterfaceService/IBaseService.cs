using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Work.Sys.DbModel;

namespace Work.Sys.DbInterfaceService
{
    /// <summary>
    /// 数据库操作基础接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseService<TKey,TEntity> where TEntity:BaseEntity<Guid>
    {
        /// <summary>
        ///  大量添加数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        int AddTable(string tableName, DataTable table);
        Task<int> AddTableAsy(string tableName, DataTable table);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity AddEntity(TEntity entity);
        Task<TEntity> AddEntityAsy(TEntity entity);
        /// <summary>
        ///  批量添加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        int AddEntity(List<TEntity> list);
        Task<int> AddEntityAys(List<TEntity> list);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity UpdateEntity(TEntity entity);
        Task<TEntity> UpdateEntityAsy(TEntity entity);
        /// <summary>
        /// 删除 实体
        /// </summary>
        /// <param name="TEntity"></param>
        /// <returns></returns>
        int DelEntity(TEntity entity);
        Task<int> DelEntityAsy(TEntity entity);
        /// <summary>
        /// 删除  主键
        /// </summary>
        /// <param name="TEntity"></param>
        /// <returns></returns>
        int DelEntity(TKey key,bool IsUpdate=false);
        Task<int> DelEntityAsy(TKey key, bool IsUpdate = false);
        /// <summary>
        /// 删除  条件
        /// </summary>
        /// <param name="TEntity"></param>
        /// <returns></returns>
        Task<int> DelEntityListAsy(List<Guid> list);
        /// <summary>
        /// 根据ID进行查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TEntity GetEntityByID(TKey key);
        Task<TEntity> GetEntityByIDAsy(TKey key);
        /// <summary>
        /// 根据sql进行查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<TEntity> GetEntityQuerySql(string sql,params object[] obj);
        Task<List<TEntity>> GetEntityQuerySqlAsy(string sql, params object[] obj);
        /// <summary>
        /// 根据条件进行查询
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        List<TEntity> GetEntityLisrByWhere(Expression<Func<TEntity, bool>> fun);
        Task<List<TEntity>> GetEntityLisrByWhereAsy(Expression<Func<TEntity, bool>> fun);
        /// <summary>
        /// 根据条件进行查询
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        TEntity GetEntityByWhere(Expression<Func<TEntity, bool>> fun);

        TEntity GetEntityByWhere2(Func<TEntity, bool> fun);
        Task<TEntity> GetEntityByWhereAsy(Expression<Func<TEntity, bool>> fun);

        /// <summary>
        /// 根据条件查询数量
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        Task<int> GetWhereCountAsy(Expression<Func<TEntity, bool>> fun);
        int GetWhereCount(Expression<Func<TEntity, bool>> fun);


    }
}
