using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Windows.Xps.Packaging;
using Microsoft.Office.Interop.Excel;

using Microsoft.Win32;

using System.Windows;
using System.Collections.ObjectModel;

namespace CandidatesBrowser2
{
    public enum PcName
        {
        Michal,
        Zaneta
        }
    class GlobalFunctions
    {

        public static string appPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public static string CVfolderPath = appPath + "\\source\\CV\\";

        public static string connectionStringZaneta = @"Server=ZANETA-PC\SQLEXPRESS;database=Candidates;integrated Security=SSPI";
        public static string connectionStringMichal = @"Server=DESKTOP-3U4D69V\SQLEXPRESS;database=Candidates;integrated Security=SSPI";
        public static string connectionString;

        public static string ReadScalar(string SQL)
        {
            System.Data.DataTable results = new System.Data.DataTable();
            SqlConnection connection = new SqlConnection(connectionString);
            
            string result = null;
            SqlCommand command = new SqlCommand(SQL, connection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            int attemp = 1;
            bool success = false;

            for (attemp = 1; attemp < 4; attemp++)
            {
                if (success == true)
                {
                    break;
                }
                else
                {
                    try
                    {
                        connection.Open();
                        success = true;
                    }
                    catch
                    {
                       // string x = MessageBox.Show("System tried " + attemp + "time(s) to connect the database. Would you like to continue?", "Connection issue", MessageBoxButtons.YesNo).ToString();

                    }
                }

            }



            SqlDataReader reader = command.ExecuteReader();

            reader.Read();
            if (reader.HasRows == true)
            { result = reader[0].ToString(); }
            connection.Close();
            return result;

        }


        public static System.Data.DataTable GetTableFromSQL(string sql)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connectionString);
           
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

            dataAdapter.SelectCommand.CommandTimeout = 1000;
            //dataAdapter.SelectCommand.Parameters.Add()
            System.Data.DataTable table = new System.Data.DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dataAdapter.Fill(table);
            return table;
        }

        public static System.Data.DataTable GetTableFromServerArgs( string procedureName ,params string [] ArgsValues)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            System.Data.DataTable table = new System.Data.DataTable();
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

        public static void ExecProcedureWithArgs(string procedureName, params string[] ArgsValues)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
           
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;
            cmd.Connection = sqlConnection;
            sqlConnection.Open();

            foreach (string param in ArgsValues)
            {
                string paramName = param.Substring(0, param.IndexOf("-"));
                string paramValue = null;
                if (param.IndexOf("-") != param.Length - 1)
                {
                    paramValue = param.Substring(param.IndexOf("-") + 1, param.Length - param.IndexOf("-") - 1);

                }
                cmd.Parameters.AddWithValue(paramName, paramValue);
            }




            cmd.ExecuteNonQuery();
        }

        public static bool IsFileLocked(string file)
        {
            FileStream stream = null;

            try
            {
                stream = new FileStream(file, FileMode.Open, FileAccess.Read);
                
               if (stream.Length<=2000)
                {
                    return true;
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }



        public static void ExportToExcel(System.Data.DataTable DT)
        {


            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Microsoft.Office.Interop.Excel.Application();
            xlexcel.Visible = false;
            xlexcel.DisplayAlerts = false;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            for (int i = 0; i < DT.Columns.Count; i++)
            {
                xlWorkSheet.Cells[1, i + 1] = DT.Columns[i].ColumnName.ToString();
            }

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                // to do: format datetime values before printing
                for (int j = 0; j < DT.Columns.Count; j++)
                {
                    xlWorkSheet.Cells[(i + 2), (j + 1)] = DT.Rows[i][j].ToString();
                        
                }
            }


            Microsoft.Office.Interop.Excel.Range MainRange = xlWorkSheet.UsedRange;


            int lastcol = MainRange.Columns.Count;

            for (int i = 1; i <= lastcol; i++)
            {
                Microsoft.Office.Interop.Excel.Range allColumns = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Columns[i];
                allColumns.AutoFit();
            }

            Microsoft.Office.Interop.Excel.Range HeaderRow = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Range[xlWorkSheet.Cells[1, 1], xlWorkSheet.Cells[1, lastcol]];
            HeaderRow.Cells.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbLightGray;
            HeaderRow.Cells.Font.Bold = true;
            MainRange.Borders.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlack;
            xlexcel.Visible = true;
            MessageBox.Show("file is ready", "",MessageBoxButton.OK);


        }

        public static System.Data.DataTable ToDataTable<T>(ObservableCollection<T> items)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}

