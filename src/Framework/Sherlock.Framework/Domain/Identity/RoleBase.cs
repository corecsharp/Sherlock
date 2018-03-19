using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Domain
{
    public class RoleBase : IRole
    {
        public long Id { get; set; }
        public string RoleName { get; set; }
        public string DisplayName { get; set; }
    }
}
