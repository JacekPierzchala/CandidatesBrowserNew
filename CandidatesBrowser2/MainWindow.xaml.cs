﻿using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            if (App.Args==null)
            {
                MessageBox.Show("Please run this application using bat file", "Warning",MessageBoxButton.OK,MessageBoxImage.Error);
                this.Close();
               

                return;
            }

           
            //Thread.Sleep(5000);
            InitializeComponent();
        }
    }
}
