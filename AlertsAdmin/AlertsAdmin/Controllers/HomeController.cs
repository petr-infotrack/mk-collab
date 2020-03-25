using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AlertsAdmin.Models;
using AlertsAdmin.Interfaces;

namespace AlertsAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAPIHelper _apiHelper;

        public HomeController(ILogger<HomeController> logger, IAPIHelper apiHelper)
        {
            _logger = logger;
            _apiHelper = apiHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Messages()
        {
            return View("Messages");
        }

        [HttpGet]
        [Route("/alerts/view/{id}")]
        public async Task<IActionResult> AlertViewAsync(int id)
        {
            var alert = await _apiHelper.GetAlertAsync(id);
            if (alert != null)
                return View("AlertsView",alert);
            _logger.LogWarning($"Could not find alert with Id: {id}");
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
