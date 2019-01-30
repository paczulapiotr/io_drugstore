using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Drugstore.Controllers
{
    [Authorize(Roles = "Storekeeper")]
    public class StorekeeperController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile xmlFile)
        {

            string [] validExtensions = new [] {
                "text/xml",
                "application/xml"
            };

            if(!validExtensions.Any(e=>e.Contains(xmlFile.ContentType)))
            {
                return NotFound();
            }

            string saveDirectory = Path.Combine(
                Directory.GetCurrentDirectory(),
                "XML", "UPLOAD", "store_update_" +
                DateTime.Now.ToString("yyyy-MM-dd") + ".xml");

            FileInfo file = new FileInfo(saveDirectory);
            using (Stream stream = file.Create())
            {
                xmlFile.CopyToAsync(stream).Wait();
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Download()
        {
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "XML", "DOWNLOAD", "example.xml");
            FileInfo file = new FileInfo(directory);
            if (file.Exists)
            {
                FileStreamResult response = null;
                using (Stream stream = file.OpenRead())
                {
                    response = File(file.OpenRead(), "application/octet-stream", "store_order_" + DateTime.Now.ToString("yyyy-MM-dddd") + ".xml");//FileStreamResult
                }

                return response;
            }
            return NotFound();
        }
    }
}
