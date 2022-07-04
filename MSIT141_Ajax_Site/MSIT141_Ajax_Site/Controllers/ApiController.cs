using Microsoft.AspNetCore.Mvc;
using MSIT141_Ajax_Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSIT141_Ajax_Site.Controllers
{
    public class ApiController : Controller
    {
        private readonly DemoContext _context;
        public  ApiController(DemoContext context) {

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
        public IActionResult FirstAjax() 
        {
            return View();
        }
        public IActionResult AjaxPost() {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult CheckName(User user)
        {
            //System.Threading.Thread.Sleep(5000);
            if (user.name == null)
            {
                return Content($"請輸入名字", "text/plain", System.Text.Encoding.UTF8);
            }
            else {
                Member mem= _context.Members.FirstOrDefault(p => p.Name == user.name);
                if (mem == null)
                    return Content($"您的名字尚未註冊，可以使用", "text/plain", System.Text.Encoding.UTF8);
                else
                    return Content($"您的名字已註冊過，請使用其他名字", "text/plain", System.Text.Encoding.UTF8);

            }
        }
    }
}
