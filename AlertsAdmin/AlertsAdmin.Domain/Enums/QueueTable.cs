using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using AlertsAdmin.Domain.Attributes;

namespace AlertsAdmin.Domain.Enums
{
    [QueueTable]
    public enum LdmQueueTable
    {
        [QueueName("Order Updates")]
        [Query(@"SELECT COUNT(*) FROM LdmCore.dbo.OrderUpdates WITH (NOLOCK) WHERE Identifier NOT LIKE '%WcfReceiver_OfficeExpressIntegrationService%'")]
        OrderUpdates,
        [QueueName("OfficeExpress Order Updates")]
        [Query(@"SELECT COUNT(*) FROM LdmCore.dbo.OrderUpdates WITH (NOLOCK) WHERE Identifier LIKE '%WcfReceiver_OfficeExpressIntegrationService%'")]
        OfficeExpressOrderUpdates,
        [QueueName("Order Execution Tasks")]
        OrderExecutionTasks,
        [QueueName("Order Update Responses")]
        OrderUpdateResponseQueue,
        [QueueName("Order Update Requests")]
        OrderUpdateRequestQueue,
        [QueueName("FileTrack Logins")]
        FileTrackLoginUpdates,
        [QueueName("FileTrack Orders")]
        FileTrackOrderUpdates
    }
}
