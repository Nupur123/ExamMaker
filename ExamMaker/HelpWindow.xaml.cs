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

namespace ExamMaker
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
        }

        // Question- How To Add a New File/ Quiz?
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            gridAnswer.Visibility = System.Windows.Visibility.Visible;

            lblAnswer.Content = "To Add or Create a New File for your Quiz, please follow these steps-" +
            "\r\n" + "->  Go to File Menu on Top Left Corner of App Page" + "\r\n" + "->  Click on 'New'" + "\r\n" +
            "->  A File Dialog Window will pop up. Browse path where you want to save your file" +
            "\r\n" + "->  Give it a File Name and Click Save Button when you are done!" + "\r\n" +
            "These steps will create a New File for you and give you access to add quiz data to it.";
        }

        //  How To Open an Existing File/ Quiz?
        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            gridAnswer.Visibility = System.Windows.Visibility.Visible;

            lblAnswer.Content = "To Open an Existing File/ Quiz, please follow these steps-" +
            "\r\n" + "->  Go to File Menu on Top Left Corner of App Page" + "\r\n" + "->  Click on 'Open'" + "\r\n" +
            "->  A File Dialog Window will pop up. Browse path of file that you want to open" +  "\r\n" +
            "->  The selected existing file will open up if it is in correct format.";
        }

        // How To Add More Questions an Existing File/ Quiz?
        private void Hyperlink_Click_2(object sender, RoutedEventArgs e)
        {
            gridAnswer.Visibility = System.Windows.Visibility.Visible;

            lblAnswer.Content = "To Add More Questions to an existing file/ quiz, please follow these steps-" +
            "\r\n" + "->  You'll have to Open an Existing File from the File Menu (See 'How to Open Existing File')" +
            "\r\n" + "->  Once the file is open, it will give you access to the add panel where you can add more questions.";

        }

        //  What does Tree View Navigation on Main App Page do?
        private void Hyperlink_Click_3(object sender, RoutedEventArgs e)
        {
            gridAnswer.Visibility = System.Windows.Visibility.Visible;

            lblAnswer.Content = "Tree View Navigation is located on the left side of app page." + "\r\n" +
            "You can open an existing file from File Menu to browse file contents in the tree view." + "\r\n" +
            "Tree View functionality is meant to provide easy navigation and browsing of questions." + "\r\n" +
            "It is also ideal to browse and select specific questions to edit them.";
        }

        //  5. How do I EDIT or UPDATE existing questions in a Quiz?
        private void Hyperlink_Click_4(object sender, RoutedEventArgs e)
        {
            gridAnswer.Visibility = System.Windows.Visibility.Visible;
            lblAnswer.Content = "To EDIT or UPDATE existing questions in a quiz, please follow these steps-" + "\r\n" +
            "->  You'll have to Open an Existing File from the File Menu (See 'How to Open Existing File')" +
             "\r\n" + "->  Once the file is open, the Tree View Navigation panel on left side of app will show up." +
             "\r\n" + "->  Browse through the treeview and select the question you want to edit or update." +
             "\r\n" + "->  When you select a specific question, edit panel with values in textboxes will load up." +
             "\r\n" + "->  The panel defaults to READ-ONLY and to make changes your have to click 'EDIT' button." +
             "\r\n" + "->  Clicking the EDIT button will allow you to make any changes that you want." +
             "\r\n" + "->  Click 'Submit Question' to save your changes when you are done.";
        }

        //   6. How do I DELETE existing questions in a Quiz?
        private void Hyperlink_Click_5(object sender, RoutedEventArgs e)
        {
            gridAnswer.Visibility = System.Windows.Visibility.Visible;

            lblAnswer.Content = "To DELETE an existing questions in a quiz, please follow these steps-" + "\r\n" +
         "->  You'll have to Open an Existing File from the File Menu (See 'How to Open Existing File')" +
          "\r\n" + "->  Once the file is open, the Tree View Navigation panel on left side of app will show up." +
          "\r\n" + "->  Browse through the treeview and select the question you want to DELETE." +
          "\r\n" + "->  When you select a specific question, edit/ delete panel with values in textboxes will load up." +
          "\r\n" + "->  Click on 'DELETE' button to delete the selected question from quiz.";
        }
    }
}
