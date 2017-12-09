using System.Data.Entity;
using Souing.Net.BLL.Base.Service;
using Souing.Net.BLL.IService;
using Souing.Net.Entity.Admin;

namespace Souing.Net.BLL.Service
{
    /// <summary>
    /// 功能描述：StudentService 
    /// 创 建 者：Administrator  2015/3/6 11:47:35
    /// </summary>
    public class UserInfoService : BaseService<UserInfoEntity>, IUserInfoService
    {
        public UserInfoService(DbContext context)
            : base(context)
        {
        }
    }
}
