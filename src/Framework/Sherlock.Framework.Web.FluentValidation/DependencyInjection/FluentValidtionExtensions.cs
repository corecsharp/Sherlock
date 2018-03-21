using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Sherlock.Framework.Web;
using Sherlock.Framework.Web.DependencyInjection;
using Sherlock.Framework.Web.Validation;
using System;

namespace Sherlock
{
    public static class FluentValidtionExtensions
    {
       /// <summary>
       /// 为 MVC 添加 FluentValidation 支持。
       /// </summary>
       /// <param name="builder"></param>
        public static void AddFluentValidationForMvc(this SherlockWebBuilder builder)
        {
            builder.AddStarter(new FluentValidationStarter());
        }

        

    }
}
