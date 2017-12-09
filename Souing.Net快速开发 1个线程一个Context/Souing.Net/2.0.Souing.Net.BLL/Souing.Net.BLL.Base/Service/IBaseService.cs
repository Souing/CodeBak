using System;
using System.Collections.Generic;
using Souing.Net.Model.Base.Domain;

namespace Souing.Net.BLL.Base.Service
{
    /// <summary>
    /// 功能描述：IBaseService 
    /// 创 建 者：Administrator  2015/3/4 10:34:04
    /// </summary>
    public interface IBaseService<T> where T : BaseClass
    {
        /// <summary>
        /// 插入单个Entity
        /// </summary>
        /// <param name="TObject">单个Entity</param>
        void Insert(T TObject);

        /// <summary>
        /// 插入多个Entity
        /// </summary>
        /// <param name="TObject">Entity集合</param>
        void Insert(List<T> TObject);

        /// <summary>
        /// 根据主键修改单个
        /// </summary>
        /// <param name="TObject"></param>
        /// <param name="fields"></param>
        void Modify(T TObject, params string[] fields);

        /// <summary>
        /// 根据条件批量修改
        /// </summary>
        /// <param name="TObject"></param>
        /// <param name="query"></param>
        /// <param name="fields"></param>
        void Modify(T TObject, Func<T, bool> query, params string[] fields);

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="TObject">主键</param>
        void Delete(T TObject);

        /// <summary>
        /// 删除多个Entity
        /// </summary>
        /// <param name="query"></param>
        void Delete(Func<T, bool> query);

        /// <summary>
        /// 根据条件查找
        /// </summary>
        /// <param name="query">lambda查询条件</param>
        /// <returns></returns>
        T GetEntity(Func<T, bool> query);

        /// <summary>
        /// 获取所有Entity
        /// </summary>
        /// <param name="query">lambda查询条件</param>
        /// <returns>返回一个集合</returns>
        ICollection<T> GetEntities(Func<T, bool> query);

        /// <summary>
        ///  计算总个数(分页)
        /// </summary>
        /// <param name="query">lambda查询条件</param>
        /// <returns>返回总个数</returns>
        long GetEntitiesCount(Func<T, bool> query);

        /// <summary>
        /// 查找是否包含
        /// </summary>
        /// <param name="query">lambda查询条件</param>
        /// <returns>True Or False</returns>
        bool Contains(Func<T, bool> query);

        /// <summary>
        /// 分页查询(Linq分页方式)
        /// </summary>
        /// <param name="pagerInfo">分页类</param>
        /// <param name="sortBy">lambda排序名称</param>
        /// <param name="isAsc">是否Asc排序</param>
        /// <param name="query">lambda查询条件</param>
        /// <returns>返回一个集合</returns>
        ICollection<T> GetEntities(PagerInfo pagerInfo, Func<T, string> sortBy, bool isAsc, Func<T, bool> query);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procedureCommand"></param>
        /// <param name="sqlParams"></param>
        void ExecuteProcedure(string procedureCommand, params object[] sqlParams);


        //#region SQL语句
        ///// <summary>
        ///// 根据指定的SQL语句查询符合条件的业务对象实例集合
        ///// </summary>
        ///// <param name="commandText">查询语句</param>
        ///// <param name="parameters">参数列表</param>
        ///// <returns></returns>
        //ICollection<T> SqlQuery(string commandText, params  YstDBParameter[] parameters);

        ///// <summary>
        ///// 执行SQL语句
        ///// </summary>
        ///// <param name="commandText"></param>
        ///// <returns></returns>
        //int SqlExec(string commandText);
        ///// <summary>
        ///// 根据指定的SQL语句查询出整数结果
        ///// </summary>
        ///// <param name="commandText">查询语句</param>
        ///// <param name="parameters">参数列表</param>
        ///// <returns></returns>
        //int SqlQueryNumber(string commandText, params  YstDBParameter[] parameters);

        //IEnumerable SqlQuery(Type type, string sql, ICollection<YstDBParameter> parameters);
        //#endregion
    }
}
