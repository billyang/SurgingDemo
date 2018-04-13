# SurgingDemo
base surging

### 前言：
### Surging 分布式微服务框架适合做什么
1. 企业级互联网架构平台；
2. 传统大型项目，伸缩性很强的项目，可应对突发的流量暴增（比如双十一订单暴增）；
3. 移动互联网项目，比如为了因对突发的因营销等带来的流量暴增，评论暴增等等情况；

欢迎补充...


本示例项目是基于 Surging https://github.com/dotnetcore/surging 

### 旨在描述如何在 surging 的基础上运行dapper完成一个增删改的例子

[Surging缓存拦截](https://github.com/billyang/SurgingDemo/wiki/Surging-%E7%BC%93%E5%AD%98%E6%8B%A6%E6%88%AA%E9%85%8D%E7%BD%AE%E7%A4%BA%E4%BE%8B)

1.在sqlserver中建立Test 数据库
----
运行下面脚本，生成user表

[test.db](https://github.com/billyang/SurgingDemo/blob/master/src/sql/surgingdemo.sql)


2.运行Surging Demo
----
clone代码 `git clone https://github.com/billyang/SurgingDemo.git`

#### 因为本示例项目没有从`nuget` 引用，直接从 surging 项目引用，没有拷贝一份放在自己的解决方案，
#### 假设目录结构如下：
D:\git\surging<br/>
D:\git\SurgingDemo
#### 最终保持`Surging`和`SurgingDemo`在同一个目录

这样做的好处： 
* 是和 surging 保持最新代码
* 是方便学习surging和调试，毕竟你想使用surging、理解surging才是踏出第一步

![](https://github.com/billyang/SurgingDemo/blob/master/docs/SurgingDemo.png?raw=true)

Surging.ApiGateway 提供了服务管理以及网关统一访问入口。 目前开发还不完善，如果现在要用于正式开发建议自己要部分重写 ApiGateway，加入权限验证。相信等到1.0版本作者也会把数据监控、流量控制、数据安全、分流控制、身份认证等管理功能加入，当然这些功能并不会影响正常使用。

本示例服务注册中心使用 consul，因为调试简单，只需 consul agent -dev 即可开启consul

##### 在 windows 中启动：<br/>
##### 发布网关 1. ApiGateway     dotnet run Surging.ApiGateway<br/>
##### 启用服务 2. Server    dotnet Bill.Demo.Services.Server.dll<br/>
##### 发布客户端（本示例使用 web mvc） 3. Bill.Demo.Web  dotnet run Bill.Demo.Web<br/>

假设你已经把SurgingDemo已运行起来了，即可对根据Dapper对User进行增删改查
![dapper](https://github.com/billyang/SurgingDemo/blob/master/docs/dapperCURD.png)


介绍一下本示例各项目的职责
-----

## Bill.Demo.Core 用户定义数据模型

## Bill.Demo.DapperCore （Dapper仓储,其中仓储需继承 UserRepository: Surging.Core.CPlatform.Ioc.BaseRepository）
```C#
public class UserRepository: BaseRepository, IBaseRepository<User>
    {
        /// <summary>
        /// 创建一个用户
        /// </summary>
        /// <param name="entity">用户</param>
        /// <param name="connectionString">链接字符串</param>
        /// <returns></returns>
        public Task<Boolean> CreateEntity(User entity, String connectionString = null)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection(connectionString))
            {
                string insertSql = @"INSERT  INTO dbo.auth_User
                                    ( TenantId ,
                                      Name ,
                                      Password ,
                                      SecurityStamp ,
                                      FullName ,
                                      Surname ,
                                      PhoneNumber ,
                                      IsPhoneNumberConfirmed ,
                                      EmailAddress ,
                                      IsEmailConfirmed ,
                                      EmailConfirmationCode ,
                                      IsActive ,
                                      PasswordResetCode ,
                                      LastLoginTime ,
                                      IsLockoutEnabled ,
                                      AccessFailedCount ,
                                      LockoutEndDateUtc
                                    )
                            VALUES  ( @tenantid ,
                                      @name ,
                                      @password ,
                                      @securitystamp ,
                                      @fullname ,
                                      @surname ,
                                      @phonenumber ,
                                      @isphonenumberconfirmed ,
                                      @emailaddress ,
                                      @isemailconfirmed ,
                                      @emailconfirmationcode ,
                                      @isactive ,
                                      @passwordresetcode ,
                                      @lastlogintime ,
                                      @islockoutenabled ,
                                      @accessfailedcount ,
                                      @lockoutenddateutc
                                    );";
                return Task.FromResult<Boolean>(conn.Execute(insertSql, entity) > 0);
            }
        }
   }
```
## Bill.Demo.IModuleServices （和Surging项目一样，定义模块服务接口以及领域模型）
```C#
       Task<UserDto> GetUserById(Int64 id);
        
        Task<Boolean> UpdateUser(UserDto user);

        Task<Boolean> DeleteUser(Int64 userId);

```

## Bill.Demo.ModuleServices （和Surging项目一样，实现模块服务）
如：
```C#
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
```

## Bill.Demo.Services.Server 服务


## Bill.Demo.Web 客户端
```C#
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

```
