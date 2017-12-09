using System.Data.Entity.ModelConfiguration;
using Souing.Net.Model.Base.Domain;

namespace Souing.Net.Entity.Map
{
    /// <summary>
    /// 功能描述：BaseClassWithMap 
    /// 创 建 者：Administrator  2015/3/30 14:15:14
    /// </summary>
    public class BaseClassWithMap<T> : BaseClassMap<T> where T : BaseClassWith
    {
        public BaseClassWithMap()
        {
            this.Property(u => u.CreatedOnUTC).IsOptional();
            this.Property(u => u.UpdatedOnUTC).IsOptional();
            this.Property(u => u.IsAudit).IsOptional();
            this.Property(u => u.IsDeleted).IsOptional();
            this.Property(u => u.Modifier).IsOptional();
            this.Property(u => u.Creator).IsOptional();
        }
    }
}
