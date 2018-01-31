using Bill.Demo.IModuleServices.Users.Dto;
using Bill.Demo.IModuleServices.Users.Models;
using Surging.Core.Caching;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.CPlatform.Support;
using Surging.Core.CPlatform.Support.Attributes;
using Surging.Core.System.Intercept;
using System.Threading.Tasks;

namespace Bill.Demo.IModuleServices.Users
{
    [ServiceBundle("api/{Service}")]
    public interface IUserService : IServiceKey
    {
        Task<UserDto> Authentication(AuthenticationRequestData requestData);

        [Service(Date = "2018-1-2", Director = "Bill", Name = "获取用户")]
        Task<string> GetUserName(int id);


        [Service(Date = "2017-8-11", Director = "Bill", Name = "根据id查找用户是否存在")]
        Task<bool> Exists(int id);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<IdentityUser> Save(IdentityUser requestData);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [Service(Date = "2017-8-11", Director = "fanly", Name = "获取用户")]
        Task<int> GetUserId(string userName);


        [Command(Strategy = StrategyType.Injection, Injection = @"return
new Bill.Demo.IModuleServices.Users.Dto.UserDto
         {
            Name=""fanly"",
            FullName=""奥巴马""
         };", RequestCacheEnabled = false, InjectionNamespaces = new string[] { "Bill.WMS.IModuleServices.Users" })]
        [Service(Date = "2017-8-11", Director = "fanly", Name = "获取用户")]
        [InterceptMethod(CachingMethod.Get, Key = "GetUser_id_{0}", CacheSectionType = SectionType.ddlCache, Mode = CacheTargetType.Redis, Time = 480)]
        Task<UserDto> GetUser(UserDto user);
                
        [Service(Date = "2017-8-11", Director = "fanly", Name = "获取用户")]
        [Command(Strategy = StrategyType.Injection, ExecutionTimeoutInMilliseconds = 1500, BreakerRequestVolumeThreshold = 3, Injection = @"return false;", RequestCacheEnabled = false)]
        [InterceptMethod(CachingMethod.Get, Key = "GetDictionary", CacheSectionType = SectionType.ddlCache, Mode = CacheTargetType.Redis, Time = 480)]
        Task<bool> GetDictionary();

    }
}
