using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser2
{
  

        public class Candidate :INotifyPropertyChanged
        {
            public int ID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FirstEmail { get; set; }
            public string SecondEmail { get; set; }
            public string FirstPhone { get; set; }
            public string SecondPhone { get; set; }
            public int AttendedProjects { get; set; }
            public bool IsCvReceived { get; set; }
            public bool IsCvUploaded
            {
                    get
                    {
                        return _isCvUploaded;
                    }

                    set
                    {
                     _isCvUploaded = value;
                    if (PropertyChanged !=null)
                            {
                             PropertyChanged(this, new PropertyChangedEventArgs("IsCvUploaded"));
                            }
                    }

            }

            public event PropertyChangedEventHandler PropertyChanged;

            private bool _isCvUploaded;
        
        public Candidate() { }

        public Candidate(int id, string firstName, string lastName,
                    string firstEmail, string secondEmail,
                    string firstPhone, string secondPhone,
                    int attendedProjects, bool isCvReceived,
                    bool isCvUploaded)
            {
                this.ID = id;
                this.FirstName = firstName;
                this.LastName = lastName;
                this.FirstEmail = firstEmail;
                this.SecondEmail = secondEmail;
                this.FirstPhone = firstPhone;
                this.SecondPhone = secondPhone;
                this.AttendedProjects = attendedProjects;
                this.IsCvReceived = isCvReceived;
                this.IsCvUploaded = isCvUploaded;
            }

        public static ObservableCollection<Candidate> CreateCandidatesCollection(DataTable DataT)
        {
            ObservableCollection<Candidate> CandidatesCollection = new ObservableCollection<Candidate>();
            foreach(DataRow row in DataT.Rows)
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
                        isCvReceived: bool.Parse(row["CV_RECEIVED"].ToString()),
                        isCvUploaded: bool.Parse(row["CV_UPLOADED"].ToString())
                        )
                      );
            }


            return CandidatesCollection;
        }

        }

        public class CandidateDetails 
        {

        public CandidateDetails() { }

        public CandidateDetails(int candidateId,int configCompanyID, string position, int configProjectId,
                                string companyName, string projectName, string areaName, string groupName
                                )
        {
            this.CandidateId = candidateId;
            this.ConfigCompanyID = configCompanyID;
            this.Position = position;
            this.ConfigProjectId = configProjectId;
            this.CompanyName = companyName;
            this.ProjectName = projectName;
            this.AreaName = areaName;
            this.GroupName = GroupName;
        }

        private int _candidateId;
        public int CandidateId
        {
            get { return _candidateId; }
            set { _candidateId = value; }
        }

        private int _configCompanyID;
        public int ConfigCompanyID
        {
            get { return _configCompanyID; }
            set { _configCompanyID = value; }
        }

        private string _position;
        public string Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private int _configProjectId;
        public int ConfigProjectId
        {
            get { return _configProjectId; }
            set { _configProjectId = value; }
        }

        private string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }

        private string _projectName;
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        private string _areaName;
        public string AreaName
        {
            get { return _areaName; }
            set { _areaName = value; }
        }

        private string _groupName;

        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }


        public static ObservableCollection<CandidateDetails> CreateAreaCollection(DataTable DataT)
        {
            ObservableCollection<CandidateDetails> CandidateDetailsCollection = new ObservableCollection<CandidateDetails>();

            foreach (DataRow row in DataT.Rows)
            {
                CandidateDetailsCollection.Add
                    (
                    new CandidateDetails(
                             candidateId: int.Parse(row["CANDIDATE_ID"].ToString()),
                             configCompanyID: int.Parse(row["CONFIG_COMPANY_ID"].ToString()),
                             areaName: row["AREA_NAME"].ToString(),
                             companyName: row["COMPANY_NAME"].ToString(),
                             configProjectId: int.Parse(row["CONFIG_COMPANY_ID"].ToString()),
                             groupName: row["GROUP_NAME"].ToString(),
                             position: row["POSITION"].ToString(),
                             projectName: row["PROJECT_NAME"].ToString()
                             )
                     );
            }
            return CandidateDetailsCollection;
        }
    }
       
        public class Area : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;           
            private bool _isChecked;

            public int Id { get; set; }
            public string AreaName { get; set; }
            public bool IsChecked {
                get
                {
                    return _isChecked;
                }
                set
                {
                    _isChecked = value;
                if (PropertyChanged !=null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
                }
                
                }
            }

            public Area(int id, string areaName, bool isChecked)
            {
                this.Id = id;
                this.AreaName = areaName;
                this.IsChecked = isChecked;
            }


        public static ObservableCollection<Area> CreateAreaCollection(DataTable DataT)
        {
            ObservableCollection<Area> AreaCollection = new ObservableCollection<Area>();

            foreach (DataRow row in DataT.Rows)
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
            return AreaCollection;
        }
    }

        public class Status : INotifyPropertyChanged
    {
            public int ID { get; set; }
            public string Description { get; set; }
            public string Definition { get; set; }
            public bool Deleted { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
            private bool _isChecked;

        public Status(int id, string description, string definition, bool deleted, bool isChecked)
            {
                this.ID = id;
                this.Description = description;
                this.Definition = definition;
                this.Deleted = deleted;
                this.IsChecked = isChecked;
        }

        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
                }

            }
        }

        public static ObservableCollection<Status> CreateStatusCollection(DataTable DataT)
        {
            ObservableCollection<Status> StatusCollection = new ObservableCollection<Status>();

            foreach (DataRow row in DataT.Rows)
            {
                StatusCollection.Add
                    (
                    new Status(
                                id: int.Parse(row["ID"].ToString()),
                                description: row["DESCRIPTION"].ToString(),
                                definition: row["DEFINITION"].ToString(),
                                deleted: bool.Parse(row["DELETED"].ToString()),
                                isChecked: true
                               )
                     );
            }
            return StatusCollection;
        }
    }

        public class Project : INotifyPropertyChanged
    {
            public int ID { get; set; }
            public string ProjectName { get; set; }
            public event PropertyChangedEventHandler PropertyChanged;
            private bool _isChecked;

        public Project(int id, string ProjectName, bool isChecked)
            {
                this.ID = id;
                this.ProjectName = ProjectName;
                this.IsChecked = isChecked;
            }

        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
                }

            }
        }


        public static ObservableCollection<Project> CreateProjectCollection(DataTable DataT)
        {
            ObservableCollection<Project> ProjectCollection = new ObservableCollection<Project>();

            foreach (DataRow row in DataT.Rows)
            {
                ProjectCollection.Add
                    (
                    new Project(
                                id: int.Parse(row["ID"].ToString()),
                                ProjectName: row["PROJECT_NAME"].ToString(),
                                isChecked: true

                               )
                     );
            }
            return ProjectCollection;
        }
    }

        public class ProjectGroup
        {
            public int ID { get; set; }
            public int ConfigProjectLibID { get; set; }
            public int ConfigGroupID { get; set; }
            public int ConfigAreaID { get; set; }

            public ProjectGroup(int id, int ConfigProjectLibID, int ConfigGroupID, int ConfigAreaID)
            {
                this.ID = id;
                this.ConfigAreaID = ConfigAreaID;
                this.ConfigGroupID = ConfigGroupID;
                this.ConfigProjectLibID = ConfigProjectLibID;

            }

        public static ObservableCollection<ProjectGroup> CreateProjectGroupCollection(DataTable DataT)
        {
            ObservableCollection<ProjectGroup> ProjectGroupCollection = new ObservableCollection<ProjectGroup>();

            foreach (DataRow row in DataT.Rows)
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
            return ProjectGroupCollection;
        }
    }

        public class Group : INotifyPropertyChanged
    {
            public int id { get; set; }
            public string Name { get; set; }
            public event PropertyChangedEventHandler PropertyChanged;
            private bool _isChecked;

            public bool IsChecked
            {
                get
                {
                    return _isChecked;
                }
                set
                {
                    _isChecked = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
                    }

                }
            }
            public Group(int id, string name, bool isChecked)
            {
                this.id = id;
                this.Name = name;
                this.IsChecked = isChecked;
            }


        public static ObservableCollection<Group> CreateGroupCollection(DataTable DataT)
        {
            ObservableCollection<Group> GroupCollection = new ObservableCollection<Group>();
            foreach (DataRow row in DataT.Rows)
            {
                GroupCollection.Add
                    (
                    new Group(
                    id: int.Parse(row["ID"].ToString()),
                    name: row["NAME"].ToString(),
                    isChecked: true
                                )
                      );
            }
            return GroupCollection;
        }
    }


}
  

    

