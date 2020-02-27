using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ldm.Alerts.Service.Services.Interfaces;
using Ldm.Charting.Data;

namespace Ldm.Alerts.Service.Services
{
    public class CollectorNotifierJob : IScheduledJob
    {
        private const string FILTER_STRING = "";

        private IAlertSourceService _source;

        private IAlertProcessor<ErrorOcccurences> _processor;
        private INotifier<string> _notifier;


        public event EventHandler Completed;
        public event EventHandler Error;
        public event EventHandler Progress;


        public CollectorNotifierJob(IAlertSourceService source, IAlertProcessor<ErrorOcccurences> processor, INotifier<string> notifier)
        {
            _source = source;
            _processor = processor;
            _notifier = notifier;
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
            try
            {
                // -- call facade - log retrieval
                _source.Open();

                var errors = _source.GetAll();

                if (errors == null)
                {
                    return false;
                }

                foreach (var error in errors)
                {
                    if (_notifier != null)
                    {
                        //TODO search condition - refactor
                        if (error.ErrorMessage.Contains(FILTER_STRING))
                        {
                            // -- replace with formatter
                            var message = error.ToString();
                            _notifier.Notify(message);
                        }
                    }

                    if (_processor.Validate(error))
                    {
                        _processor.Process(error);
                    }
                }
            }
            catch (Exception ex)
            {
                // -- log, and suppress
                throw;
            }
            finally
            {
                _source.Close();
            }
        }

        public async Task ExecuteAsync()
        {
            await Task.Run(() =>
               {
                   this.Execute();

               }).ContinueWith((t) =>
               {
                   if (t.Status == TaskStatus.Faulted)
                   {
                       Error?.Invoke(this, new EventArgs());
                   }

                   Completed?.Invoke(this, new EventArgs());
               });
        }
    }
}
