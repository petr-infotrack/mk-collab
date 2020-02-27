using System.Reflection;
using Serilog;
using Topshelf;

using Ldm.Alerts.Service.Interfaces;
using System;

namespace Ldm.Alerts.Service.Services
{
    public abstract class WindowsServiceBase : IWindowsService
    {
        private readonly string _appVersion;

        protected readonly ILogger Logger;

        public WindowsServiceBase(ILogger logger)
        {
            Logger = logger;

            _appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        protected abstract bool StartAction(HostControl host);
        protected abstract bool StopAction(HostControl host);
        protected abstract bool PauseAction(HostControl host);
        protected abstract bool ResumeAction(HostControl host);
        protected abstract bool ShutdownAction(HostControl host);


        public bool Start(HostControl host)
        {
            try
            {
                Logger.Information($"Alert Monitoring Service is starting.  Version: {_appVersion}", _appVersion);

                var result = StartAction(host);

                Logger.Information($"Alert Monitoring Service has started.");

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Alert Monitoring Service Start() has failed.");
                // ignored
            }

            return false;

        }

        public bool Stop(HostControl host)
        {
            try
            {
                Logger.Information($"Alert Monitoring Service is stopping ...");

                var result = StopAction(host);

                Logger.Information($"Alert Monitoring Service has stopped.");

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Alert Monitoring Service Stop() has failed.");
                // ignored
            }

            return false;
        }

        public bool Pause(HostControl host)
        {
            try
            {
                Logger.Information($"Alert Monitoring Service is being paused ...");

                var result = PauseAction(host);

                Logger.Information($"Alert Monitoring Service has paused.");

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Alert Monitoring Service Pause() has failed.");
                // ignored
            }

            return false;

        }

        public bool Resume(HostControl host)
        {
            try
            {
                Logger.Information($"Alert Monitoring Service is being resumed ...");

                var result = ResumeAction(host);

                Logger.Information($"Alert Monitoring Service has resumed.");

                return result;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Alert Monitoring Service Resume() has failed.");
                // ignored
            }

            return false;

        }

        public bool Shutdown(HostControl host)
        {
            try
            {
                Logger.Information($"Alert Monitoring Service is shutting down ...");

                var result = ShutdownAction(host);

                Logger.Information($"Alert Monitoring Service has been shutdown.");

                return result;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Alert Monitoring Service Shutdown() has failed.");
                // ignored
            }

            return false;
        }
    }
}
