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
using System.IO;
using Microsoft.Win32;
using System.Windows.Xps.Packaging;


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

        public ObservableCollection<ProjectGroup> ProjectGroupFiltered;
        public ObservableCollection<Project> ProjectFiltered;
        public ObservableCollection<Area> AreaFiltered;

        public static string Key;

        public static MainWindow MainWindStatic;

        PcName pcmane;
        #endregion


        public MainWindow()
        {

            ParseArgs();
            LoadData();

            Key = GlobalFunctions.ReadScalar(SQLs.key);

            PasswordWindow pass = new PasswordWindow();
            pass.Show();


            InitializeComponent();

            
            MainView.ItemsSource = CandidatesCollection;
            StatusCombo.ItemsSource = StatusCollection;
            AreaCombo.ItemsSource = AreaCollection;
            ProjectList.ItemsSource = ProjectsCollection;
            GroupList.ItemsSource = GroupCollection;
            MainWindStatic = this;
            MainWindowView.Hide();
        }

        void ParseArgs()
        {
            if (App.Args == null)
            {
                MessageBox.Show("Please run this application using bat file", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                System.Windows.Application.Current.Shutdown();
                Environment.Exit(0);
                return;
               
            }

            try
            {
                if (!Directory.Exists(GlobalFunctions.CVfolderPath))
                {
                    DirectoryInfo di =Directory.CreateDirectory(GlobalFunctions.CVfolderPath);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }
                else
                {
                    DirectoryInfo di = new DirectoryInfo(GlobalFunctions.CVfolderPath);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }

                
            }

            catch
            {

            }

            try
            {
                pcmane = (PcName)Enum.Parse(typeof(PcName), App.Args[0]);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Please run this application using bat file", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                System.Windows.Application.Current.Shutdown();
                Environment.Exit(0);
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

            CandidatesCollection=Candidate.CreateCandidatesCollection(CandidatesDT);
              

            #endregion

            #region Status
            StatusesDT = GlobalFunctions.GetTableFromSQL(SQLs.Statuses);
            StatusCollection = Status.CreateStatusCollection(StatusesDT);
            #endregion

            #region Projects
            ProjectsDT = GlobalFunctions.GetTableFromSQL(SQLs.Projects);
            ProjectsCollection = Project.CreateProjectCollection(ProjectsDT);

            #endregion

            #region ProjectsGroup
            ProjectsGroupDT = GlobalFunctions.GetTableFromSQL(SQLs.ProjectGroup);
            ProjectGroupCollection = ProjectGroup.CreateProjectGroupCollection(ProjectsGroupDT);
            #endregion

            #region Area
            AreaDT = GlobalFunctions.GetTableFromSQL(SQLs.Area);
            AreaCollection=Area.CreateAreaCollection(AreaDT);
            #endregion

            #region Group
            GroupDT = GlobalFunctions.GetTableFromSQL(SQLs.Groups);
            GroupCollection = Group.CreateGroupCollection(GroupDT);            
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
            //llectionsChange("Area");
            AreaColection_Change();
        }
      
        public void AreaColection_Change()
        {
            ObservableCollection<Area> AreaFiltered = new ObservableCollection<Area>();
            foreach (Area item in AreaCombo.Items)
            {
                if (item.IsChecked)
                {
                    AreaFiltered.Add(item);
                }
            }

            if (AreaFiltered.Count == AreaCombo.Items.Count)
            {
                AreaSelectAllCheckbox.IsChecked = true;
            }
            else
            {
                AreaSelectAllCheckbox.IsChecked = false;
            }

            ObservableCollection<Project> ProjectFiltered =
                new ObservableCollection<Project>(ProjectsCollection.Join
                (ProjectGroupCollection.Join
                        (AreaFiltered, p => p.ConfigAreaID, a => a.Id, (p, a) => p).ToList(),
                  p => p.ID, pg => pg.ConfigProjectLibID, (p, pg) => p).ToList());
            ProjectList.ItemsSource = ProjectFiltered;
            ProjectInPutText.Text = "";

            //CollectionsChange("Area");


        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
          
            int i = 0;

            foreach (Group item in GroupList.Items)
            {
                if (item.IsChecked)
                {
                    i++;
                }
            }

            if (i == GroupList.Items.Count)
            {
                GroupSelectAllCheckbox.IsChecked = true;
            }
            else
                GroupSelectAllCheckbox.IsChecked = false;
        }

        private void CollectionsChange(string CollectionName)
        {
            switch (CollectionName)
            {
                case ("Area"):
                 {
                       AreaFiltered = new ObservableCollection<Area>();
                        foreach (Area item in AreaCombo.Items)
                        {
                            if (item.IsChecked)
                            {
                                AreaFiltered.Add(item);
                            }
                        }

                        if (AreaFiltered.Count == AreaCombo.Items.Count)
                        {
                            AreaSelectAllCheckbox.IsChecked = true;
                        }
                        else
                        {
                            AreaSelectAllCheckbox.IsChecked = false;
                        }

                      ProjectGroupFiltered = new ObservableCollection<ProjectGroup>(ProjectGroupCollection.Join(AreaFiltered, p => p.ConfigAreaID, a => a.Id, (p, a) => p).ToList());
                    }
                    break;

                case ("Group"):
                    {
                        ObservableCollection<Group> GroupFiltered = new ObservableCollection<Group>();
                        foreach (Group item in GroupList.Items)
                        {
                            if (item.IsChecked)
                            {
                                GroupFiltered.Add(item);
                            }
                        }

                        if (GroupFiltered.Count == GroupList.Items.Count)
                        {
                            GroupSelectAllCheckbox.IsChecked = true;
                        }
                        else
                        {
                            GroupSelectAllCheckbox.IsChecked = false;
                        }

                        ProjectGroupFiltered = new ObservableCollection<ProjectGroup>(ProjectGroupCollection.Join(GroupFiltered, p => p.ConfigGroupID, a => a.id, (p, a) => p).ToList());
                    
                     }
                    break;


                case ("Project"):
                    {
                        ProjectFiltered = new ObservableCollection<Project>();
                        foreach (Project item in ProjectList.Items)
                        {
                            if (item.IsChecked)
                            {
                                ProjectFiltered.Add(item);
                            }
                        }

                        if (ProjectFiltered.Count == ProjectList.Items.Count)
                        {
                            ProjectSelectAllCheckbox.IsChecked = true;
                        }
                        else
                        {
                            ProjectSelectAllCheckbox.IsChecked = false;
                        }

                        ProjectGroupFiltered = new ObservableCollection<ProjectGroup>(ProjectGroupCollection.Join(ProjectFiltered, p => p.ConfigProjectLibID, a => a.ID, (p, a) => p).ToList());

                    }
                    break;

            }
           //ProjectFiltered.Clear();
            ProjectFiltered = new ObservableCollection<Project>(ProjectsCollection.Join(ProjectGroupFiltered, p => p.ID, a => a.ConfigProjectLibID, (p, a) => p).ToList());
            ProjectList.ItemsSource = ProjectFiltered;
            ProjectInPutText.Text = "";

            //AreaFiltered.Clear();
            AreaFiltered = new ObservableCollection<Area>(AreaCollection.Join(ProjectGroupFiltered, p => p.Id, a => a.ConfigAreaID, (p, a) => p).ToList().Distinct());
            AreaCombo.ItemsSource = AreaFiltered;
        }

        private void ChckBox_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;

            foreach (Status item in StatusCombo.Items)
            {
                if (item.IsChecked)
                {
                    i++;
                }
            }

            if (i == StatusCombo.Items.Count)
            {
                StatusSelectAllCheckbox.IsChecked = true;
            }
            else
                StatusSelectAllCheckbox.IsChecked = false;
        }

        private void ProjectCheckbox_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;

            foreach (Project item in ProjectList.Items)
            {
                if (item.IsChecked)
                {
                    i++;
                }
            }

            if (i == ProjectList.Items.Count)
            {
                ProjectSelectAllCheckbox.IsChecked = true;
            }
            else
                ProjectSelectAllCheckbox.IsChecked = false;
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

            bool? CvReceived = null;

            if (CVrEceivedYes.IsChecked==true)
                CvReceived = true;
            else if (CVrEceivedNo.IsChecked == true)
                CvReceived = false;

            bool? CvUploaded = null;

            if (CvUploadedYes.IsChecked == true)
                CvUploaded = true;
            else if (CvUploadedNo.IsChecked == true)
                CvUploaded = false;

            CandidatesDT = GlobalFunctions.GetTableFromServerArgs("SEARCH_CANDIDATE", "@FIRST_NAME"+ "-"+ firstName, "@LAST_NAME" + "-" + lastName,
              "@POSITION" + "-" + position, "@PROJECT" + "-" + project, "@AREA" + "-" + area, "@GROUP" + "-" + group, "@STATUS"+ "-" + status
              , "@CVRECEIVED" + "-" + CvReceived, "@CV_UPLOADED" + "-" + CvUploaded);

            CandidatesCollection = Candidate.CreateCandidatesCollection(CandidatesDT);

            MainView.ItemsSource = CandidatesCollection;

        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            LastNametxt.Text = "";
            FirstNametxt.Text = "";
            Positiontxt.Text = "";
            CVrEceivedAll.IsChecked = true;
            CvUploadedAll.IsChecked = true;

            ProjectSelectAllCheckbox.IsChecked = true;
            ProjectSelectAllCheckbox_Click(this, e);
            GroupSelectAllCheckbox.IsChecked = true;
            GroupSelectAllCheckbox_Click(this, e);
            StatusSelectAllCheckbox.IsChecked = true;
            StatusSelectAllCheckbox_Click(this, e);
            AreaSelectAllCheckbox.IsChecked = true;
            AreaSelectAllCheckbox_Click(this, e);
        }

        #region ContextMenu
        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (MainView.SelectedItems.Count == 1 && ((Candidate)MainView.SelectedItem).IsCvUploaded)
            {
                readMenuItem.IsEnabled = true;
            }
            else
            {
                readMenuItem.IsEnabled = false;
            }

            if (MainView.SelectedItems.Count == 1 && ((Candidate)MainView.SelectedItem).IsCvUploaded)
            {
                removeMenuItem.IsEnabled = true;
            }
            else
            {
                removeMenuItem.IsEnabled = false;
            }


            if (MainView.SelectedItems.Count == 1)
            {
                attachMenuItem.IsEnabled = true;
            }
            else
            {
                attachMenuItem.IsEnabled = false;
            }
        }

        private void readMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string dectinationDirectory = GlobalFunctions.CVfolderPath + ((Candidate)MainView.SelectedItem).ID.ToString() + "\\";

            string [] files = Directory.GetFiles(dectinationDirectory);


            foreach (string file in files)
            {
               
                if (!file.Contains("~"))
                {
                    System.Diagnostics.Process.Start(file);
                    //GlobalFunctions.ReadWordFile(file);
                }
               
                //XpsDocument xpsDocument=GlobalFunctions.ConvertWordToXps(file, System.IO.Path.GetFileName(file));
                
            }

        }

        private void attachMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Word files(*.doc)|*.doc|PDF files(*.pdf)|*.pdf";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.ShowDialog();
            string  sourceFile= openFileDialog.FileName.ToString();
            if (!string.IsNullOrEmpty(sourceFile))
            {
                saveFile(sourceFile, ((Candidate)MainView.SelectedItem).ID.ToString());
            }
           
        }

        private void removeFile(string filePath, string dectinationDirectory)
        {
            if (!GlobalFunctions.IsFileLocked(filePath))
            {
                try
                {
                    string[] files = Directory.GetFiles(dectinationDirectory);
                    File.Delete(filePath);
                    if (files.Length == 1)
                    {
                        ((Candidate)MainView.SelectedItem).IsCvUploaded = false;

                        GlobalFunctions.ExecProcedureWithArgs("UPDATE_CV", "@ID" + "-" + ((Candidate)MainView.SelectedItem).ID, "@CV_UPLOADED" + "-" + false);

                    }
                    MessageBox.Show("File deleted succesfully ", "",MessageBoxButton.OK,MessageBoxImage.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("File was not deleted! " + ex.Message,"",MessageBoxButton.OK,MessageBoxImage.Error);
                }

            }
            else
            {
                MessageBox.Show("File is currently locked, cannot be deleted", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void removeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string dectinationDirectory = GlobalFunctions.CVfolderPath + ((Candidate)MainView.SelectedItem).ID.ToString() + "\\";
            OpenFileDialog openFileDialog = new OpenFileDialog();
           // openFileDialog.Filter = "Word files(*.doc)|*.doc|PDF files(*.pdf)|*.pdf";
            openFileDialog.InitialDirectory = dectinationDirectory;
            
            openFileDialog.ShowDialog();
            string sourceFile = openFileDialog.FileName.ToString();

           
                if (!string.IsNullOrEmpty(sourceFile))
                {
                    removeFile(sourceFile, dectinationDirectory);
                }
            
            
        }

        private void saveFile(string sourceFilePath, string id)
        {
            string fileName = System.IO.Path.GetFileName(sourceFilePath);
            string dectinationDirectory= GlobalFunctions.CVfolderPath+  id + "\\";
            if (!Directory.Exists(dectinationDirectory))
            {
                Directory.CreateDirectory(dectinationDirectory);
            }
            if (File.Exists(dectinationDirectory + fileName))
            {
              
                if ((MessageBox.Show("File you are trying to copy already exists in this folder. Would you like to remove old one?", "", MessageBoxButton.YesNo))==MessageBoxResult.Yes)
                {


                }
                else
                {
                    return;
                }
            }

            try
            {
                File.Copy(sourceFilePath, dectinationDirectory + fileName);
                ((Candidate)MainView.SelectedItem).IsCvUploaded = true;
                GlobalFunctions.ExecProcedureWithArgs("UPDATE_CV", "@ID" + "-" + ((Candidate)MainView.SelectedItem).ID, "@CV_UPLOADED" + "-" + true);
                MessageBox.Show("File attached succesfully ", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("File was not deleted! " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }




        }

        #endregion
    }
}
