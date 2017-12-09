using Souing.Net.Model.Base.Domain;

namespace Souing.Net.Entity.Admin
{
    /// <summary>
    /// 功能描述：后台用户 
    /// 创 建 者：Administrator  2015/3/30 13:44:34
    /// </summary>
    public class UserInfoEntity : BaseClassWith
    {
        public string UserName { get; set; }

        public string PassWord { get; set; }

        public string NickName { get; set; }

        public int? Gender { get; set; }
    }
}
