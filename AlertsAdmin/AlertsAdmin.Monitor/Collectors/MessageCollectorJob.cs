using System.Threading.Tasks;
using Quartz;

namespace AlertsAdmin.Monitor.Collector
{
    public class MessageCollectorJob : IJob
    {
        public MessageCollectorJob()
        {
        }

        public async Task Execute(IJobExecutionContext context)
        {
        }
    }
}
