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
    public class QueuesController: Controller
    {
        private const string ROUTE_BASE = "api/v1/queues";


        private readonly IQueueRepository _queueRepository;
        private readonly ILogger<QueuesController> _logger;

        public QueuesController(IQueueRepository queueRepository, ILogger<QueuesController> logger)
        {
            _queueRepository = queueRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route(ROUTE_BASE+"/LdmQueues")]
        public async Task<IActionResult> LdmQueues()
        {
            return Json(await _queueRepository.GetQueueDataAsync<LdmQueueTable>());
        }

        [HttpGet]
        [Route(ROUTE_BASE+"/PencilQueues")]
        public async Task<IActionResult> PencilQueues()
        {
            return Json(await _queueRepository.GetQueueDataAsync<PencilQueueTable>());
        }

    }
}
 