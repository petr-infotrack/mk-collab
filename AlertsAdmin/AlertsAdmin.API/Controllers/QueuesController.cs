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
using AlertsAdmin.Domain.Extensions;

namespace AlertsAdmin.API.Controllers
{
    [Route("api/v1/[controller]")]
    public class QueuesController: Controller
    {
        private readonly IQueueRepository _queueRepository;
        private readonly IQueueHistoryService _queueHistoryService;
        private readonly ILogger<QueuesController> _logger;

        public QueuesController(IQueueRepository queueRepository, ILogger<QueuesController> logger, IQueueHistoryService queueHistoryService)
        {
            _queueRepository = queueRepository;
            _logger = logger;
            _queueHistoryService = queueHistoryService;
        }

        [HttpGet]
        [Route("LdmQueues")]
        public async Task<IActionResult> LdmQueues()
        {
            var queueData = await _queueRepository.GetQueueDataAsync<LdmQueueTable>();
            await _queueHistoryService.Process(queueData);
            return Json(queueData);
        }

        [HttpGet]
        [Route("PencilQueues")]
        public async Task<IActionResult> PencilQueues()
        {
            var queueData = await _queueRepository.GetQueueDataAsync<PencilQueueTable>();
            await _queueHistoryService.Process(queueData);
            return Json(queueData);
        }

    }
}
 