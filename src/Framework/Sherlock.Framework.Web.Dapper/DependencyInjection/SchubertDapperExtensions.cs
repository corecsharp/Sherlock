using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Sherlock.Framework.DependencyInjection;
using Sherlock.Framework.Web.AspNetIdentity;
using Sherlock.Framework.Domain;
using Sherlock.Framework.Web.DependencyInjection;
using Sherlock.Framework.Web;
using Sherlock.Framework.Services;

namespace Sherlock
{
    public static class SherlockDapperExtensions
    {
        private static Guid _module = Guid.NewGuid();

        /// <summary>
        /// 加入 Asp.Net Identity ，同时使用 Dapper 持久化存储。
        /// </summary>
        /// <typeparam name="TUser">用户类型。</typeparam>
        /// <typeparam name="TRole">角色类型。</typeparam>
        /// <typeparam name="TIdentityService"><see cref="IIdentityService"/></typeparam>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static SherlockWebBuilder AddIdentityWithDapperStores<TUser, TRole, TIdentityService>(this SherlockWebBuilder builder, Action<IdentityOptions> configure = null)
            where TUser : class, IUser
            where TRole : class, IRole
            where TIdentityService : IIdentityService
        {
            if (builder.AddedModules.Add(_module))
            {
                builder.AddStarter(new DapperIdentityStarter<TUser, TRole, TIdentityService>(configure));
            }
            return builder;
        }

        /// <summary>
        /// 为 Asp.Net Identity 配置 Dapper 持久化存储。
        /// </summary>
        /// <typeparam name="TUser">用户类型。</typeparam>
        /// <typeparam name="TRole">角色类型。</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        internal static IdentityBuilder AddDapperStores<TUser, TRole>(this IdentityBuilder builder)
            where TUser : class, IUser
            where TRole : class, IRole
        {
            builder.Services.AddSmart(ServiceDescriber.Scoped<IUserStore<TUser>, DapperUserStore<TUser, TRole>>(SmartOptions.Replace));
            builder.Services.AddSmart(ServiceDescriber.Scoped<IRoleStore<TRole>, DapperRoleStore<TRole>>(SmartOptions.Replace));

            return builder;

        }
    }
}
