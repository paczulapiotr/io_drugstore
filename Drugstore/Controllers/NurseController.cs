using Drugstore.Identity;
using Drugstore.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Drugstore.Controllers
{
    [Authorize(Roles = "Nurse")]
    public class NurseController : Controller
    {
        private readonly DrugstoreDbContext context;
        private readonly UserManager<SystemUser> userManager;

        public NurseController(DrugstoreDbContext context, UserManager<SystemUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Circiut()
        {
            return Ok();
        }
    }
}