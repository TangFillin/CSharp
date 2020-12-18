using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRunApp.Models;

namespace WebRunApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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
        [HttpGet]
        public IActionResult GetData()
        {
            StringBuilder builder = new StringBuilder();
            using(Stream stream = new FileStream("", FileMode.Open))
            {
                using(StreamReader reader = new StreamReader(@"C:\Users\fillin\Desktop\圣墟.txt"))
                {
                    builder.AppendLine(reader.ReadLine());
                }
            }
            return Json(new { txt = builder });
        }
    }
}
