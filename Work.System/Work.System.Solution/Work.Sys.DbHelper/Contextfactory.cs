using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.Sys.DbHelper
{
    /// <summary>
    /// 上下文工厂类
    /// </summary>
    public static class Contextfactory
    {
        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <returns></returns>
        public  static DbContext GetContext(DbOpertionType OpertionType)
        {
            DbContextType ContextType = DBInitializer.DbContextType;
            if (ContextType == DbContextType.MySql)
            {
                if (OpertionType == DbOpertionType.Read)
                    return new ReadMySqlContext();
                else
                    return new WriteMySqlContext();
            }
            else
            {
                if (OpertionType == DbOpertionType.Read)
                    return new ReadSqlServiceContext();
                else
                    return new WriteSqlServiceContext();
            }
        }
        /// <summary>
        /// 获取上下文操作
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="OpertionType"></param>
        /// <returns></returns>
        public static TEntity CallContext<TEntity>(DbOpertionType OpertionType) where TEntity: DbContext
        {
            var DbContext = GetContext(OpertionType);
            return (TEntity)DbContext;
        }
    }
}
