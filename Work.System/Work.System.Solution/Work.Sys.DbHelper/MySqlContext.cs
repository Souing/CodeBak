using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Sys.DbModel;

namespace Work.Sys.DbHelper
{
    /// <summary>
    /// MySql操作类
    /// </summary>
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MySqlContext : DbContext
    {
        public DbSet<Test> TestEntities { get; set; }
        /// <summary>
        /// 配置默认的字符串链接
        /// </summary>
        public MySqlContext() : base("DefaultConnection") {
        }
        /// <summary>
        /// 自定义数据库链接
        /// </summary>
        /// <param name="connenction"></param>
        public MySqlContext(string connenction) : base(connenction) { }
        /// <summary>
        /// 实体对应规则的映射配置
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
    /// <summary>
    /// 写
    /// </summary>
    public class WriteMySqlContext : MySqlContext {     
        public WriteMySqlContext() : base("WriteMySqlConnection") { }
    }
    /// <summary>
    /// 读
    /// </summary>
    public class ReadMySqlContext : MySqlContext {
        public ReadMySqlContext() : base("ReadMySqlConnection") { }
    }
}
