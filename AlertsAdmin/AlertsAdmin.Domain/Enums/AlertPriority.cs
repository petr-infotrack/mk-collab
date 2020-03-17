using AlertsAdmin.Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlertsAdmin.Domain.Enums
{
    public enum AlertPriority
    {
        [DisplayClass("bg-danger")]
        Critical,
        [DisplayClass("bg-warning")]
        High,
        [DisplayClass("bg-info")]
        Low
    }
}
