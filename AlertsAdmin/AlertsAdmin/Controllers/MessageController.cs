using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Domain.Models;
using AlertsAdmin.Models;
using AlertsAdmin.Service.Search;
using Microsoft.AspNetCore.Mvc;

namespace AlertsAdmin.Controllers
{
    public class MessageController : Controller
    {
        private readonly IAlertRepository _alertRepo;
        private readonly IMessageSearch _messageSearch;

        public MessageController(IAlertRepository alertRepository, IMessageSearch messageSearch)
        {
            _alertRepo = alertRepository;
            _messageSearch = messageSearch;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var messages = await _alertRepo.GetAllAlertsAsync();

            return View(new MessageViewModel { Messages = messages });
        }

        [HttpPost]
        public async Task<IActionResult> Search(MessageSearchOptions options)
        {
            var messages = await _messageSearch.Search(options);
            return View("Index",new MessageViewModel { Messages = messages, options = options});
        }

    }
}
