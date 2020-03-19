using System;
using System.Collections.Generic;
using System.Text;
using AlertsAdmin.Domain.Enums;
using AlertsAdmin.Domain.Models;
using AlertsAdmin.Elastic.Models;

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
