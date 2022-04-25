using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSIT133Site.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MSIT133Site.Controllers
{
    public class ApiController : Controller
    {
        private readonly DemoContext _context;
        private readonly IWebHostEnvironment _host;

        public ApiController(DemoContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }
        //public IActionResult Index(string username,string age)
        public IActionResult Index(User user)
        {
            System.Threading.Thread.Sleep(5000);
            if (string.IsNullOrEmpty(user.userName))
            {
                user.userName= "Ajax";
            }

            return Content($"Hello { user.userName},You are {user.age} years old","text/plain",System.Text.Encoding.UTF8);
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
        public IActionResult Register(Member member, IFormFile photo)
        {

            //C:\Users\User\Documents\Ajax\MSIT133Site\wwwroot\uploads
            //_host.ContentRootPath => C:\Users\User\Documents\Ajax\MSIT133Site\
            //_host.WebRootPath => C:\Users\User\Documents\Ajax\MSIT133Site\wwwroot 

            //將檔案儲存到uploads資料夾中
            string uploadFolder = Path.Combine(_host.WebRootPath, "uploads", photo.FileName);
            using (var fileStream = new FileStream(uploadFolder, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }

            //將圖檔轉成二進位 memoryStream
            byte[] imgByte = null;
            using (MemoryStream stream = new MemoryStream())
            {
                photo.CopyTo(stream);
                imgByte = stream.ToArray();
            }

            //寫進資料庫
            member.FileName = photo.FileName;
            member.FileData = imgByte;

            _context.Members.Add(member);
            _context.SaveChanges();


            //string info = $"{_host.WebRootPath} - ${_host.ContentRootPath}";
            string info = $"{photo.FileName} - {photo.Length} - {photo.ContentType}";
            //string info = uploadFolder;
            return Content(info, "text/plain", System.Text.Encoding.UTF8);
            //return Content($"Hello {user.userName}, You are {user.age} years old. 電子郵件 {user.email}", "text/plain", System.Text.Encoding.UTF8);
        }

        //讀出城市名稱
        public IActionResult City()
        {
            var cities = _context.Addresses.Select(a => new { a.City }).Distinct().OrderBy(a => a.City);

            return Json(cities);
        }

        //讀出鄉鎮區
        public IActionResult Districts(string city)
        {
            var districts = _context.Addresses.Where(a => a.City == city).Select(a => new { a.SiteId }).Distinct().OrderBy(a => a.SiteId);

            return Json(districts);
        }

        //讀出路名
        public IActionResult roads(string district)
        {
            var roads = _context.Addresses.Where(a => a.SiteId == district).Select(a => new { a.Road }).Distinct().OrderBy(a => a.Road);

            return Json(roads);
        }
        public IActionResult GetImageByte(int id = 1)
        {
            Member member = _context.Members.Find(id);
            byte[] img = member.FileData;
            return File(img, "image/jpeg");
        }
    }
}
