using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Users.Dal.Entities
{
    internal class UserAccess
    {
        public Guid AccessId { get; set; }
        public Guid UserId { get; set; }
        public Guid? RoleId { get; set; }
        public String AccessLogin { get; set; } = null!;
        public String AccessSalt { get; set; } = null!;
        public String AccessDk { get; set; } = null!;
    }
}
