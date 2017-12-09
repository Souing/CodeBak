using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using Souing.Net.Entity.Admin;
using Souing.Net.Entity.Map;

namespace Souing.Net.Entity
{
    /// <summary>
    /// 功能描述：SouingContext 
    /// 创 建 者：Administrator  2015/3/30 14:05:24
    /// </summary>
    public class SouingContext : DbContext
    {
        public SouingContext(string name = "SouingContext")
            : base("name=" + name)
        {
            Database.SetInitializer<SouingContext>(new DropCreateDatabaseIfModelChanges<SouingContext>());
            // 对所有实体关闭惰性加载
            // this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<UserInfoEntity> UserInfos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //表名为类名，不是上面带s的名字  移除复数表名的契约
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //引用外部的配置
            modelBuilder.Configurations.Add(new UserInfoMap());

            //复杂类型配置
            base.OnModelCreating(modelBuilder);
        }
    }
}
