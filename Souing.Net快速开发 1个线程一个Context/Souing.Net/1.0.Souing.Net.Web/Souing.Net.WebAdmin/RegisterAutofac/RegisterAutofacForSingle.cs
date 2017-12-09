using System.Data.Entity;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Souing.Net.BLL.IService;
using Souing.Net.BLL.Service;
using Souing.Net.Entity;

namespace Souing.Net.Admin.RegisterAutofac
{
    public static class RegisterAutofacForSingle
    {
        public static void RegisterAutofac()
        {
            var builder = new ContainerBuilder();
            
            #region IOC注册区域
            // 注册builder, 实现one context per request
            builder.RegisterType<SouingContext>().As<DbContext>();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // UserInfo
            builder.RegisterType<UserInfoService>().As<IUserInfoService>();

            //// 倘若需要默认注册所有的，请这样写（主要参数需要修改）
           //  builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
           //    .AsImplementedInterfaces();
           

          
            #endregion
            // then
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


          
        }

       
    }
}
