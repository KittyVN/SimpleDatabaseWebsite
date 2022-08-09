using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace BestellserviceWeb.Controllers
{
    public class UploadController : Controller
    {
        private readonly string wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        private IWebHostEnvironment hostEnvironment;

        public UploadController(IWebHostEnvironment Environment)
        {
            hostEnvironment = Environment;
        }

        public IActionResult Index()
        {
            List<string> images = Directory.GetFiles(wwwrootPath, "*.png")
                                                    .Select(Path.GetFileName).ToList();
            return View(images);
        }

        [HttpPost]
        public ActionResult Upload(IFormFile file)
        {
            var fileDic = "Files";
            string filePath = Path.Combine(hostEnvironment.WebRootPath,fileDic);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var fileName = file.FileName;
            filePath = Path.Combine(filePath,fileName);

            var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Index(List<IFormFile> file)
        {
            var supportedTypes = new[] { "png", "svg", "pdf" };
            var fileDic = "Files";
            string path = Path.Combine(hostEnvironment.WebRootPath, fileDic);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (file != null)
            {
                for(int i = 0; i < file.Count(); i++)
                {
                    var fileExt = System.IO.Path.GetExtension(file[i].FileName).Substring(1);
                    if (supportedTypes.Contains(fileExt))
                    {
                        var fileName = file.ElementAt(i).FileName;
                        string filepath = Path.Combine(path, fileName);
                        //var path = Path.Combine(wwwrootPath, DateTime.Now.Ticks.ToString() + Path.GetExtension(formFile[i].FileName));

                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            await file[i].CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        TempData["notice"] = "File Extension Is InValid - Only Upload WORD/PDF/EXCEL/TXT File";
                    }
                }
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
