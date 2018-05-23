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
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using Word = Microsoft.Office.Interop.Word;
using System.Windows;

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

        public static void ReadWordFile(string wordFilename)
        {

            // Create a WordApplication and host word document 
            Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            try
            {  
                

                if (!IsFileLocked(wordFilename))
                {
                    wordApp.Documents.Open(wordFilename, ReadOnly: false);
                }
                else
                {
                    wordApp.Documents.Open(wordFilename, ReadOnly: true);
                }
               // wordApp.Documents.Open(wordFilename,true);
               
               

                // To Invisible the word document 
                wordApp.Application.Visible = true;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error occurs, The error message is  " + ex.ToString());
                return ;
            }
            //finally
            //{
            //    wordApp.Documents.Close();
            //    ((_Application)wordApp).Quit(WdSaveOptions.wdDoNotSaveChanges);
            //}
        }

        public static XpsDocument ConvertWordToXps(string wordFilename, string xpsFilename)
        {
            // Create a WordApplication and host word document 
            Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            try
            {
                wordApp.Documents.Open(wordFilename);

                // To Invisible the word document 
                wordApp.Application.Visible = true;


                // Minimize the opened word document 
                wordApp.WindowState = WdWindowState.wdWindowStateMinimize;


                Document doc = wordApp.ActiveDocument;


                doc.SaveAs(xpsFilename, WdSaveFormat.wdFormatXPS);


                XpsDocument xpsDocument = new XpsDocument(xpsFilename, FileAccess.Read);
                return xpsDocument;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error occurs, The error message is  " + ex.ToString());
                return null;
            }
            finally
            {
                wordApp.Documents.Close();
                ((_Application)wordApp).Quit(WdSaveOptions.wdDoNotSaveChanges);
            }
        }

    }
}

