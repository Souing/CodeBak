using System.Data.Entity;
using Souing.Net.DAL.Base.Service;
using Souing.Net.DAL.IService;
using Souing.Net.Entity.Admin;

namespace Souing.Net.DAL.Service
{
    /// <summary>
    /// 功能描述：StudentService 
    /// 创 建 者：Administrator  2015/3/6 11:47:35
    /// </summary>
    internal sealed class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
        public UserInfoService(DbContext context)
            : base(context)
        {

        }
    }
}
