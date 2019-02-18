using Drugstore.Models;
using Drugstore.Models.Shared;
using Drugstore.UseCases.ExternalPharmacist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Drugstore.Controllers
{
    [Authorize(Roles = "ExternalPharmacist")]
    public class ExternalPharmacistController : Controller
    {
        private readonly UploadSoldMedicinesListUseCase uploadSoldMedicines;

        public ExternalPharmacistController(UploadSoldMedicinesListUseCase uploadSoldMedicines)
        {
            this.uploadSoldMedicines = uploadSoldMedicines;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile xmlFile)
        {
            var result = uploadSoldMedicines.Execute(xmlFile);

            return result.Succes ?
               RedirectToAction(nameof(Succes),result) :
               RedirectToAction(nameof(Failed), result);
        }

        public IActionResult Succes(ResultViewModel<Dictionary<string, object>> result)
        {
            return View(nameof(Succes), result);
        }

        public IActionResult Failed(ResultViewModel<Dictionary<string, object>> result)
        {
            return View(nameof(Failed), result.Message);
        }
    }
}
