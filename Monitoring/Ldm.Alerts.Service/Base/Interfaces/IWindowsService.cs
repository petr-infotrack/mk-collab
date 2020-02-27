using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Ldm.Alerts.Service.Interfaces
{
    public interface IWindowsService
    {
        bool Start(HostControl host);
        bool Stop(HostControl host);
        bool Pause(HostControl host);
        bool Resume(HostControl host);
        bool Shutdown(HostControl host);
    }
}
