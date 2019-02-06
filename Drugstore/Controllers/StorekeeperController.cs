using Drugstore.UseCases;
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
        private readonly GetXMLStoreUpdateToUseCase getXMLStore;
        private readonly PostXMLStoreOrderListUseCase postXMLStore;

        public StorekeeperController(
            GetXMLStoreUpdateToUseCase getXMLStore,
            PostXMLStoreOrderListUseCase postXMLStore)
        {
            this.getXMLStore = getXMLStore;
            this.postXMLStore = postXMLStore;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile xmlFile)
        {
            var result = getXMLStore.Execute(xmlFile);
            return result ? RedirectToAction("Index") : (IActionResult)NotFound();
        }

        [HttpGet]
        public FileStreamResult Download()
        {
            var xmlFile = postXMLStore.Execute();
            return File(xmlFile, 
                "application/octet-stream", 
                "store_order_" + 
                DateTime.Now.ToString("yyyy-MM-dddd") + 
                ".xml");

        }
    }
}
