using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CandidatesBrowser2
{
    public enum PcName
        {
        Michal,
        Zaneta
        }
    class GlobalFunctions
    {

        public static string connectionStringZaneta = @"Server=ZANETA-PC\SQLEXPRESS;database=Candidates;integrated Security=SSPI";
        public static string connectionStringMichal = @"Server=DESKTOP-3U4D69V\SQLEXPRESS;database=Candidates;integrated Security=SSPI";
        public static string connectionString;

        public  static DataTable GetTableFromSQL(string sql)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connectionString);
           
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

            dataAdapter.SelectCommand.CommandTimeout = 1000;
            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dataAdapter.Fill(table);
            return table;
        }
    }
}
