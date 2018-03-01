using ProtoBuf;
using Surging.Core.System.Intercept;
using System;

namespace Bill.Demo.IModuleServices.Users.Dto
{
    [ProtoContract]
    public class UserDto 
    {
        [ProtoMember(1)]
        [CacheKey(1)]
        public long Id { get; set; }

        [ProtoMember(2)]
        public string PhoneNumber { get; set; }

        [ProtoMember(3)]
        public string Name { get; set; }

        [ProtoMember(4)]
        public string Password { get; set; }

        [ProtoMember(5)]
        public string FullName { get; set; }

        [ProtoMember(6)]
        public string Surname { get; set; }

        [ProtoMember(7)]
        public string EmailAddress { get; set; }

        [ProtoMember(8)]
        public int? TenantId { get; set; }
        
    }
}
