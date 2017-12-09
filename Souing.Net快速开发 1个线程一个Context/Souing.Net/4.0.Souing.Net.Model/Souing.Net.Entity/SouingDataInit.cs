using System;
using System.Collections.Generic;
using System.Data.Entity;
using Souing.Net.Entity.Admin;
using Souing.Net.Model.Base.Domain;

namespace Souing.Net.Entity
{
    public class SouingDataInit : CreateDatabaseIfNotExists<SouingContext>
    {
        protected override void Seed(SouingContext context)
        {
            var userInfoList = new List<UserInfoEntity>
            {
                new UserInfoEntity
                {
                    UserName ="ljw", NickName="ljwNickName", PassWord="000000",
                        CreatedOnUTC=DateTime.Now, Creator="ljwCreator", IsAudit=1, IsDeleted=1, Modifier="ljwModifier", UpdatedOnUTC=DateTime.Now
                   
                }
            };
            userInfoList.ForEach(p => context.UserInfos.Add(p));
            base.Seed(context);
        }
    }
}
