# SurgingDemo
base surging

本示例项目是基于 Surging，所有的示例以及解决方案都是根据自己的理解而成的，并不代表 surging 作者的想法。
使用步骤如下：
1. git clone surging；

2. 在和 surging clone 相同的目录下git clone SurgingDemo，因为本示例项目没有从nuget 引用，直接 surging 项目引用，没有拷贝一份放在自己的解决方案，
一是和 surging 保持最新代码，二是方便学习surging和调试，毕竟你想使用surging、理解surging才是踏出第一步；

3. ApiGateway 使用 surging 的例子，当然正式开发建议自己重写 ApiGateway

4. 服务管理使用 consul，因为调试简单，只需 consul agent -dev 即可开启consul

5. 在 windows 中启动：
发布网关 1. ApiGateway     dotnet run Surging.ApiGateway
启用服务 2. Server    dotnet Bill.Demo.Services.Server.dll
发布客户端（本示例使用 web mvc） 3. Bill.Demo.Web  dotnet run Bill.Demo.Web
