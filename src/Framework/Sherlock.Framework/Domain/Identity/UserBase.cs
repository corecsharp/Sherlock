using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Domain
{
    public class UserBase : IUser
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool LockoutEnabled { get; set; }

        public DateTime? LockoutEnd { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public string Language { get; set; } = "zh-Hans";

        public string TimeZone { get; set; } = "China Standard Time";

        public string NormalizedUserName { get; set; }

        public string SecurityStamp { get; set; } = Guid.NewGuid().ToString("N");

        public int AccessFailedCount { get; set; }

        public string PasswordHash { get; set; }
    }
}
