using System;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;


namespace Ldm.Alerts.Service.Services
{
    public interface IScheduledJob
    {
        event EventHandler Completed;
        event EventHandler Error;
        event EventHandler Progress;

        void Execute();
        Task ExecuteAsync();
        void Cancel();
    }
}