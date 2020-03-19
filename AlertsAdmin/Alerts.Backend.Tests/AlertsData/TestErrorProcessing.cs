using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using AlertsAdmin.Data.Contexts;
using AlertsAdmin.Elastic;
using AlertsAdmin.Elastic.Models;
using AlertsAdmin.Monitor.Logic;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Alerts.Backend.Tests.AlertsData
{
    public class TestErrorProcessing
    {

        // Set-up routine
        private AlertMonitoringRepository GetRepository()
        {
            var connStr = "Data Source=auawsrpt001l.Infotrack.com.au;Initial Catalog=AlertMonitoring;Integrated Security=SSPI;";
            var connStrTest = "Data Source=auawsrpt001l.Infotrack.com.au;Initial Catalog=AlertMonitoringTest;Integrated Security=SSPI;";

            var builder = new DbContextOptionsBuilder<AlertMonitoringContext>()
                .UseSqlServer(connStr);


            Func<AlertMonitoringContext> factory = () => new AlertMonitoringContext(builder.Options);

            return new AlertMonitoringRepository(factory);
        }

        [Fact]
        public void TestSaveErrorMessage()
        {
            var repo = GetRepository();

            var message = new ElasticErrorMessage()
            {
                MessageTemplate = "Message {msg} from system {sys}",
                Level = "Error",
                ElasticId = "ELK_ID_00001",
                Message = "Health check {HealthCheckName} completed after {ElapsedMilliseconds}ms with status {HealthStatus} and '{HealthCheckDescription}'",
                Fields = new ElasticErrorFields()
                {
                    Application = "Test",
                    Environment = "Prod",
                    MachineName = "machine1",
                    OrderIdInt = 123,
                    OrderIdStr = null,
                    Region = "this is region"

                }
            };


            repo.AddMessageAsync(message).Wait();

           // var result = repo.SaveAsync().Result;

        }


        [Fact]
        public void TestSaveErrorCollection()
        {

            // Get elastic messages
            var el = new ElasticDataRepository();

            var errors = el.GetElasticErrorMessages(DateTime.Now, 5, 5);

            Assert.True(errors.Count > 0);


            // Get save first 100 into alert monitoring repository
            var repo = GetRepository();

            var cnt = 0;

            foreach(var  error in errors)
            {
                if (cnt >= 1000)
                {
                    break;
                }

                repo.AddMessageAsync(error).Wait();

                //var result = repo.SaveAsync().Result;

                cnt++;
            }


        }
    }
}
