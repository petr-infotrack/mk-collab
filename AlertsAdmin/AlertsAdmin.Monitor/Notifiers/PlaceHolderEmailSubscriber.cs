using AlertsAdmin.Elastic.Models;

namespace AlertsAdmin.Monitor.Notifiers
{
    public class PlaceHolderEmailSubscriber : INotificationSubscriber<ElasticErrorMessage>
    {
        public PlaceHolderEmailSubscriber()
        {
        }

        public bool ShouldBeNotified(ElasticErrorMessage data)
        {
            return true;
        }

        // Mandatory method - receiver of the notification
        public void Notify(ElasticErrorMessage data)
        {
            //TODO - add the message construction
            var msg = "";

            this.SendEmail(msg);
        }

        // Some action - add any method(s) needed to achive the goal
        public void SendEmail(string message)
        {
            // do something
        }
    }
}