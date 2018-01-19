using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Work.Sys.DbHelper;

namespace Work.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Autofac
            //ContainerBuilder builder = new ContainerBuilder();
            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
            //IContainer container = builder.Build();
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            //Autofac初始化过程
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterControllers(System.Reflection.Assembly.GetExecutingAssembly());//注册mvc容器的实现  
            var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();
            builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t => t.Name.Contains("Service")).AsImplementedInterfaces();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //初始化数据库
            DBInitializer.Initialize(DbContextType.MySql);
        }
    }
}
