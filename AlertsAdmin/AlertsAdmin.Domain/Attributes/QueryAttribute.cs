using System;
using System.Collections.Generic;
using System.Text;

namespace AlertsAdmin.Domain.Attributes
{
    public class QueryAttribute: Attribute
    {
        private readonly string _query;

        public QueryAttribute(string query)
        {
            _query = query;
        }

        public string Query => _query;
    }
}
