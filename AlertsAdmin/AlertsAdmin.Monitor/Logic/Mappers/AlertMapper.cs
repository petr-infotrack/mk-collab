using AlertsAdmin.Domain.Models;
using System;

namespace AlertsAdmin.Monitor.Logic.Mappers
{
    public class AlertMapper
    {
        public Alert Map(MessageType messageType, int firstId, int lastId)
        {
            var model = new Alert()
            {
                Status = messageType.DefaultStatus,
                StatusMessage = null,
                TimeStamp = DateTime.Now,
                MessageTypeId = messageType.Id,
                FirstInstanceId = firstId,
                LastInstanceId = lastId,
            };

            return model;
        }
    }
}