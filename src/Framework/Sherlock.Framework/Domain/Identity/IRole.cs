using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Domain
{
    public interface IRole
    {
        long Id { get; set; }
        string RoleName { get; set; }

        string DisplayName { get; set; }
    }
}
