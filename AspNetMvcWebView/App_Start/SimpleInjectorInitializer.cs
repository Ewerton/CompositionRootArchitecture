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
        /// <summary>
        /// Initialize the container and register it as MVC3 Dependency Resolver.
        /// </summary>
        public static void Initialize()
        {
            CompositionRoot.RegisterDependencies(new WebRequestLifestyle());

            // Mvc Controllers are dependencies of the view and should be registered as follows and not in the CompositionRoot to avoid circular dependency.
            CompositionRoot.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            DependencyResolver.SetResolver(CompositionRoot.GetDependencyResolver());

            CompositionRoot.Verify();
        }
    }
}