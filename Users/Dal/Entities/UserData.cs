using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Users.Dal.Entities
{
    internal class UserData
    {
        public Guid UserId { get; set; }
        public String UserName { get; set; } = null!;
        public String UserEmail { get; set; } = null!;
        public String? UserEmailCode { get; set; }
        public DateTime UserDelAt{ get; set; }  
    }
}
