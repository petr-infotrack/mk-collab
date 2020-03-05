using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;


namespace Ldm.Charting.Data
{
    public class VicImageCountsRepository : IVicImageCountsRepository
    {
        private readonly IDbConnection _db = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PULSE"].ConnectionString);

        public IEnumerable<dynamic> GetVicImageCounts()
        {
            return _db.Query("Vic_images", null, commandType: CommandType.StoredProcedure);
        }
    }
}
