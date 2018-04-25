using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser2
{
  

    
        public class Candidate
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

            public Candidate(int id, string firstName, string lastName,
                    string firstEmail, string secondEmail,
                    string firstPhone, string secondPhone,
                    int attendedProjects, bool isCvReceived)
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
        }


}
  

    

