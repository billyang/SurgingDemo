using Bill.Demo.Core.Authorization.Users;
using Bill.Demo.DapperCore.Repositories.Users;
using Bill.Demo.IModuleServices.Users;
using Bill.Demo.IModuleServices.Users.Dto;
using Bill.Demo.IModuleServices.Users.Models;
using Surging.Core.CPlatform.EventBus.Events;
using Surging.Core.CPlatform.EventBus.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Bill.WMS.ModuleServices
{
    [ModuleName("User")]
    public class UserService : ProxyServiceBase, IUserService
    {
        private readonly UserRepository _repository;
        private readonly IEventBus _eventBus;
        public UserService(UserRepository repository, IEventBus eventBus)
        {
            this._repository = repository;
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
                FullName = "奥巴马"
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

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            Publish(evt);
            
            await Task.CompletedTask;
        }
        
        public async Task<Boolean> CreateUser(CreateUserDto user) {
            return await _repository.CreateEntity(new User()
            {
                AccessFailedCount = 0,
                EmailAddress = user.EmailAddress,
                IsEmailConfirmed = false,
                EmailConfirmationCode = "",
                IsActive = false,
                IsLockoutEnabled = false,
                IsPhoneNumberConfirmed = false,
                IsTwoFactorEnabled = false,
                LastLoginTime = DateTime.Now,
                Name = user.Name,
                Password = user.Password,
                PasswordResetCode = "",
                PhoneNumber = user.PhoneNumber,
                SecurityStamp = "Bill",
                Surname = user.Surname,
                FullName = user.FullName,
                TenantId = user.TenantId
            });
        }

        public async Task<IList<UserDto>> GetAllUsers()
        {
            var users = await _repository.GetAllEntity();
            IList<UserDto> output = new List<UserDto>();
            users.ToList().ForEach(user => output.Add(new UserDto()
            {
                Id = user.Id,
                EmailAddress = user.EmailAddress,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Surname = user.Surname,
                TenantId = user.TenantId,
                FullName = user.FullName,
            })
            );
            return output;
        }

        public async Task<UserDto> GetUserById(Int64 id)
        {
            var user = await _repository.GetEntityById(id);
            return new UserDto()
            {
                Id = user.Id,
                EmailAddress = user.EmailAddress,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Surname = user.Surname,
                TenantId = user.TenantId,
                FullName = user.FullName,
            };
        }

        public async Task<Boolean> UpdateUser(UserDto user)
        {
            var entity = await _repository.GetEntityById(user.Id);
            entity.Name = user.Name;
            entity.Password = user.Password;
            entity.FullName = user.FullName;
            entity.Surname = user.Surname;
            entity.EmailAddress = user.EmailAddress;
            entity.PhoneNumber = user.PhoneNumber;

            return await _repository.Update(entity);

        }

        public async Task<Boolean> DeleteUser(Int64 userId)
        {
            return await _repository.Delete(userId);
        }

    }
}
