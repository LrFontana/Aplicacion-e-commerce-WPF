using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Repositories.ExamenDb
{
    public abstract class ExamenDbContext
    {
        //Propiedades.
        private readonly string _connectionString;

        //Constructor.
        public ExamenDbContext()
        {
            _connectionString = "Server=DESKTOP-A2H9T9K\\LEOGESTIO; Database=pruebademo; Trusted_Connection=True; MultipleActiveResultSets=True; Encrypt=False";
        }

        //Metodo.
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
