using AlertsAdmin.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AlertsAdmin.Domain.Enums;

namespace AlertsAdmin.Controllers
{
    public class DataController: Controller
    {
        private readonly IQueueRepository _queueRepository;
        public DataController(IQueueRepository queueRepository)
        {
            _queueRepository = queueRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Queues()
        {
            var data = await _queueRepository.GetQueueDataAsync<LdmQueueTable>();
            var jsonData = JsonConvert.SerializeObject(data);
            return Json(jsonData);
        }

        [HttpGet]
        public async Task<IActionResult> PencilQueues()
        {
            var data = await _queueRepository.GetQueueDataAsync<PencilQueueTable>();
            var jsonData = JsonConvert.SerializeObject(data);
            return Json(jsonData);
        }
    }
}
 