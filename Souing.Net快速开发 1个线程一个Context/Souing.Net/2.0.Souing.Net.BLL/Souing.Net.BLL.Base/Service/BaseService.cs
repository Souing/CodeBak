using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using Souing.Net.Model.Base.Domain;

namespace Souing.Net.BLL.Base.Service
{
    /// <summary>
    /// 功能描述：BaseService 
    /// 创 建 者：Administrator  2015/3/4 10:49:06
    /// </summary>
    public class BaseService<T> : IBaseService<T> where T : BaseClass
    {
        protected readonly DbContext Context;

        public BaseService(DbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// 添加一个Entity
        /// </summary>
        /// <param name="TObject"></param>
        public virtual void Insert(T TObject)
        {
            Context.Set<T>().Add(TObject);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="TObject"></param>
        public virtual void Insert(List<T> TObject)
        {
            TObject.ForEach
                (
                     p => Context.Set<T>().Add(p)
                );
        }

        /// <summary>
        /// 修改单个Entity
        /// </summary>
        /// <param name="TObject"></param>
        /// <param name="fields"></param>
        public virtual void Modify(T TObject, params string[] fields)
        {
            Context.Set<T>().Attach(TObject);
            if (fields.Length == 0)
            {
                Context.Entry(TObject).State = EntityState.Modified;
            }
            else
            {
                Context.Entry(TObject).State = EntityState.Unchanged;
                foreach (string tempStr in fields)
                {
                    Context.Entry(TObject).Property(tempStr).IsModified = true;
                }
            }
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="TObject"></param>
        /// <param name="query"></param>
        /// <param name="fields"></param>
        public virtual void Modify(T TObject, Func<T, bool> query, params string[] fields)
        {
            //先根据条件查出符合要修改的集合
            List<T> tempList = Context.Set<T>().Where(query).ToList();
            //获取类型
            Type t = typeof(T);
            //利用反射获取T类型public属性集合
            List<PropertyInfo> tempPro = t.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            var propertyDic = new Dictionary<string, PropertyInfo>();
            //遍历T的所有属性，将符合修改的存入字典中
            tempPro.ForEach(p => { if (fields.Contains(p.Name)) { propertyDic.Add(p.Name, p); } });
            //遍历要修改的属性
            foreach (string tempStr in fields)
            {
                if (propertyDic.ContainsKey(tempStr))
                {
                    PropertyInfo propertyInfo = propertyDic[tempStr];
                    //获取要修改属性的值
                    object value = propertyInfo.GetValue(TObject, null);
                    foreach (T tempData in tempList)
                    {
                        //设置值
                        propertyInfo.SetValue(tempData, value, null);
                    }
                }
            }
        }

        /// <summary>
        /// 删除单个Entity
        /// </summary>
        /// <param name="TObject"></param>
        public virtual void Delete(T TObject)
        {
            Context.Set<T>().Attach(TObject);
            Context.Entry(TObject).State = EntityState.Deleted;
            Context.Set<T>().Remove(TObject);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="query"></param>
        public virtual void Delete(Func<T, bool> query)
        {
            List<T> tempList = Context.Set<T>().Where(query).ToList();
            tempList.ForEach(
                p =>
                {
                    Context.Set<T>().Attach(p);
                    Context.Entry(p).State = EntityState.Deleted;
                    Context.Set<T>().Remove(p);
                });
        }

        /// <summary>
        /// 得到一个Entity
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual T GetEntity(Func<T, bool> query)
        {
            return Context.Set<T>().Where<T>(query).FirstOrDefault();
        }

        /// <summary>
        /// 得到一个Entity集合
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual ICollection<T> GetEntities(Func<T, bool> query)
        {
            return Context.Set<T>().Where<T>(query).ToList();
        }

        /// <summary>
        /// 得到符合条件的数量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual long GetEntitiesCount(Func<T, bool> query = null)
        {
            return query == null ? Context.Set<T>().LongCount() : Context.Set<T>().Where(query).LongCount();
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual bool Contains(Func<T, bool> query)
        {
            return Context.Set<T>().Any(query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagerInfo"></param>
        /// <param name="sortBy"></param>
        /// <param name="isAsc"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual ICollection<T> GetEntities(PagerInfo pagerInfo, Func<T, string> sortBy, bool isAsc, Func<T, bool> query)
        {
            pagerInfo.TotalCount = Context.Set<T>().LongCount(query);
            if (isAsc)
            {
                return Context.Set<T>().Where(query).OrderBy(sortBy).Skip(pagerInfo.PageSize * (pagerInfo.PageIndex - 1)).
                    Take(pagerInfo.PageSize).ToList();
            }
            return Context.Set<T>().Where(query).OrderByDescending(sortBy).Skip(pagerInfo.PageSize * (pagerInfo.PageIndex - 1)).
                Take(pagerInfo.PageSize).ToList();
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procedureCommand"></param>
        /// <param name="sqlParams"></param>
        public void ExecuteProcedure(string procedureCommand, params object[] sqlParams)
        {
            Context.Database.ExecuteSqlCommand(procedureCommand, sqlParams);
        }
    }
}
