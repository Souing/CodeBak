using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.Sys.Cache
{
    /// <summary>
    /// 缓存基础接口
    /// </summary>
    public interface ICacheHelper
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        void AddCache();
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        void DelCache(string key);
    }
}
