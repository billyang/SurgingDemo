using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bill.Demo.Core.Authorization.Users
{
    public class User 
    {
        public virtual long Id { get; set; }

        public virtual bool IsLockoutEnabled { get; set; }
        
        public virtual string PhoneNumber { get; set; }
        
        public virtual bool IsPhoneNumberConfirmed { get; set; }
        
        public virtual string SecurityStamp { get; set; }
       
        public virtual bool IsTwoFactorEnabled { get; set; }
        
        public virtual bool IsEmailConfirmed { get; set; }
        
        [ForeignKey("UserId")]
        public virtual ICollection<UserRole> Roles { get; set; }
        
        
        public virtual int AccessFailedCount { get; set; }
        
        //[ForeignKey("UserId")]
        //public virtual ICollection<UserLogin> Logins { get; set; }
        
        public virtual DateTime? LockoutEndDateUtc { get; set; }
        
        [Required]
        [StringLength(32)]
        public virtual string Name { get; set; }
        
        [StringLength(328)]
        public virtual string EmailConfirmationCode { get; set; }
        
        [Required]
        [StringLength(128)]
        public virtual string Password { get; set; }
        
        [NotMapped]
        public virtual string FullName { get; set; }
       
        [Required]
        [StringLength(32)]
        public virtual string Surname { get; set; }
       
        public virtual bool IsActive { get; set; }
       
        [Required]
        [StringLength(256)]
        public virtual string EmailAddress { get; set; }
        
        public virtual int? TenantId { get; set; }        
        
        [StringLength(328)]
        public virtual string PasswordResetCode { get; set; }
       
        public virtual DateTime? LastLoginTime { get; set; }
    }
}
