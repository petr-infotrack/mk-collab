using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlertsAdmin.Controllers
{
    public class AlertController : Controller
    {
        private static IAlertRepository _alertRepo;

        public AlertController(IAlertRepository alertRepository)
        {
            _alertRepo = alertRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var alerts = await _alertRepo.GetAllAlertsAsync();

            return View(new AlertViewModel { Alerts = alerts});
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchString)
        {
            var alerts = await _alertRepo.FindAlertsByMessage(searchString);
            return View("Index",new AlertViewModel { Alerts = alerts });
        }

    }
}
