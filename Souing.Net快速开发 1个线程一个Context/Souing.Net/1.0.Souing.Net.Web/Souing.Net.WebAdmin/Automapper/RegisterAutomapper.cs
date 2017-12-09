using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Souing.Net.Entity.Admin;
using Souing.Net.ViewModel.Admin.UserInfo;

namespace Souing.Net.Admin.Automapper
{
    public static class RegisterAutomapper
    {
        public static void Excute()
        {
            // UserInfo
            Mapper.CreateMap<UserInfoViewModel, UserInfoEntity>();

            Mapper.CreateMap<UserInfoEntity, UserInfoViewModel>();//.ForMember(dest => dest.ValidatorCode, sort => sort.Ignore());

        }

    }
}