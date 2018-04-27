using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;

namespace CandidatesBrowser2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static String[] Args;
       

         void AppStartUp(object sender, StartupEventArgs e)
        {
            if (e.Args.Length>0)
            {
                Args = e.Args;
               
            }
           
            SplashScreen sc = new SplashScreen("cv_image.png");

            sc.Show(false, true);
            sc.Close(TimeSpan.FromMilliseconds(1500));
        }
    }
}
