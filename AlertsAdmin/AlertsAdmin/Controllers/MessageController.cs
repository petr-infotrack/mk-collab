using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlertsAdmin.Controllers
{
    public class MessageController : Controller
    {
        private static IAlertRepository _alertRepo;

        public MessageController(IAlertRepository alertRepository)
        {
            _alertRepo = alertRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var alerts = await _alertRepo.GetAllAlertsAsync();

            return View(new MessageViewModel { Messages = alerts});
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchString)
        {
            var alerts = await _alertRepo.FindAlertsByMessage(searchString);
            return View("Index",new MessageViewModel { Messages = alerts });
        }

    }
}
