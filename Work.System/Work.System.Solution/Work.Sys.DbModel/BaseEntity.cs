using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.Sys.DbModel
{
    /// <summary>
    ///  基础类型实体
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseEntity<TEntity>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public TEntity ID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }
    }
}
