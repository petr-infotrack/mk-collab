using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlertsAdmin.Data.Contexts
{
    public class LdmCoreContext:DbContext
    {
        public LdmCoreContext(DbContextOptions options)
            : base(options) { }
    }
}
