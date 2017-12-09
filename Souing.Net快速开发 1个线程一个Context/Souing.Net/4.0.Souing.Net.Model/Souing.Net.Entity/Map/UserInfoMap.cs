using Souing.Net.Entity.Admin;

namespace Souing.Net.Entity.Map
{
    /// <summary>
    /// 后台用户表
    /// </summary>
    public class UserInfoMap : BaseClassWithMap<UserInfoEntity>
    {
        public UserInfoMap()
        {
            this.ToTable("SysUserInfo");
            this.Property(u => u.UserName).IsRequired().HasMaxLength(30);
            this.Property(u => u.NickName).IsRequired().HasMaxLength(50);
            this.Property(u => u.PassWord).IsRequired().HasMaxLength(32);
        }
    }
}
