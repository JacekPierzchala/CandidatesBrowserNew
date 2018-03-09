﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public DataTable CandidatesDT;
        public ObservableCollection<Candidate> CandidatesCollection=new ObservableCollection<Candidate>();
        public DataTable StatusesDT;
        public ObservableCollection<Status> StatusCollection = new ObservableCollection<Status>();

        PcName pcmane;

        public MainWindow()
        {
            ParseArgs();
            LoadData();
  
            InitializeComponent();

            MainView.ItemsSource = CandidatesCollection;
            StatusCombo.ItemsSource = StatusCollection;
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
            #region CandidatesDT
            CandidatesDT = GlobalFunctions.GetTableFromSQL(SQLs.Candidates);
            foreach (DataRow row in CandidatesDT.Rows)
            {
                CandidatesCollection.Add
                    (
                    new Candidate
                        (
                        id:int.Parse(row["ID"].ToString()),
                        firstName: row["FIRST_NAME"].ToString(),
                        lastName: row["LAST_NAME"].ToString(),
                        firstEmail: row["1ST_@"].ToString(),
                        secondEmail: row["2ND_@"].ToString(),
                        firstPhone: row["1ST_TEL"].ToString(),
                        secondPhone: row["2ND_TEL"].ToString(),
                        attendedProjects: int.Parse(row["ATTENDED_PROJECTS"].ToString()),
                        isCvReceived: bool.Parse(row["CV_RECEIVED"].ToString())
                        )                  
                      );
            }

            #endregion



            #region Status

            StatusesDT = GlobalFunctions.GetTableFromSQL(SQLs.Statuses);

            foreach(DataRow row in StatusesDT.Rows)
            {
                StatusCollection.Add
                    (
                    new Status(
                                id: int.Parse(row["ID"].ToString()),
                                description: row["DESCRIPTION"].ToString(),
                                definition: row["DEFINITION"].ToString(),
                                deleted: bool.Parse(row["DELETED"].ToString())
                               )
                     );
            }
            #endregion
        }
    }
}
