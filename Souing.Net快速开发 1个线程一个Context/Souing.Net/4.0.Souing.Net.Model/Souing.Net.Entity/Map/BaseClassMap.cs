using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Souing.Net.Model.Base.Domain;

namespace Souing.Net.Entity.Map
{
    /// <summary>
    /// 功能描述：BaseClassMap 
    /// 创 建 者：Administrator  2015/3/30 14:15:14
    /// </summary>
    public class BaseClassMap<T> : EntityTypeConfiguration<T> where T : BaseClass
    {
        public BaseClassMap()
        {
            this.HasKey(u => u.SId);
            this.Property(u => u.SId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
