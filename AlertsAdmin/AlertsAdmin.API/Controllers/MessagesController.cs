using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlertsAdmin.API.Controllers
{
    [Route("api/v1/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageSearch _messageSearch;

        public MessagesController(IMessageRepository messageRepository, IMessageSearch messageSearch)
        {
            _messageRepository = messageRepository;
            _messageSearch = messageSearch;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var messages = await _messageRepository.GetAllMessagesAsync();
            return Json(messages);
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> Index(int Id)
        {
            var message = await _messageRepository.GetMessageByIdAsync(Id);
            return Json(message);
        }

        [HttpPost]
        public async Task<IActionResult> Search(MessageSearchOptions options)
        {
            var messages = await _messageSearch.Search(options);
            return Json(messages);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MessageType message)
        {
            await _messageRepository.UpdateMessageAsync(message);
            return Redirect("Index");
        }

    }
}
