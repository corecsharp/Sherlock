using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Domain
{
    /// <summary>
    /// 表示一个用户实例的接口。
    /// </summary>
    public interface IUser
    {
        long Id { get; set; }
        string UserName { get; set; }

        string NormalizedUserName { get; set; }

        /// <summary>
        /// 获取或设置用户安全码。
        /// </summary>
        string SecurityStamp { get; set; }

        /// <summary>
        /// 获取或设置用户的密码 Hash 串。
        /// </summary>
        string PasswordHash { get; set; }

        /// <summary>
        ///     Email
        /// </summary>
        string Email { get; set; }

        /// <summary>
        ///     PhoneNumber for the user
        /// </summary>
        string PhoneNumber { get; set; }

        string Language { get; set; }

        string TimeZone { get; set; }

        bool EmailConfirmed { get; set; }

        bool PhoneNumberConfirmed { get; set; }

        DateTime? LockoutEnd { get; set; }

        bool LockoutEnabled { get; set; }

        int AccessFailedCount { get; set; }
    }
}
