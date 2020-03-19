using System;
using System.Text;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AlertsAdmin.Monitor.Notifiers
{
    public interface INotifier<T>
    {
        void Notify(T data);

    }
   
}
