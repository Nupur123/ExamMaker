using System;
using System.Collections.Generic;
using System.Data;
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
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace ExamMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool failed = false;
        public string filename;
        private TreeViewItem tree;


        XmlDocument xmlDoc = new XmlDocument();
        XmlNode rootNode = null;
        XmlNode QuestionsNode;
        int ID;
        //string XmlPath = @"C:\Users\anshulika\Documents\";

        public MainWindow()
        {
            InitializeComponent();

            LoadQuiz();
        }

        private void LoadQuiz()
        {
            //if (File.Exists(@"C:\Users\anshulika\Documents\testQuiz.xml"))
            //{
            //    xmlDoc.Load(@"C:\Users\anshulika\Documents\testQuiz.xml");
            //    rootNode = xmlDoc.DocumentElement;
            //}


        }
        private void LoadItem()
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("ns", "urn:Question-Schema");
            xmlDoc.Load(filename);
            XmlNodeList nodes = xmlDoc.SelectNodes("/ns:Quiz/ns:Details", ns);
        }
        private void LoadItemsFromTreeView(string QuestionId = null)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("ns", "urn:Question-Schema");
            xmlDoc.Load(filename);
            //ns or namespace is IMPORTANT on retrieving Values from XML file with Namespaces
            XmlNodeList nodes = xmlDoc.SelectNodes("/ns:Quiz/ns:Details", ns);
            foreach (XmlNode xn in nodes)
            {

                txtTitle.Text = xn["Title"].InnerText;
                txtSubject.Text = xn["Subject"].InnerText;
                txtTime.Text = xn["Time"].InnerText;
                string Diff = xn["Difficulty"].InnerText;
                string Course = xn["Course"].InnerText;
                switch (Diff)
                {
                    case "beginner":
                        cmbDiff.SelectedValue = "Beginner";
                        break;
                    case "intermediate":
                        cmbDiff.SelectedValue = "Intermediate";
                        break;
                    default:
                        cmbDiff.SelectedValue = "Advance";
                        break;
                }
                switch (Course)
                {
                    case "Software Developer":
                        cmbCourse.SelectedValue = "Software Developer";
                        break;
                    default:
                        break;
                }
            }
            if (QuestionId != null)
            {
                XmlNodeList GetQuestionMulti = xmlDoc.SelectNodes("/ns:Quiz/ns:Questions/ns:MultipleChoice/ns:Question[@id=" + QuestionId + "]", ns);
                foreach (XmlNode xn in GetQuestionMulti)
                {
                    txtQuestion.Text = xn["Questi"].InnerText;
                    XmlNodeList GetMultiOptions = xmlDoc.SelectNodes("/ns:Quiz/ns:Questions/ns:MultipleChoice/ns:Question[@id=" + QuestionId + "]/ns:Options", ns);
                    foreach (XmlNode xno in GetMultiOptions)
                    {
                        string[] _option = new string[5];
                        int x = 0;
                        foreach(XmlNode xno2 in xno)
                        {
                            _option[x] = xno2.InnerText;
                            if ((xno2.Attributes["Correct"] != null) && (xno2.Attributes["Correct"].Value) == "yes")
                            {
                                switch (x)
                                {
                                    case 0: rbOption1.IsChecked = true;
                                        break;
                                    case 1: rbOption2.IsChecked = true;
                                        break;
                                    case 2: rbOption3.IsChecked = true;
                                        break;
                                    case 3: rbOption4.IsChecked = true;
                                        break;
                                }
                            }
                            x++;
                        }
                        txtOption1.Text = _option[0];
                        txtOption2.Text = _option[1];
                        txtOption3.Text = _option[2];
                        txtOption4.Text = _option[3]; //temporary method!
                    }
                    
                }
            }

            //string title = test.InnerText;

        }



        // method to create and write to xml file 
        private void CreateQuiz()
        {
            ID = 1;

            xmlDoc.PrependChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", ""));

            rootNode = xmlDoc.CreateElement("Quiz");
            xmlDoc.AppendChild(rootNode);

            rootNode = xmlDoc.DocumentElement;

            XmlNode detailsNode = xmlDoc.CreateElement("Details");
            rootNode.AppendChild(detailsNode);

            XmlElement Title = xmlDoc.CreateElement("Title");
            detailsNode.AppendChild(Title);

            XmlElement Subject = xmlDoc.CreateElement("Subject");
            detailsNode.AppendChild(Subject);

            XmlElement Category = xmlDoc.CreateElement("Category");
            detailsNode.AppendChild(Category);

            XmlElement Time = xmlDoc.CreateElement("Time");
            detailsNode.AppendChild(Time);

            XmlElement Difficulty = xmlDoc.CreateElement("Difficulty");
            detailsNode.AppendChild(Difficulty);

            Title.InnerText = txtTitle.Text;
            Subject.InnerText = txtSubject.Text;
            Category.InnerText = txtCategory.Text;
            Time.InnerText = txtTime.Text;
            Difficulty.InnerText = txtDifficulty.Text;

            //trying to append questions node to root node
            QuestionsNode = xmlDoc.CreateElement("Questions");
            rootNode.AppendChild(QuestionsNode);

        }

        private void AddQuestion()
        {
            //setting object reference here before using it
            QuestionsNode = xmlDoc.DocumentElement;

            XmlElement MultipleChoiceNode = xmlDoc.CreateElement("MultipleChoice");
            QuestionsNode.AppendChild(MultipleChoiceNode);

            XmlElement Question = xmlDoc.CreateElement("Question");
            MultipleChoiceNode.AppendChild(Question);

            XmlAttribute QuestionID = xmlDoc.CreateAttribute("ID");
            QuestionID.Value = ID++.ToString();
            Question.Attributes.Append(QuestionID);
            Question.InnerText = txtQuestion.Text;

            XmlElement Options = xmlDoc.CreateElement("Options");
            MultipleChoiceNode.AppendChild(Options);

            XmlElement Option1 = xmlDoc.CreateElement("Option");
            XmlElement Option2 = xmlDoc.CreateElement("Option");
            XmlElement Option3 = xmlDoc.CreateElement("Option");
            XmlElement Option4 = xmlDoc.CreateElement("Option");

            Option1.InnerText = txtOption1.Text;
            Options.AppendChild(Option1);
            Option2.InnerText = txtOption2.Text;
            Options.AppendChild(Option2);
            Option3.InnerText = txtOption3.Text;
            Options.AppendChild(Option3);
            Option4.InnerText = txtOption4.Text;
            Options.AppendChild(Option4);

            XmlAttribute Correct = xmlDoc.CreateAttribute("Correct");
            Correct.Value = "Yes";

            if (rbOption1.IsChecked == true)
            {
                Option1.Attributes.Append(Correct);
            }

            if (rbOption2.IsChecked == true)
            {
                Option2.Attributes.Append(Correct);
            }

            if (rbOption3.IsChecked == true)
            {
                Option3.Attributes.Append(Correct);
            }

            if (rbOption4.IsChecked == true)
            {
                Option4.Attributes.Append(Correct);
            }
        }




        //if (cmbQuestionType.SelectedItem == "Fill Blanks")
        //{
        //    //setting object reference here before using it
        //    QuestionsNode = xmlDoc.DocumentElement;

        //    XmlElement FillBlanksNode = xmlDoc.CreateElement("FillBlanks");
        //    QuestionsNode.AppendChild(FillBlanksNode);

        //    XmlElement Question = xmlDoc.CreateElement("Question");
        //    FillBlanksNode.AppendChild(Question);

        //    XmlAttribute QuestionID = xmlDoc.CreateAttribute("ID");
        //    QuestionID.Value = ID++.ToString();
        //    Question.Attributes.Append(QuestionID);
        //    Question.InnerText = txtFillBlanks.Text;

        //    XmlElement Choice = xmlDoc.CreateElement("Choice");
        //    FillBlanksNode.AppendChild(Choice);

        //    XmlElement Choice1 = xmlDoc.CreateElement("Choice");
        //    XmlElement Choice2 = xmlDoc.CreateElement("Choice");

        //Choice1.InnerText = txtOption1.Text;
        //Choice.AppendChild(Choice1);
        //Choice2.InnerText = txtOption2.Text;
        //Choice.AppendChild(Choice2);
        //}

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //if (!File.Exists(@"C:\Users\anshulika\Documents\testQuiz.xml"))
            //{
            //    CreateQuiz();
            //}

            AddQuestion();

            xmlDoc.Save(@"C:\Users\anshulika\Documents\testQuiz.xml");

            // xmlDoc.Save(XmlPath + txtTitle.Text + ".xml");
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
                string xsd = AppDomain.CurrentDomain.BaseDirectory + "validator.xsd";   //this is always default
                OpenValidate OV = new OpenValidate();
                OV.ValidateXml(filename, xsd);
                if (!OV.failed)
                {
                    MessageBox.Show("File Open Status: Success");
                    status.Text = "File is valid";
                    LoadTreeView();
                    LoadItemsFromTreeView();
                }
                else
                    status.Text = OV.status;
                //Paragraph paragraph = new Paragraph();
                //paragraph.Inlines.Add(System.IO.File.ReadAllText(filename));
                //FlowDocument document = new FlowDocument(paragraph);
                //FlowDocReader.Document = document;
            }
        }
        public void LoadTreeView()
        {
            ClearAll();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            // create XmlReader object
            XmlReader reader = XmlReader.Create(filename, settings);
            tree = new TreeViewItem(); // instantiate TreeViewItem
            tree.Header = "Quiz"; // assign name to TreeViewItem
            QuizTree.Items.Add(tree); // add TreeViewItem to TreeView
            BuildTreeView BT = new BuildTreeView();
            BT.BuildTree(reader, tree); // build node and tree hierarchy
        }
        public void ClearAll()
        {
            QuizTree.Items.Clear();
            failed = false;
            //filename = "";
        }


        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item == e.OriginalSource)
            {
                if (item.Header.ToString().IndexOf("MultipleChoice") == 0) //if it is a multiple type
                    cmbQuestionType.SelectedValue = "Mutiple Choice";
                else if (item.Header.ToString().IndexOf("fillin") == 0) //if it is a fill in type
                    cmbQuestionType.SelectedValue = "Fill in the blanks";
                else if (item.Header.ToString().IndexOf("longAnswer") == 0) //if it is a long Answer type
                    cmbQuestionType.SelectedValue = "Long Answer";

                int i = item.Header.ToString().IndexOf("Question no.");
                if (i == 0)
                    LoadItemsFromTreeView(item.Header.ToString().Replace("Question no. ", ""));
                //status.Text = item.Header.ToString().Replace("Question no. ", "");  //extract the item Id

            }
            else
            {
                Console.WriteLine("Parent of selected");
                Console.WriteLine(item.Header);
                Console.WriteLine(item.Items.Count);
            }
        }

        private void cmbQuestionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string Type = (e.AddedItems[0] as ComboBoxItem).Content as string;
            if (Type != null)
            {
                switch (Type)
                {
                    case "Multiple Choice":
                        gridFillBlanks.Visibility = System.Windows.Visibility.Hidden;
                        gridTrueFalse.Visibility = System.Windows.Visibility.Hidden;
                        gridMultipleChoice.Visibility = System.Windows.Visibility.Visible;
                        break;

                    case "Fill in the blanks":
                        gridMultipleChoice.Visibility = System.Windows.Visibility.Hidden;
                        gridTrueFalse.Visibility = System.Windows.Visibility.Hidden;
                        gridFillBlanks.Visibility = System.Windows.Visibility.Visible;
                        break;

                    case "True False":
                        gridMultipleChoice.Visibility = System.Windows.Visibility.Hidden;
                        gridFillBlanks.Visibility = System.Windows.Visibility.Hidden;
                        gridTrueFalse.Visibility = System.Windows.Visibility.Visible;
                        break;

                }
            }
        }

        private void cmbQuestionType_DropDownClosed(object sender, EventArgs e)
        {
            
        }
    }
}
