using System;
using System.Collections.Generic;
using System.Text;
using AlertsAdmin.Elastic.Models;

namespace AlertsAdmin.Monitor.Logic
{
    public interface IDataProcessor<in TRecord>
    {
        void Process(TRecord message);
    }

    public class ErrorMessageProcessor : IDataProcessor<ElasticErrorMessage>
    {


        public ErrorMessageProcessor()
        {

        }

        public void Process(ElasticErrorMessage message)
        {

        }
    }
}
