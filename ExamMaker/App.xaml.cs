using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ExamMaker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 1)
            {
                string argument = e.Args[0].ToString();
                AddQuestion.arg = argument;
               // MessageBox.Show("Now opening file: \n\n" + e.Args[0]);
            }
            MainWindow wnd = new MainWindow();
           
                
            //wnd.Title = "Welcome to Exam Maker";
            wnd.Show();
        }

    }
}
