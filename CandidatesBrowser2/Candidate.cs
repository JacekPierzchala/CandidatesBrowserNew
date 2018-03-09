﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser2
{
    public class Candidate
    {
        public int ID { get; set; }
        public  string FirstName { get; set; }
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

    public class Status
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Definition { get; set; }
        public bool Deleted { get; set; }

        public Status(int id,string description, string definition, bool deleted)
        {
            this.ID = id;
            this.Description = description;
            this.Definition = definition;
            this.Deleted = deleted;
        }
        
    }


}
