using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.Sys.DbHelper
{
    /// <summary>
    /// Sql数据库操作类
    /// </summary>
    public class SqlServiceContext : DbContext
    {
        /// <summary>
        /// 配置默认的字符串链接
        /// </summary>
        public SqlServiceContext() {
        }
        /// <summary>
        /// 自定义数据库链接
        /// </summary>
        /// <param name="connenction"></param>
        public SqlServiceContext(string connenction) : base(connenction) {
        }
        /// <summary>
        /// 实体对应规则的映射配置
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
    /// <summary>
    /// 读
    /// </summary>
    public class WriteSqlServiceContext : SqlServiceContext
    {
        public WriteSqlServiceContext() : base("") { }
    }
    /// <summary>
    /// 写
    /// </summary>
    public class ReadSqlServiceContext : SqlServiceContext
    {
        public ReadSqlServiceContext() : base("") { }
    }
}
