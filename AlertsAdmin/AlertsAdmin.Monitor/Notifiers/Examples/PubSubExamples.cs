using System;
using System.Collections.Generic;
using System.Text;

namespace AlertsAdmin.Monitor.Notifiers.Examples
{

    // EXAMPLE - shared data - e.g. alert instance or a part of it
    public class MyRecord
    {

        public string Something { get; set; }
    }


    // EXAMPLE - subscriber 
    //         - this is no constraint implementation that provides full flexibility to add and inject other essential elements to act on the received notification
    //           the main reason to stick with the simple pattern here is that it can be easily decoupled via proxy if needed

    public class TestEmailSubscriber : INotificationSubscriber<MyRecord>
    {

        public bool ShouldBeNotified(MyRecord data)
        {
            return true;
        }

       // Mandatory method - receiver of the notification
        public void Notify(MyRecord data)
        {
                var msg = "";

                this.SendEmail(msg);

        }

   // Some action - add any method(s) needed to achive the goal
        public void SendEmail(string message)
        {

        }
    }


    // EXAMPLE - publisher for testing purposes ( the actual publisher will be added this afternoon)
    public class AlertUpdatePublisher : NotificationPublisherBase<MyRecord>
    {
        public override void Publish(MyRecord data)
        {
            // -- possible filters or condition

            foreach (var subscriber in this.subscribers)
            {
                if (subscriber.ShouldBeNotified(data))
                {
                    subscriber.Notify(data);
                }
            }
        }
    }



    public class TestIntegration
    {
        private INotificationPublisher<MyRecord> _publisher;

        public TestIntegration()
        {
            // can be DIed
            _publisher = new AlertUpdatePublisher();

            _publisher.Register(new TestEmailSubscriber());

        }


        public void TriggerNotification()
        {

            var data = new MyRecord()
            {
                Something = "my test data"
            };

            _publisher?.Publish(data);
        }

    }
}
