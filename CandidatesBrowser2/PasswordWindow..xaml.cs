using System;
using System.Collections.Generic;
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
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public bool correct = false;
        
        public PasswordWindow()
        {
            InitializeComponent();           
        }

     
        private void ConfirmPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (Passwordfield.Password.ToString()== MainWindow.Key)
            {
                correct = true;
                this.Close();
                MainWindow.MainWindStatic.Show();
            }
            else
            {
                
                if (MessageBox.Show("Provided password is not correct.", "Incorrect password", MessageBoxButton.OKCancel)== MessageBoxResult.Cancel)
                {
                    this.Close();                                      
                }
               
                
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //this.Close();
            if (!correct)
            {
                System.Windows.Application.Current.Shutdown();
                Environment.Exit(0);
                return;
            }

               
        }
    }
}
