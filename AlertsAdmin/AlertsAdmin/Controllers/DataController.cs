using AlertsAdmin.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
            var data = await _queueRepository.GetQueueData();
            var jsonData = JsonConvert.SerializeObject(data);
            return Json(jsonData);
        }
    }
}
 