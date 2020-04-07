using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KVReferencesDemo.Models;

namespace KVReferencesDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly string MySecret;

        public HomeController(ILogger<HomeController> logger
            , Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _logger = logger;

            MySecret = configuration["mySecret"];
        }

        public IActionResult Index()
        {
            ViewData.Add("mySecret", MySecret);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
