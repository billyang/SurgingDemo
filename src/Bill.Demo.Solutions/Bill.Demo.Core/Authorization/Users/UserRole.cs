using System;
using System.Collections.Generic;
using System.Text;

namespace Bill.Demo.Core.Authorization.Users
{
    public class UserRole
    {
        public virtual int Id { get; set; }
        //
        // 摘要:
        //     User id.
        public virtual long UserId { get; set; }
        //
        // 摘要:
        //     Role id.
        public virtual int RoleId { get; set; }
    }
}
