using AlertsAdmin.Domain.Enums;
using AlertsAdmin.Domain.Models;
using AlertsAdmin.Elastic.Models;
using System;

namespace AlertsAdmin.Monitor.Logic.Mappers
{
    public class MessageTypeMapper
    {
        public MessageType Map(ElasticErrorMessage message)
        {
            var model = new MessageType()
            {
                Template = message.MessageTemplate,

                Level = AlertLevel.Error,
                DefaultStatus = AlertStatus.Active,
                ExpiryCount = 0,
                ExpiryTime = new TimeSpan(),
                Notification = AlertNotification.Monitor,
                Priority = AlertPriority.High
            };

            return model;
        }
    }
}