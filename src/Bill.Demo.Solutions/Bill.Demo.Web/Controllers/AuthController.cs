using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bill.Demo.IModuleServices.Users;
using Bill.Demo.IModuleServices.Users.Dto;
using Microsoft.AspNetCore.Mvc;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.ProxyGenerator;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bill.Demo.Web.Controllers
{
    public class AuthController : Controller
    {
        // GET: /<controller>/
        public async Task<IActionResult> User()
        {
            return View();
        }

        public async Task<JsonResult> GetAllUsers()
        {
            var service = ServiceLocator.GetService<IServiceProxyFactory>();
            var userProxy = service.CreateProxy<IUserService>("User");

            var output = await userProxy.GetAllUsers();

            return new JsonResult(output);
        }

        public async Task<JsonResult> Add(UserDto dto)
        {
            var service = ServiceLocator.GetService<IServiceProxyFactory>();
            var userProxy = service.CreateProxy<IUserService>("User");
            var output = await userProxy.CreateUser(new IModuleServices.Users.Dto.CreateUserDto
            {
                EmailAddress = dto.EmailAddress,
                Name = dto.Name,
                FullName = dto.FullName,
                Password = dto.Password,
                PhoneNumber = dto.PhoneNumber,
                Surname = dto.Surname,
            });
            return new JsonResult(output);
        }
        
        public async Task<IActionResult> Delete(Int64 id)
        {
            var service = ServiceLocator.GetService<IServiceProxyFactory>();
            var userProxy = service.CreateProxy<IUserService>("User");
            await userProxy.DeleteUser(id);

            return RedirectToAction("User");
        }

        public async Task<JsonResult> GetUser(Int64 id)
        {
            var service = ServiceLocator.GetService<IServiceProxyFactory>();
            var userProxy = service.CreateProxy<IUserService>("User");
            var output= await userProxy.GetUserById(id);

            return new JsonResult(output);
        }
        public async Task<JsonResult> Update(UserDto dto)
        {
            var service = ServiceLocator.GetService<IServiceProxyFactory>();
            var userProxy = service.CreateProxy<IUserService>("User");
            var output = await userProxy.UpdateUser(dto);
            return new JsonResult(output);
        }
    }

}
