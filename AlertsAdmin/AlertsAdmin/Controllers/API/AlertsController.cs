using AlertsAdmin.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AlertsAdmin.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Reflection;
using AlertsAdmin.Domain.Models;
using AlertsAdmin.Models;
using AlertsAdmin.Domain.Extensions;

namespace AlertsAdmin.Controllers.API
{
    public class AlertsController: Controller
    {
        private const string ROUTE_BASE = "api/v1/alerts";

        private readonly IAlertRepository _alertRepository;
        private readonly IAlertInstanceRepository _alertInstanceRepository;
        private readonly ILogger<AlertsController> _logger;

        public AlertsController(IAlertRepository alertRepository, IAlertInstanceRepository alertInstanceRepository, ILogger<AlertsController> logger)
        {
            _alertRepository = alertRepository;
            _alertInstanceRepository = alertInstanceRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route(ROUTE_BASE)]
        public async Task<IActionResult> Alerts()
        {
            var alerts = await _alertRepository.GetActiveAlertsAsync();
            return Json(ConvertAlerts(alerts));
        }

        [HttpGet]
        [Route(ROUTE_BASE+"/{id}")]
        public async Task<IActionResult> Alerts(int id)
        {
            var alertInstances = await _alertInstanceRepository.GetAlertInstancesAsync(id);
            return Json(alertInstances);
        }

        [HttpGet]
        [Route(ROUTE_BASE + "/View/{id}")]
        public async Task<IActionResult> Alert(int id)
        {
            var alert = await _alertRepository.GetAlertAsync(id);
            if(alert != null)
                return Json(alert);
            _logger.LogWarning($"Could not find alert with Id: {id}");
            return NotFound();
        }

        private IEnumerable<AlertResponse> ConvertAlerts(IEnumerable<Alert> alerts)
        {
            var response = alerts.Select(a =>
                new AlertResponse
                {
                    Id = a.Id,
                    Title = $"{a.LastOccuranceString} - ({a.Count} Occurances)",
                    Class = a.AlertPriority.TryGetClass(out var @class) ? @class : "bg-warning",
                    Message = a.Template
                }
            );
            return response;
        }
    }
}
 