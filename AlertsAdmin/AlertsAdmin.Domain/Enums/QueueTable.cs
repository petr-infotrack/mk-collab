using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AlertsAdmin.Domain.Enums
{
    public enum QueueTable
    {
        OrderExecutionTasks,
        OrderUpdateResponseQueue,
        OrderUpdateRequestQueue,
        FileTrackLoginUpdates,
        FileTrackOrderUpdates,
    }
}
