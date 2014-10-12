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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace ExamMaker
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private bool failed = false;
        public string filename;
        public Window1()
        {
            InitializeComponent();
            //status.Text = System.IO.Directory.GetCurrentDirectory();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".xml";
            dlg.Filter = "Xml File (.xml)|*.xml";
            

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                status.Text = "";
                failed = false;
                filename = dlg.FileName;
                string xsd = AppDomain.CurrentDomain.BaseDirectory + "validator.xsd";
                OpenValidate OV = new OpenValidate();
                OV.ValidateXml(filename, xsd);
                if(!OV.failed)
                {
                    MessageBox.Show("File Validation Status: Success");
                    status.Text = "File is valid";
                }
                //Paragraph paragraph = new Paragraph();
                //paragraph.Inlines.Add(System.IO.File.ReadAllText(filename));
                //FlowDocument document = new FlowDocument(paragraph);
                //FlowDocReader.Document = document;
            }
        }
    }
}
