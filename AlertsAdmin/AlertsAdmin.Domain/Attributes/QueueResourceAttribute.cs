using System;
using System.Collections.Generic;
using System.Text;

namespace AlertsAdmin.Domain.Attributes
{
    public class QueueResourceAttribute : Attribute
    {
        private readonly string _resource;

        public QueueResourceAttribute(string Resource)
        {
            _resource = Resource;
        }

        public string Resource => _resource;
    }
}
