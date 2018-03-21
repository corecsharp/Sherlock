using Sherlock.Framework.Web;
using Sherlock.Framework.Web.DependencyInjection;
using System;

namespace Sherlock
{
    public static class FluentValidtionExtensions
    {
        private static Guid _module = Guid.NewGuid();

        /// <summary>
        /// 为 MVC 添加 FluentValidation 支持。
        /// </summary>
        /// <param name="builder"></param>
        public static void AddFluentValidationForMvc(this SherlockWebBuilder builder)
        {
            if (builder.AddedModules.Add(_module))
            {
                builder.AddStarter(new FluentValidationStarter());
            }
        }

        

    }
}
