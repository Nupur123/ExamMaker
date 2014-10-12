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
            Window1 wnd = new Window1();
            if (e.Args.Length == 1)
                MessageBox.Show("Now opening file: \n\n" + e.Args[0]);
            wnd.Show();
        }

    }
}
