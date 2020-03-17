using System;
using AlertsAdmin.Elastic;
using Xunit;

namespace Alerts.Backend.Tests.ElasticFeed
{
    public class ElasticFeedTests
    {
        [Fact]
        public void ReadSummarizedErrorsFromElastic()
        {
            var el = new ElasticDataRepository();


            var errors = el.GetElasticSearchErrors(10, 10, 10);

            Assert.True(errors.Count>0);

        }

        [Fact]
        public void ReadErrorsFromElastic()
        {
            var el = new ElasticDataRepository();


            var errors = el.GetElasticErrorMessages(DateTime.Now, 10, 10);

            Assert.True(errors.Count > 0);

        }

        //GetElasticErrorMessages

    }
}
