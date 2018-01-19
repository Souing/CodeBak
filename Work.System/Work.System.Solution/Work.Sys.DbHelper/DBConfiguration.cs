using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.Sys.DbHelper
{
    /// <summary>
    /// 数据库迁移配置类
    /// </summary>
    public sealed class WriteMySqlDBConfiguration : DbMigrationsConfiguration<WriteMySqlContext>
    {
        /*
         * 数据库迁移配置
         */
        public WriteMySqlDBConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
    /// <summary>
    /// 数据库迁移配置类
    /// </summary>
    public sealed class ReadMySqlDBConfiguration : DbMigrationsConfiguration<ReadMySqlContext>
    {
        /*
         * 数据库迁移配置
         */
        public ReadMySqlDBConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
    /// <summary>
    /// 数据库迁移配置类
    /// </summary>
    public sealed class WriteSqlServiceDBConfiguration : DbMigrationsConfiguration<WriteSqlServiceContext>
    {
        /*
         * 数据库迁移配置
         */
        public WriteSqlServiceDBConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
    /// <summary>
    /// 数据库迁移配置类
    /// </summary>
    public sealed class ReadSqlSqlServiceDBConfiguration : DbMigrationsConfiguration<ReadSqlServiceContext>
    {
        /*
         * 数据库迁移配置
         */
        public ReadSqlSqlServiceDBConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}
