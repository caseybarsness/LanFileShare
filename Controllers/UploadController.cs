using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Identity;
using WebApplication.Models;




namespace WebApplication.Controllers
{
    [Authorize]
    public class UploadController : Controller
    {
    private ApplicationDbContext db { get; }
    private IHostingEnvironment env;
    private readonly UserManager<ApplicationUser> UserManager;
    public UploadController(ApplicationDbContext dataContext, IHostingEnvironment environment, UserManager<ApplicationUser> userManager){
        db = dataContext;
        env = environment;
        UserManager = userManager;
    }
        public IActionResult Index()
        {
           
            return View();
        }

    [HttpPost]
    public async Task<IActionResult> Index(ICollection<IFormFile> files)
    {
        var uploads = Path.Combine(env.WebRootPath, "uploads");
        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                var fileName = file.FileName.Split('.')[0] + 
                DateTime.Now.ToString("dd_MM_yyyy_mm_ss") + "." + 
                file.FileName.Split('.')[1];
                
                using (var fileStream = new FileStream(Path.Combine(uploads,fileName), FileMode.Create))
                {
                    var remoteIpAddress = this.Request.HttpContext.Connection.RemoteIpAddress.ToString();                    
                    var user = await UserManager.GetUserAsync(HttpContext.User);
                    var shrFile = new ShareFile();
                    shrFile.CreatedDate = DateTime.Now;
                    shrFile.FileName = fileName;
                    shrFile.Person = user;
                    shrFile.PublicIpAddress = remoteIpAddress;
                    var curId = db.Files.OrderByDescending(x => x.ShareFileID).FirstOrDefault();
                    var newid = 0;
                    if(curId == null){
                        newid =1;
                    }
                    else{
                        newid = curId.ShareFileID+1;
                    }
                    shrFile.ShareFileID =  newid;
                    db.Files.Add(shrFile);
                    await db.SaveChangesAsync();
                    await file.CopyToAsync(fileStream);
                }
            }
        }
        return View();
    }

        public IActionResult Error()
        {
            return View();
        }
    }
}
