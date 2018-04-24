using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        #region Collections
        public DataTable CandidatesDT;
        public ObservableCollection<Candidate> CandidatesCollection=new ObservableCollection<Candidate>();
        public DataTable StatusesDT;
        public ObservableCollection<Status> StatusCollection = new ObservableCollection<Status>();
        public DataTable ProjectsDT;
        public ObservableCollection<Project> ProjectsCollection = new ObservableCollection<Project>();
        public DataTable ProjectsGroupDT;
        public ObservableCollection<ProjectGroup> ProjectGroupCollection = new ObservableCollection<ProjectGroup>();
        public DataTable AreaDT;
        public ObservableCollection<Area> AreaCollection = new ObservableCollection<Area>();
        public DataTable GroupDT;
        public ObservableCollection<Group> GroupCollection = new ObservableCollection<Group>();

        PcName pcmane;
        #endregion

        public MainWindow()
        {
            ParseArgs();
            LoadData();
  
            InitializeComponent();

            MainView.ItemsSource = CandidatesCollection;
            StatusCombo.ItemsSource = StatusCollection;
            AreaCombo.ItemsSource = AreaCollection;
            ProjectList.ItemsSource = ProjectsCollection;
            GroupList.ItemsSource = GroupCollection;
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

            #region Projects
            ProjectsDT = GlobalFunctions.GetTableFromSQL(SQLs.Projects);
            foreach (DataRow row in ProjectsDT.Rows)
            {
                ProjectsCollection.Add
                    (
                    new Project(
                                id: int.Parse(row["ID"].ToString()),
                                ProjectName: row["PROJECT_NAME"].ToString()
                                
                               )
                     );
            }


            #endregion

            #region ProjectsGroup
            ProjectsGroupDT = GlobalFunctions.GetTableFromSQL(SQLs.ProjectGroup);

            foreach (DataRow row in ProjectsGroupDT.Rows)
            {
                ProjectGroupCollection.Add
                    (
                    new ProjectGroup(
                                id: int.Parse(row["ID"].ToString()),
                                ConfigProjectLibID: int.Parse(row["CONFIG_PROJECT_LIB"].ToString()),
                                ConfigGroupID: int.Parse(row["CONFIG_GROUP_ID"].ToString()),
                                ConfigAreaID: int.Parse(row["CONFIG_AREA_ID"].ToString())

                               )
                     );
            }

            #endregion

            #region Area
            AreaDT = GlobalFunctions.GetTableFromSQL(SQLs.Area);

            foreach (DataRow row in AreaDT.Rows)
            {
                AreaCollection.Add
                    (
                    new Area(
                             id: int.Parse(row["ID"].ToString()),
                             areaName: row["AREA_NAME"].ToString(),
                             isChecked: true
                             )
                     );
            }

            #endregion

            #region

            GroupDT = GlobalFunctions.GetTableFromSQL(SQLs.Groups);

            foreach (DataRow row in GroupDT.Rows)
            {
                GroupCollection.Add(
                    new Group (
                    id: int.Parse(row["ID"].ToString()),
                    name: row["NAME"].ToString()
                                )
                                );
            }

            #endregion
        }

        private void ProjectInPutText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeProjectList(((TextBox)sender).Text.ToString());
        }

        public  void ChangeProjectList(string text)
        {        
            ObservableCollection<Project> ProjectsCollectionFiltered = new ObservableCollection<Project>(ProjectsCollection.Where(Project => Project.ProjectName.ToLower().StartsWith(text.ToLower())).ToList());           
            ProjectList.ItemsSource = ProjectsCollectionFiltered;
        }

      
        

      
        private void AreaChckBox_Checked(object sender, RoutedEventArgs e)
        {
            string x = "";
            AreaColection_Change();
        }

        private void AreaChckBox_Click(object sender, RoutedEventArgs e)
        {
            AreaColection_Change();
        }

       
        public void AreaColection_Change()
        {
            ObservableCollection<Area> AreaFiltered = new ObservableCollection<Area>();
            foreach (Area item in AreaCombo.Items )
            {
               if(item.IsChecked)
                {
                    AreaFiltered.Add(item);
                }
            }

            //ObservableCollection<Project> ProjectFiltered = new ObservableCollection<Project>(ProjectsCollection.Join();


            string x = "";

        }


    }
}
