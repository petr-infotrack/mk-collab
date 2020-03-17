using System;
using System.Collections.Generic;
using System.Text;

namespace AlertsAdmin.Domain.Attributes
{
    public class DisplayClassAttribute : Attribute
    {
        private readonly string _class;

        public DisplayClassAttribute(string DisplayClass)
        {
            _class = DisplayClass;
        }

        public string DisplayClass => _class;
    }
}
