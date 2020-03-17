using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Models;

namespace AlertsAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAlertRepository _alertRepository;

        public HomeController(ILogger<HomeController> logger, IAlertRepository alertRepository)
        {
            _logger = logger;
            _alertRepository = alertRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/alerts/view/{id}")]
        public async Task<IActionResult> AlertViewAsync(int id)
        {
            //TODO: Don't directly call repository instead call /api/v1/alerts/view/{id} for id. 
            var alert = await _alertRepository.GetAlertAsync(id);
            if (alert != null)
                return View("AlertsView",alert);
            _logger.LogWarning($"Could not find alert with Id: {id}");
            return Redirect("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
