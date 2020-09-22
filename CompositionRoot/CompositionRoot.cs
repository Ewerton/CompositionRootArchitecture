using Business;
using Data;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CompositionRoot
{
    public class CompositionRoot
    {
        private static Container container = new Container();

        public static Container Container { get { return container; }  }

        public static void RegisterDependencies(ScopedLifestyle scopedLifestyle)
        {
            container.Options.DefaultScopedLifestyle = scopedLifestyle;

            RegisterDependencies();
        }

        public static void RegisterDependencies()
        {
            // Registrations for the Data.dll
            container.Register<BlogRepo>();
            container.Register<PostRepo>();
            container.Register<CommentRepo>();

            // Registrations for the Bussiness.dll
            container.Register<BlogBusinessRules>();
            container.Register<PostBusinessRules>();
            container.Register<CommentBusinessRules>();
        }

        public static void Register<T>() where T : class
        {
            container.Register<T>();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            // Sets up the basic configuration that for integrating Simple Injector with
            // ASP.NET Core by setting the DefaultScopedLifestyle, and setting up auto
            // cross wiring.
            services.AddSimpleInjector(container, options =>
            {
                // AddAspNetCore() wraps web requests in a Simple Injector scope and
                // allows request-scoped framework services to be resolved.
                options.AddAspNetCore()

                    // Ensure activation of a specific framework type to be created by
                    // Simple Injector instead of the built-in configuration system.
                    // All calls are optional. You can enable what you need. For instance,
                    // ViewComponents, PageModels, and TagHelpers are not needed when you
                    // build a Web API.
                    .AddControllerActivation()
                    .AddViewComponentActivation()
                    .AddPageModelActivation()
                    .AddTagHelperActivation();

                // Optionally, allow application components to depend on the non-generic
                // ILogger (Microsoft.Extensions.Logging) or IStringLocalizer
                // (Microsoft.Extensions.Localization) abstractions.
                //options.AddLogging();
                //options.AddLocalization();
            });
        }

        public static void Register<TService, TImpl>() where TService : class where TImpl : class, TService
        {
            container.Register<TService, TImpl>();
        }

        public static void RegisterMvcControllers(Assembly mvcAssembly)
        {
            // Mvc Controllers are dependencies of the view and should be registered as follows and not in the CompositionRoot to avoid circular dependency.
            container.RegisterMvcControllers(mvcAssembly);
        }

        // Create others Register() overloads to that fit your need...

        public static T GetInstance<T>() where T : class
        {
            return container.GetInstance<T>();
        }

        // Create others GetInstance() overloads to that fit your need...

        public static void Verify()
        {
            container.Verify();
        }

        public static IDependencyResolver GetDependencyResolver()
        {
            return new SimpleInjectorDependencyResolver(container);
        }
    }
}
