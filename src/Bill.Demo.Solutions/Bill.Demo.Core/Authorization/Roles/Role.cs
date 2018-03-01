using System;
using System.ComponentModel.DataAnnotations;

namespace Bill.Demo.Core.Authorization.Roles
{
    public class Role
    {        
        
        [Required]
        [StringLength(32)]
        public virtual string Name { get; set; }
        
        [Required]
        [StringLength(64)]
        public virtual string DisplayName { get; set; }
        
        public virtual bool IsStatic { get; set; }
        
        public virtual bool IsDefault { get; set; }
        

    }
}
