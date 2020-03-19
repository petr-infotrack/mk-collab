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
using Microsoft.AspNetCore.Routing;

namespace AlertsAdmin.Controllers.API
{
    [Route("api/v1/[controller]")]
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
        //[Route(ROUTE_BASE)]
        public async Task<IActionResult> Alerts()
        {
            var alerts = await _alertRepository.GetActiveAlertsAsync();
            var payload = ConvertAlerts(alerts.OrderBy(a => a.Priority));
            return Json(payload);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Alerts(int id)
        {
            var alertInstances = await _alertInstanceRepository.GetAlertInstancesAsync(id);
            var payload = ConvertAlertInstances(alertInstances);
            return Json(payload);
        }

        [HttpGet]
        [Route("View/{id}")]
        public async Task<IActionResult> Alert(int id)
        {
            var alert = await _alertRepository.GetAlertAsync(id);
            if(alert != null)
                return Json(alert);
            _logger.LogWarning($"Could not find alert with Id: {id}");
            return NotFound();
        }

        [HttpPost]
        [Route("acknowledge")]
        public async Task<IActionResult> Acknowledge(AlertAcknowledgeRequest request)
        {
            if (string.IsNullOrEmpty(request.Message) || (request.AckCount == null && request.AckTime == null))
                return BadRequest();
            await _alertRepository.AcknowledgeAlert(request);
            return RedirectToAction("Index", "Home");
        }

        private IEnumerable<object> ConvertAlertInstances(IEnumerable<AlertInstance> alertInstances)
        {
            return alertInstances.OrderByDescending(a => a.Timestamp).Select(a =>
                new
                {
                    a.ElasticId,
                    Timestamp = a.Timestamp.ToShortTimeString(),
                    a.Message
                }
            );
        }

        private IEnumerable<object> ConvertAlerts(IEnumerable<Alert> alerts)
        {
            return alerts.Select(a =>
                new
                {
                    a.Id,
                    Title = $"{a.TimeStamp.ToShortTimeString()} - ({a.InstanceCount} Occurances)",
                    Class = a.Priority.TryGetClass(out var @class) ? @class : "bg-warning",
                    Message = a.MessageType.Template
                }
            );
        }
    }
}
 