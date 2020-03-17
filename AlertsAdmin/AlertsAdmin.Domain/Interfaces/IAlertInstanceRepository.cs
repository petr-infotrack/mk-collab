using AlertsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertsAdmin.Domain.Interfaces
{
    public interface IAlertInstanceRepository
    {
        Task<IEnumerable<AlertInstance>> GetAlertInstancesAsync(int alertId);
    }
}
