using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlertsAdmin.Domain.Interfaces;

namespace AlertsAdmin.Controllers
{
    public class AlertsController : Controller
    {
        private readonly IAlertRepository _alertsRepo;

        public AlertsController(IAlertRepository alertRepo)
        {
            _alertsRepo = alertRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var alerts = await _alertsRepo.GetActiveAlertsAsync();
            return View(alerts);
        }
    }
}
