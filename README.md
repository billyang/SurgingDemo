# SurgingDemo
base surging

本示例项目是基于 Surging https://github.com/dotnetcore/surging ，所有的示例以及解决方案都是根据自己的理解而成的，并不代表 surging 作者的想法。

### 旨在描述如何在 surging 的基础上运行dapper完成一个增删改的例子。

1.在sqlserver中建立Test 数据库
====
运行下面脚本，生成user表

[test.db](https://github.com/billyang/SurgingDemo/blob/master/src/sql/surgingdemo.sql)


2.接下来运行Surging Demo
====

在 https://github.com/dotnetcore/surging 中 git clone；

# 在和 surging clone 相同的目录下git clone SurgingDemo，因为本示例项目没有从nuget 引用，直接 surging 项目引用，没有拷贝一份放在自己的解决方案， 一是和 surging 保持最新代码，二是方便学习surging和调试，毕竟你想使用surging、理解surging才是踏出第一步；
![](https://github.com/billyang/SurgingDemo/blob/master/docs/SurgingDemo.png?raw=true)

ApiGateway 使用 surging 的例子，当然正式开发建议自己重写 ApiGateway

服务管理使用 consul，因为调试简单，只需 consul agent -dev 即可开启consul

在 windows 中启动：<br/>
发布网关 1. ApiGateway     dotnet run Surging.ApiGateway<br/>
启用服务 2. Server    dotnet Bill.Demo.Services.Server.dll<br/>
发布客户端（本示例使用 web mvc） 3. Bill.Demo.Web  dotnet run Bill.Demo.Web<br/>

假设你已经把SurgingDemo已运行起来了，即可对根据Dapper对User进行增删改查
![dapper](https://github.com/billyang/SurgingDemo/blob/master/docs/dapperCURD.png)


介绍一下本示例各项目的职责，
====
Bill.Demo.Core 用户定义数据模型
-------

Bill.Demo.DapperCore （Dapper仓储
其中仓储需继承 UserRepository: Surging.Core.CPlatform.Ioc.BaseRepository）
-------

Bill.Demo.IModuleServices （和Surging项目一样，定义模块服务接口以及领域模型）
-------

       Task<UserDto> GetUserById(Int64 id);
        
        Task<Boolean> UpdateUser(UserDto user);

        Task<Boolean> DeleteUser(Int64 userId);



Bill.Demo.ModuleServices （和Surging项目一样，实现模块服务）
-------
如：


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


Bill.Demo.Web 客户端
-------

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

