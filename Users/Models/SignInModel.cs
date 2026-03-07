using csharp_all.Users.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Users.Models
{
    internal class SignInModel
    {
        public UserData UserData { get; set; } = null!;
        public UserAccess UserAccess { get; set; } = null!;
        public AccessToken AccessToken { get; set; } = null!;
    }
}
