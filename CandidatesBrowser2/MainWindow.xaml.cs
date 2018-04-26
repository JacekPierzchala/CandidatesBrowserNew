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
            try
            {
                CandidatesDT = GlobalFunctions.GetTableFromSQL(SQLs.Candidates);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Cannot run application " + ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
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
                                deleted: bool.Parse(row["DELETED"].ToString()),
                                isChecked:true
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
                                ProjectName: row["PROJECT_NAME"].ToString(),
                                isChecked:true
                                
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

            #region Group

            GroupDT = GlobalFunctions.GetTableFromSQL(SQLs.Groups);

            foreach (DataRow row in GroupDT.Rows)
            {
                GroupCollection.Add(
                    new Group (
                    id: int.Parse(row["ID"].ToString()),
                    name: row["NAME"].ToString(),
                    isChecked:true
                                )
                                );
            }

            #endregion
        }

        #region CollectionsChange
        private void ProjectInPutText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeProjectList(((TextBox)sender).Text.ToString());
        }

        public  void ChangeProjectList(string text)
        {        
            ObservableCollection<Project> ProjectsCollectionFiltered = new ObservableCollection<Project>(ProjectsCollection.Where(Project => Project.ProjectName.ToLower().StartsWith(text.ToLower())).ToList());           
            ProjectList.ItemsSource = ProjectsCollectionFiltered;
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

         
            ObservableCollection<Project> ProjectFiltered = new ObservableCollection<Project>(ProjectsCollection.Join(ProjectGroupCollection.Join(AreaFiltered, p => p.ConfigAreaID, a => a.Id, (p, a) => p).ToList(), p => p.ID, pg => pg.ConfigProjectLibID, (p, pg) => p).ToList());
            ProjectList.ItemsSource = ProjectFiltered;
            ProjectInPutText.Text = "";
        }

        #endregion
        
        #region SelectAll
        private void AreaSelectAllCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)AreaSelectAllCheckbox.IsChecked)
            {
                foreach (Area item in AreaCombo.Items)
                {
                    item.IsChecked = true;                                     
                }

            }
            else if(!(bool)AreaSelectAllCheckbox.IsChecked)
            {
                foreach (Area item in AreaCombo.Items)
                {
                    item.IsChecked = false;
                }
            }

            AreaColection_Change();
        }
      
        private void StatusSelectAllCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)StatusSelectAllCheckbox.IsChecked)
            {
                foreach (Status item in StatusCombo.Items)
                {
                    item.IsChecked = true;
                }

            }
            else if (!(bool)StatusSelectAllCheckbox.IsChecked)
            {
                foreach (Status item in StatusCombo.Items)
                {
                    item.IsChecked = false;
                }
            }
        }

        private void GroupSelectAllCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)GroupSelectAllCheckbox.IsChecked)
            {
                foreach (Group item in GroupList.Items)
                {
                    item.IsChecked = true;
                }

            }
            else if (!(bool)GroupSelectAllCheckbox.IsChecked)
            {
                foreach (Group item in GroupList.Items)
                {
                    item.IsChecked = false;
                }
            }
        }

        private void ProjectSelectAllCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)ProjectSelectAllCheckbox.IsChecked)
            {
                foreach (Project item in ProjectList.Items)
                {
                    item.IsChecked = true;
                }

            }
            else if (!(bool)ProjectSelectAllCheckbox.IsChecked)
            {
                foreach (Project item in ProjectList.Items)
                {
                    item.IsChecked = false;
                }
            }
        }

        #endregion

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string area = "|";
            foreach (Area item in AreaCombo.Items)
            {
                if(item.IsChecked)
                {
                    area += item.Id + "|";
                }
            }
            if (area == "|")
            {
                area = null;
            }


            string status = "|";
            foreach (Status item in StatusCombo.Items)
            {
                if (item.IsChecked)
                {
                    status += item.ID + "|";
                }
            }
            if (status == "|")
            {
                status = null;
            }



            string group = "|";
            foreach (Group item in GroupList.Items)
            {
                if (item.IsChecked)
                {
                    group += item.id + "|";
                }
            }
            if (group == "|")
            {
                group = null;
            }

            string project = "|";
            foreach (Project item in ProjectList.Items)
            {
                if (item.IsChecked)
                {
                    project +=  item.ID +"|";
                }
            }           
            if (project== "|")
            {
                project = null;
            }

            string lastName = LastNametxt.Text;
            string firstName = FirstNametxt.Text;
            string position = Positiontxt.Text;

            CandidatesDT = GlobalFunctions.GetTableFromServerArgs("SEARCH_CANDIDATE", "@FIRST_NAME"+ "-"+ firstName, "@LAST_NAME" + "-" + lastName,
              "@POSITION" + "-" + position, "@PROJECT" + "-" + project, "@AREA" + "-" + area, "@GROUP" + "-" + group, "@STATUS"+ "-" + status);

            CandidatesCollection.Clear();


            foreach (DataRow row in CandidatesDT.Rows)
            {
                CandidatesCollection.Add
                    (
                    new Candidate
                        (
                        id: int.Parse(row["ID"].ToString()),
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

            MainView.ItemsSource = CandidatesCollection;

        }
    }
}
