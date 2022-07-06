using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSIT141_Ajax_Site.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MSIT141_Ajax_Site.Controllers
{
    public class ApiController : Controller
    {
        private readonly DemoContext _context;
        private readonly IWebHostEnvironment _host;
        public  ApiController(DemoContext context,IWebHostEnvironment hostEnvironment) {
            _host = hostEnvironment;
            _context = context;
        }
        public IActionResult Index(User user)
        {
            //System.Threading.Thread.Sleep(5000);
            if (user.name == null)
            {
                user.name = "Ajax";
            }
            return Content($"{user.name} 你好啊~~^^  你的年齡為{user.age}","text/plain",System.Text.Encoding.UTF8);
        }
      
        //public IActionResult Register()
        //{
        //    return View();
        //}
        public IActionResult Register(Member member, IFormFile file)
        {

            string uploadsFolder = Path.Combine(_host.WebRootPath, "uploads");
            //檔案上傳要有實際路徑
            //C:\Users\Student\Documents\Ajax\MSIT141Site\wwwroot\uploads
            //string path = _host.ContentRootPath; //會取得專案資料夾的實際路徑
            //string path = Path.Combine(_host.WebRootPath, "uploads");
            //string filePath = Path.Combine(uploadsFolder, file.FileName);
            //using (var fileStream=new FileStream(filePath, FileMode.Create))
            //{
            //    file.CopyTo(fileStream);
            //}
            string path = Path.Combine(_host.WebRootPath, "uploads", file.FileName); //會取得專案資料夾下wwwroot的實際路徑
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream); //儲存檔案到uploads資料夾中
            }
            byte[] imgByte = null;
            using (var memoryStream=new MemoryStream()) {
                file.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }
            member.FileData = imgByte;
            member.FileName = file.FileName;
            _context.Members.Add(member);
            _context.SaveChanges();

                string info = $"{file.FileName} - {file.ContentType} - {file.Length}";
            return Content(info, "text/plain", System.Text.Encoding.UTF8);
        }

        public IActionResult CheckAccount(string name)
        {
            var exists = _context.Members.Any(m => m.Name == name);
            return Content(exists.ToString(), "text/plain");
        }
        public IActionResult CheckName(string name)
        {
                var exists= _context.Members.Any(p=>p.Name==name);
            return Content(exists.ToString(), "text/plain");
           }
        
        public IActionResult City()
        {
            var cities = _context.Addresses.Select(c => c.City).Distinct();
            return Json(cities);
        }
        public IActionResult Districts(string city)
        {
            var Districts = _context.Addresses.Where(d=>d.City==city).Select(c=>c.SiteId). Distinct();
            return Json(Districts);
        }
        public IActionResult Roads(string district)
        {
            var roads = _context.Addresses.Where(d => d.SiteId ==district ).Select(c => c.Road).Distinct();
            return Json(roads);
        }
        public IActionResult GetImageBytes(int id=1)
        {
            byte[] imdByte = _context.Members.FirstOrDefault(p => p.MemberId == id).FileData;
            return File(imdByte,"image/jpg");
        }
    }
}
