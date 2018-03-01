using System;
using System.Collections.Generic;
using System.Text;

namespace Bill.Demo.IModuleServices.Users.Dto
{
    public class CreateUserDto
    {       

        public  string PhoneNumber { get; set; }
        
        
        public  string Name { get; set; }
        
        
        public  string Password { get; set; }
        
        public  string FullName { get; set; }
        
        public  string Surname { get; set; }
        
        public  string EmailAddress { get; set; }

        public  int? TenantId { get; set; }
        
          
    }
}
