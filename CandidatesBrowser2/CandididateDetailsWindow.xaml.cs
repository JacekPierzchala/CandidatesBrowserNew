using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CandidatesBrowser2
{
    /// <summary>
    /// Interaction logic for CandididateDetailsWindow.xaml
    /// </summary>
    public partial class CandididateDetailsWindow : Window
    {
       public  Candidate Candidatenew { get; set; }
        public static CandididateDetailsWindow can { get; set; }
        public ObservableCollection<CandidateDetails> CandidateDetailsCollection { get; set; }

        public CandididateDetailsWindow(Candidate candidate)
        {
           
            InitializeComponent();
            Candidatenew = new Candidate();
            CandidateDetailsCollection = new ObservableCollection<CandidateDetails>();
            
            

            Candidatenew = candidate;
            
           
            CandidateDetailsCollection =CandidateDetails.CreateAreaCollection(GlobalFunctions.GetTableFromServerArgs("GET_CANDIDATE_DETAILS", "@CANDIDATE_ID-" + Candidatenew.ID));
            firstNametxt.Text = Candidatenew.FirstName;
            lastNametxt.Text = Candidatenew.LastName;
            firstPhonetxt.Text = Candidatenew.FirstPhone;
            secondPhonetxt.Text = Candidatenew.SecondPhone;
            firstEmailtxt.Text = Candidatenew.FirstEmail;
            secondEmailtxt.Text = Candidatenew.SecondEmail;
            projectsListbox.ItemsSource = CandidateDetailsCollection;
            positionsLst.ItemsSource = CandidateDetailsCollection;


        }
    }
}
