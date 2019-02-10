﻿using Drugstore.Models;
using Drugstore.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            if (result.Success)
            {
                Dictionary<string, string> output = result.Results.ToDictionary(item => item.Key, item => item.Value.ToString());
                return RedirectToAction(nameof(Succes), output);
            }
            return RedirectToAction(nameof(Failed), new { message = result.Error } );              
        }

        [HttpGet]
        public IActionResult Failed(string message)
        {
            return View(model: message);
        }

        [HttpGet]
        public IActionResult Succes(Dictionary<string, string> result)
        {
            return View(result);
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
