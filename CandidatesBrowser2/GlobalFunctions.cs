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
            //dataAdapter.SelectCommand.Parameters.Add()
            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dataAdapter.Fill(table);
            return table;
        }

        public static DataTable GetTableFromServerArgs( string procedureName ,params string [] ArgsValues)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            DataTable table = new DataTable();
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;
            cmd.Connection = sqlConnection;
            sqlConnection.Open();

            foreach (string param in ArgsValues)
            {
                string paramName = param.Substring(0, param.IndexOf("-"));
                string paramValue = null;
                if (param.IndexOf("-")!= param.Length-1)
                {
                    paramValue = param.Substring(param.IndexOf("-") + 1, param.Length - param.IndexOf("-") - 1);

                }
                cmd.Parameters.AddWithValue(paramName, paramValue);
            }
           
           
          

            reader = cmd.ExecuteReader();

            table.Load(reader);

            sqlConnection.Close();
            return table;
        }
    }
}
