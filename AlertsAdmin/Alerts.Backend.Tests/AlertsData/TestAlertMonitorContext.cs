using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertsAdmin.Data.Contexts;
using AlertsAdmin.Elastic.Models;
using AlertsAdmin.Monitor.Logic;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Alerts.Backend.Tests.AlertsData
{
    public class TestAlertMonitorContext
    {

        private AlertMonitoringContext GetContext()
        {
            var connStr = "Data Source=auawsrpt001l.Infotrack.com.au;Initial Catalog=AlertMonitoring;Integrated Security=SSPI;";

            var builder = new DbContextOptionsBuilder<AlertMonitoringContext>()
                .UseSqlServer(connStr);


           return  new AlertMonitoringContext(builder.Options);

        }


        [Fact]
        public void TestEntityAccessForPopulatedRepository()
        {
            var db = GetContext();

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

            var messageTypeQuery = db.MessageTypes.Include(x => x.Alert).Where(x => x.Id < 100).ToList();

            var alertQuery = db.Alerts.Where(x => x.Id   <100).ToList();


        }

    }
}
