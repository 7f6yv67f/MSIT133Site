using Microsoft.AspNetCore.Mvc;
using MSIT133Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSIT133Site.Controllers
{
    public class ApiController : Controller
    {
        //private readonly DemoContext _context;

        //public ApiController(DemoContext context)
        //{
        //    _context = context;
        //}
        public IActionResult Index(string username,string age)
        {
            System.Threading.Thread.Sleep(5000);
            if (string.IsNullOrEmpty(username))
            {
                username = "Ajax";
            }

            return Content($"Hello { username},You are {age} years old","text/plain",System.Text.Encoding.UTF8);
        }
        public IActionResult CheckName(string Name)
        {
            DemoContext db = new DemoContext();
            var c = db.Members.FirstOrDefault(m => m.Name == Name);
            string txt = "";
            if (c != null)
            {
                txt = "名稱已使用過!";
            }
            else
            {
                txt = "可使用";
            }
            return Content(txt, "text/plain");
            //db.Members.Where(m => m.Name == name);
            //_context.Members.Where
        }
    }
}
