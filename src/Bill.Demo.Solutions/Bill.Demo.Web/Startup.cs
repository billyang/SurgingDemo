using System;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Surging.Core.Caching;
using Surging.Core.Caching.Configurations;
using Surging.Core.Codec.MessagePack;
using Surging.Core.Consul;
using Surging.Core.Consul.Configurations;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Cache;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.DotNetty;
using Surging.Core.EventBusRabbitMQ;
using Surging.Core.EventBusRabbitMQ.Configurations;
using Surging.Core.ProxyGenerator;
using Surging.Core.System.Intercept;

namespace Bill.WMS.Web
{
    public class Startup
    {
        private ContainerBuilder _builder;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddCacheFile("Configs/cacheSettings.json", optional: false)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddEventBusFile($"Configs/eventBusSettings.json", optional: true);
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddLogging();
            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.AddMicroService(option =>
            {
                option.AddClient();
                option.AddCache();
                option.AddClientIntercepted(typeof(CacheProviderInterceptor));
                //option.UseZooKeeperManager(new ConfigInfo("127.0.0.1:2181"));
                option.UseConsulManager(new ConfigInfo("127.0.0.1:8500"));
                option.UseConsulCacheManager(new ConfigInfo("127.0.0.1:8500"));
                option.UseDotNettyTransport();
              
                option.UseRabbitMQTransport();
                //option.UseProtoBufferCodec();
                option.UseMessagePackCodec();
                builder.Register(p => new CPlatformContainer(ServiceLocator.Current));
            });

            _builder = builder;
            ServiceLocator.Current = builder.Build();
            return new AutofacServiceProvider(ServiceLocator.Current);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            var serviceCacheProvider = app.ApplicationServices.GetRequiredService<ICacheNodeProvider>();
            var addressDescriptors = serviceCacheProvider.GetServiceCaches().ToList();
            app.ApplicationServices.GetRequiredService<IServiceCacheManager>().SetCachesAsync(addressDescriptors);
            loggerFactory.AddConsole();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            
        }
        

        #region 私有方法
        /// <summary>
        /// 配置日志服务
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureLogging(IServiceCollection services)
        {
            services.AddLogging();
        }

        private static void ConfigureEventBus(IConfigurationBuilder build)
        {
            build
            .AddEventBusFile("eventBusSettings.json", optional: false);
        }

        /// <summary>
        /// 配置缓存服务
        /// </summary>
        private void ConfigureCache(IConfigurationBuilder build)
        {
            build
              .AddCacheFile("cacheSettings.json", optional: false);
        }

        ///// <summary>
        ///// 测试
        ///// </summary>
        ///// <param name="serviceProxyFactory"></param>
        //public static void Test(IServiceProxyFactory serviceProxyFactory)
        //{
        //    Task.Run(async () =>
        //    {

        //        var userProxy = serviceProxyFactory.CreateProxy<IUserService>("User");
        //        await userProxy.GetUserId("user");
        //        do
        //        {
        //            Console.WriteLine("正在循环 1w次调用 GetUser.....");
        //            //1w次调用
        //            var watch = Stopwatch.StartNew();
        //            for (var i = 0; i < 10000; i++)
        //            {
        //                var a = userProxy.GetDictionary().Result;
        //            }
        //            watch.Stop();
        //            Console.WriteLine($"1w次调用结束，执行时间：{watch.ElapsedMilliseconds}ms");
        //            Console.WriteLine("Press any key to continue, q to exit the loop...");
        //            var key = Console.ReadLine();
        //            if (key.ToLower() == "q")
        //                break;
        //        } while (true);
        //    }).Wait();
        //}

        //public static void TestRabbitMq(IServiceProxyFactory serviceProxyFactory)
        //{
        //    serviceProxyFactory.CreateProxy<IUserService>("User").PublishThroughEventBusAsync(new UserEvent()
        //    {
        //        Age = "18",
        //        Name = "fanly",
        //        UserId = "1"
        //    });
        //    Console.WriteLine("Press any key to exit...");
        //    Console.ReadLine();
        //}

        //public static void TestForRoutePath(IServiceProxyProvider serviceProxyProvider)
        //{
        //    Dictionary<string, object> model = new Dictionary<string, object>();
        //    model.Add("user", JsonConvert.SerializeObject(new
        //    {
        //        Name = "fanly",
        //        Age = 18,
        //        UserId = 1
        //    }));
        //    string path = "api/user/getuser";
        //    string serviceKey = "User";

        //    var userProxy = serviceProxyProvider.Invoke<object>(model, path, serviceKey);
        //    var s = userProxy.Result;
        //    Console.WriteLine("Press any key to exit...");
        //    Console.ReadLine();
        //}
        
        #endregion


    }
}
