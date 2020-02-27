using Ldm.Alerts.Service.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;

namespace Ldm.Alerts.Service.Services
{
    public class AlertMonitorService : WindowsServiceBase, IWindowsService
    {
        private const int DEFAULT_TIMER_INTERVAL_MINUTES = 3;

        private const int FIRST_RUN_TIMER_INTERVAL_MS = 500;

        private static object _jobLock = new object();

        private readonly IScheduledJob _job;

        private readonly System.Timers.Timer _timer;

        private bool _firstRunFlag = true;

        private int _timerInterval = DEFAULT_TIMER_INTERVAL_MINUTES * 60 * 1000; // DEBUG Return values

        private static int collectionInProgress = 0;

        private const int YES = 1;
        private const int NO = 0;


        // TODO -- replace string with structure
 
        public AlertMonitorService(ILogger logger, IScheduledJob job) : base(logger)
        {
            _timer = new System.Timers.Timer
            {
                AutoReset = false,
                Interval = _firstRunFlag ? FIRST_RUN_TIMER_INTERVAL_MS : _timerInterval
            };

            _timer.Elapsed += TimerElapsed;

            _job = job;

            _job.Completed += JobControlCompleted;

            _job.Error += JobControlError; ;

            _firstRunFlag = false;
        }

        private void JobControlError(object sender, EventArgs e)
        {
            Interlocked.CompareExchange(ref collectionInProgress, NO, YES);
        }

        protected void JobControlCompleted(object sender, EventArgs e)
        {
            Interlocked.CompareExchange(ref collectionInProgress, NO, YES);
        }

        #region -- Timer implementation --

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;

            _timer.Interval = _timerInterval;

            _timer.Start();

            if (Interlocked.Exchange(ref collectionInProgress, YES) == NO)
            {
                //DEBUG ONLY
                //_job.Execute();

                await _job.ExecuteAsync();
            }
        }

        private void StartTimer()
        {
            if (!_timer.Enabled)
            {
                _timer.Start();
            }
        }


        private void StopTimer()
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
            }
        }

        #endregion

        #region -- Service Action Handlers --

        protected override bool StartAction(HostControl host)
        {
            StartTimer();

            return true;
        }

        protected override bool StopAction(HostControl host)
        {
            StopTimer();

            //if (collectionInProgress)
            //{
            //     _job.Cancel();
            //}

            return true;
        }

        protected override bool PauseAction(HostControl host)
        {
            //TODO implement
            return false;
        }

        protected override bool ResumeAction(HostControl host)
        {
            //TODO implement
            return false;
        }

        protected override bool ShutdownAction(HostControl host)
        {
            StopTimer();

            return true;
        }

        #endregion
    }
}
