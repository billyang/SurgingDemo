using ProtoBuf;
using Surging.Core.CPlatform;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bill.Demo.IModuleServices.Users.Models
{
    [ProtoContract]
    public class IdentityUser : RequestData
    {
        [ProtoMember(1)]
        public string RoleId { get; set; }
    }
}
