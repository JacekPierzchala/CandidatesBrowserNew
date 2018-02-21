using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CandidatesBrowser2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public DataTable Candidates;
        PcName pcmane;

        public MainWindow()
        {

            ParseArgs();
            LoadData();



            //Thread.Sleep(5000);
            InitializeComponent();

            MainView.ItemsSource = Candidates.AsDataView();
        }

        void ParseArgs()
        {
            if (App.Args == null)
            {
                MessageBox.Show("Please run this application using bat file", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }


            try
            {
                pcmane = (PcName)Enum.Parse(typeof(PcName), App.Args[0]);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Please run this application using bat file", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }


            switch (pcmane)
            {
                case PcName.Michal:
                    {
                        GlobalFunctions.connectionString = GlobalFunctions.connectionStringMichal;
                    }
                    break;

                case PcName.Zaneta:
                    {
                        GlobalFunctions.connectionString = GlobalFunctions.connectionStringZaneta;
                    }
                    break;
            }

        }

        void LoadData()
        {
            Candidates = GlobalFunctions.GetTableFromSQL(SQLs.Candidates);
        }
    }
}
