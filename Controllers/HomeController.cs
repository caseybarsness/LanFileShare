using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Data;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
    private ApplicationDbContext db { get; }

    public HomeController(ApplicationDbContext dataContext){
        db = dataContext;
    }
        public IActionResult Index()
        {
            var remoteIpAddress = this.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            ViewBag.PublicIp = remoteIpAddress;
            ViewBag.AvalibleFiles = db.Files.Where(x => x.PublicIpAddress.ToLower() == remoteIpAddress.ToLower()).ToList();

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
