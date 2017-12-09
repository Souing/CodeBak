using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Souing.Net.Entity.Admin;
using Souing.Net.ViewModel.Admin.UserInfo;

namespace Souing.Net.Admin.Automapper
{
    public static class MapperExtention
    {
        #region Admin

        public static UserInfoEntity ToEntity(this UserInfoViewModel model)
        {
            return Mapper.Map<UserInfoViewModel, UserInfoEntity>(model);
        }

        public static UserInfoViewModel ToModel(this UserInfoEntity entity)
        {
            return Mapper.Map<UserInfoEntity, UserInfoViewModel>(entity);
        }

        public static List<UserInfoViewModel> ToModelList(this ICollection<UserInfoEntity> entity)
        {
            return Mapper.Map<ICollection<UserInfoEntity>, List<UserInfoViewModel>>(entity);
        }

        public static UserInfoEntity ToEntity(this UserInfoViewModel model, UserInfoEntity destination)
        {
            return Mapper.Map(model, destination);
        }
        #endregion
    }
}