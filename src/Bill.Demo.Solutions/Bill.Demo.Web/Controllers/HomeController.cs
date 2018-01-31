using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bill.Demo.Web.Models;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.ProxyGenerator;
using Bill.Demo.IModuleServices.Users;

namespace Bill.Demo.Web.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var service = ServiceLocator.GetService<IServiceProxyFactory>();
            var userProxy = service.CreateProxy<IUserService>("User");

            var userName = await userProxy.GetUserName(1);
            ViewBag.UserName = userName;
            return View();
        }
        public async Task<JsonResult> TestLoop10000()
        {
            var service = ServiceLocator.GetService<IServiceProxyFactory>();
            var userProxy = service.CreateProxy<IUserService>("User");

            //var userName = await userProxy.GetUserName(1);
            //ViewBag.UserName = userName;


            //1w次调用
            var watch = Stopwatch.StartNew();
            for (var i = 0; i < 10000; i++)
            {
                var a = await userProxy.GetDictionary();
                //var result = serviceProxyProvider.Invoke<object>(new Dictionary<string, object>(), "api/user/GetDictionary", "User").Result;
            }
            watch.Stop();

            var watchMessage = $"1w次调用结束，执行时间：{watch.ElapsedMilliseconds}ms";

            return new JsonResult(watchMessage);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
