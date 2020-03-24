using System;
using AlertsAdmin.Elastic;
using Xunit;

namespace Alerts.Backend.Tests.ElasticFeed
{
    public class ElasticFeedTests
    {
        [Fact]
        public void ReadErrorsFromElastic()
        {
            var el = new ElasticDataRepository();

            var errors = el.GetErrorMessages(DateTime.Now, 10, 10);

            Assert.True(errors.Count > 0);
        }

        //GetElasticErrorMessages

    }
}
