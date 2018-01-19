using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.Sys.DbHelper
{
    /// <summary>
    /// 数据库策略初始化类
    /// </summary>
    public static class DBInitializer
    {
        public static DbContextType DbContextType { get; set; }
        /// <summary>
        /// 数据库初始化策略配置
        /// </summary>`
        public static void Initialize(DbContextType ContextType)
        {
            string IsUsedWR = System.Configuration.ConfigurationManager.AppSettings["IsUsedWR"];
            DbContextType = ContextType;
            ///获得数据库最后一个版本
            //   Database.SetInitializer<DBContextHelper>(new MigrateDatabaseToLatestVersion<DBContextHelper, DBConfiguration>());
            if (ContextType == DbContextType.SqlService)
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<WriteSqlServiceContext, WriteSqlServiceDBConfiguration>());
                if (IsUsedWR == "1") {
                    Database.SetInitializer(new MigrateDatabaseToLatestVersion<ReadSqlServiceContext, ReadSqlSqlServiceDBConfiguration>());
                }
                else
                {
                    Database.SetInitializer<ReadSqlServiceContext>(null);
                }
            }
            else
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<WriteMySqlContext, WriteMySqlDBConfiguration>());
                if (IsUsedWR == "1")
                {
                    Database.SetInitializer(new MigrateDatabaseToLatestVersion<ReadMySqlContext, ReadMySqlDBConfiguration>());
                }
                else
                {
                    Database.SetInitializer<ReadMySqlContext>(null);
                }                
                //Database.SetInitializer<WriteMySqlContext>(null);
               // Database.SetInitializer<ReadMySqlContext>(null);
            }
            // Database.SetInitializer<DBContextHelper>(null);
            ///删除原来数据库 重新创建数据库
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ContextHelper>());
            // Database.SetInitializer<ContextHelper>(new DropCreateDatabaseIfModelChanges<ContextHelper>());
        }
    }
}
