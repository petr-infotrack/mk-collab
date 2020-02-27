using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AlertsAdmin.Domain.Models;

namespace AlertsAdmin.Domain.Interfaces
{
    public interface IAlertInstanceRepository
    {
        Task<IEnumerable<AlertInstance>> GetAllAlertInstancesAsync();
    }
}
