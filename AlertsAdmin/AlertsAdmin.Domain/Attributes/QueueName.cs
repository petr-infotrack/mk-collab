using System;
using System.Collections.Generic;
using System.Text;

namespace AlertsAdmin.Domain.Attributes
{
    public class QueueName : Attribute
    {
        private readonly string _name;

        public QueueName(string Name)
        {
            _name = Name;
        }

        public string Name => _name;
    }
}
