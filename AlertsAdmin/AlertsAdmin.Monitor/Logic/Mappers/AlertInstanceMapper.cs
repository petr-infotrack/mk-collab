using System;
using System.Collections.Generic;
using System.Text;
using AlertsAdmin.Domain.Models;
using AlertsAdmin.Elastic.Models;

namespace AlertsAdmin.Monitor.Logic.Mappers
{
    public class AlertInstanceMapper
    {
        public AlertInstance Map(ElasticErrorMessage message, MessageType messageType)
        {
            var model = new AlertInstance()
            {
                ElasticId = message.ElasticId,
                Message = message.Message,
                MessageType = messageType,
                Timestamp = message.Timestamp,
                JsonData = null, //TODO populate critical info from the message
            };

            return model;
        }
    }
}
