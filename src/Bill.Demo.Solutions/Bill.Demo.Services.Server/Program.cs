using Autofac;
using Surging.Core.Codec.MessagePack;
using Surging.Core.Consul;
using Surging.Core.Consul.Configurations;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.DotNetty;
using Surging.Core.EventBusRabbitMQ;
using Surging.Core.Log4net;
using Surging.Core.ServiceHosting;
using Surging.Core.ServiceHosting.Internal.Implementation;
using System;
using System.Text;

namespace Bill.Demo.Services.Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var host = new ServiceHostBuilder()
                .RegisterServices(builder =>
                {
                    builder.AddMicroService(option =>
                    {
                        option.AddServiceRuntime();
                        //option.UseZooKeeperManager(new ConfigInfo("127.0.0.1:2181"));
                        option.UseConsulManager(new ConfigInfo("127.0.0.1:8500"));
                        option.UseDotNettyTransport();
                        option.UseRabbitMQTransport();
                        option.AddRabbitMQAdapt();
                        //option.UseProtoBufferCodec();
                        option.UseMessagePackCodec();
                        builder.Register(p => new CPlatformContainer(ServiceLocator.Current));
                    });
                })
                .SubscribeAt()
                .UseLog4net("Configs/log4net.config")
                //.UseServer("127.0.0.1", 98)
                //.UseServer("127.0.0.1", 98，“true”) //自动生成Token
                //.UseServer("127.0.0.1", 98，“123456789”) //固定密码Token
                .UseServer(options => {
                    options.Ip = "127.0.0.1";
                    options.Port = 98;
                    options.Token = "True";
                    options.ExecutionTimeoutInMilliseconds = 30000;
                    options.MaxConcurrentRequests = 200;
                    //options.NotRelatedAssemblyFiles = "Centa.Agency.Application.DTO\\w*|StackExchange.Redis\\w*";

                    //options.Ip = "127.0.0.1";
                    //options.Port = 98;
                    //options.ExecutionTimeoutInMilliseconds = 30000;
                    //options.Strategy = StrategyType.Failover; //容错策略使用故障切换
                    //options.RequestCacheEnabled = true; //开启缓存（只有通过接口代理远程调用，才能启用缓存）
                    //options.Injection = "return null"; //注入方式
                    //options.InjectionNamespaces = new string[] { "Bill.WMS.IModuleServices.Users" }; //脚本注入使用的命名空间
                    //options.BreakeErrorThresholdPercentage = 50;  //错误率达到多少开启熔断保护
                    //options.BreakeSleepWindowInMilliseconds = 60000; //熔断多少毫秒后去尝试请求
                    //options.BreakerForceClosed = false;   //是否强制关闭熔断
                    //options.BreakerRequestVolumeThreshold = 20;//10秒钟内至少多少请求失败，熔断器才发挥起作用
                    //options.MaxConcurrentRequests = 100000;//支持最大并发
                })
                .UseStartup<Startup>()
                .Build();

            using (host.Run())
            {
                Console.WriteLine($"服务端启动成功，{DateTime.Now}。");
            }
        }
    }
}