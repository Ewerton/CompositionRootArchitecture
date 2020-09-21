using Business;
using Data;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositionRoot
{
    public static class CompositionRoot
    {
        public static void RegisterDependencies(Container container)
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
    }
}
