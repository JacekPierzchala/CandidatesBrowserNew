using System;
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

        public Candidate(int id, string firstName, string lastName,
                string firstEmail, string secondEmail, string firstPhone, string secondPhone)
        {
            this.ID = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.FirstEmail = firstEmail;
            this.SecondEmail = secondEmail;
            this.FirstPhone = firstPhone;
            this.SecondPhone = secondPhone;
        }

    }
}
