using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Domain.Models;
using AlertsAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AlertsAdmin.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IMessageSearch _messageSearch;

        public MessagesController(IMessageRepository messageRepo, IMessageSearch messageSearch)
        {
            _messageRepo = messageRepo;
            _messageSearch = messageSearch;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var messages = await _messageRepo.GetAllMessagesAsync();

            return View(new MessageViewModel { Messages = messages });
        }

        [HttpPost]
        public async Task<IActionResult> Search(MessageSearchOptions options)
        {
            var messages = await _messageSearch.Search(options);
            return View("Index",new MessageViewModel { Messages = messages, options = options});
        }

        [HttpGet]
        public async Task<IActionResult> Search(int Id)
        {
            var message = await _messageRepo.GetMessageByIdAsync(Id);
            var messages = new List<MessageType>() { message };
            return View("Index", new MessageViewModel { Messages = messages, options = null });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var message = await _messageRepo.GetMessageByIdAsync(Id);
            if (message == null)
                return NotFound();
            var messageJSON = JsonConvert.SerializeObject(message);
            return Json(messageJSON);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MessageType message)
        {
            await _messageRepo.UpdateMessageAsync(message);
            return Redirect("Index");
        }
    }
}
