// Commented cause I prefer to call it explicitly in the Global.Asax
//[assembly: WebActivator.PostApplicationStartMethod(typeof(AspNetMvcWebView.SimpleInjectorInitializer), "Initialize")]

namespace AspNetMvcWebView
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using CompositionRoot;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            
            InitializeContainer(container);

            // Mvc Controllers are dependencies of the view and should be registered as follows and not in the CompositionRoot to avoid circular dependency.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            
            container.Verify();
            
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
     
        private static void InitializeContainer(Container container)
        {
            CompositionRoot.RegisterDependencies(container);
        }
    }
}