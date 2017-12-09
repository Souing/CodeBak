using System;

namespace Souing.Net.Model.Base.Domain
{
    /// <summary>
    /// 功能描述：BassClassWith 
    /// 创 建 者：Administrator  2015/3/4 10:37:35
    /// </summary>
    public class BaseClassWith : BaseClass
    {
        // 摘要:
        //     该对象的创建时间
        // ReSharper disable once InconsistentNaming
        public virtual DateTime CreatedOnUTC { get; set; }
        //
        // 摘要:
        //     该对象的创建者
        public virtual string Creator { get; set; }
        //
        // 摘要:
        //     是否审核通过
        public virtual int IsAudit { get; set; }
        //
        // 摘要:
        //     是否删除，很多数据无需物理删除，只进行逻辑删除
        public virtual int IsDeleted { get; set; }
        //
        // 摘要:
        //     该对象最后一次的修改者
        public virtual string Modifier { get; set; }
        //
        // 摘要:
        //     该对象的最后一次修改时间
        // ReSharper disable once InconsistentNaming
        public virtual DateTime UpdatedOnUTC { get; set; }
    }
}
