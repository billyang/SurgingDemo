using Bill.Demo.IModuleServices.Users;
using Bill.Demo.IModuleServices.Users.Dto;
using Bill.Demo.IModuleServices.Users.Models;
using Surging.Core.CPlatform.EventBus.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bill.WMS.ModuleServices
{
    [ModuleName("User")]
    public class UserService : ProxyServiceBase, IUserService
    {
        //private readonly UserRepository _repository;
        private readonly IEventBus _eventBus;
        //public UserService(UserRepository repository, IEventBus eventBus)
        //{
        //    this._repository = repository;
        //    this._eventBus = eventBus;
        //}

        public UserService(IEventBus eventBus)
        {
            //this._repository = repository;
            this._eventBus = eventBus;
        }

        public Task<UserDto> Authentication(AuthenticationRequestData requestData)
        {
            if (requestData.UserName == "admin" && requestData.Password == "admin")
            {
                return Task.FromResult(new UserDto() { Name = "admin" });
            }
            return Task.FromResult<UserDto>(null);
        }

        public Task<string> GetUserName(int id)
        {
            return Task.FromResult($"id:{id} is name fanly.");
        }

        public Task<bool> Exists(int id)
        {
            return Task.FromResult(true);
        }

        public Task<int> GetUserId(string userName)
        {
            return Task.FromResult(1);
        }

        public Task<DateTime> GetUserLastSignInTime(int id)
        {
            return Task.FromResult(new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc));
        }

        public Task<bool> Get(List<UserDto> users)
        {
            return Task.FromResult(true);
        }

        public Task<UserDto> GetUser(UserDto user)
        {
            return Task.FromResult(new UserDto
            {
                Name = "bill",
                FullName = "奥巴马",
                CreationTime = DateTime.UtcNow
            });
        }

        public Task<IdentityUser> Save(IdentityUser requestData)
        {
            return Task.FromResult(requestData);
        }


        public Task<bool> GetDictionary()
        {
            return Task.FromResult<bool>(true);
        }

    }
}
