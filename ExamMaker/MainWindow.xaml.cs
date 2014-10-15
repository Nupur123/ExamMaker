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
        const string xmlNS = "urn:Question-Schema";
        private bool failed = false;
        private bool isNew = false;
        public string filename;
        public string ItemID;
        private TreeViewItem tree;
        private string FilePath = AppDomain.CurrentDomain.BaseDirectory;
        private string NewFilePath = null;

        XmlDocument xmlDoc = new XmlDocument();
        XmlNode rootNode = null;
        XmlNode QuestionsNode;
        private int ID = 0;

        
        public MainWindow()
        {
            InitializeComponent();
            
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
                XmlNodeList GetQuestionMulti = xmlDoc.SelectNodes("/ns:Quiz/ns:Questions/ns:MultipleChoice/ns:Question[@ID=" + QuestionId + "]", ns);
                foreach (XmlNode xn in GetQuestionMulti)
                {
                    txtQuestion.Text = xn["Questi"].InnerText;
                    XmlNodeList GetMultiOptions = xmlDoc.SelectNodes("/ns:Quiz/ns:Questions/ns:MultipleChoice/ns:Question[@ID=" + QuestionId + "]/ns:Options", ns);
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
            isNew = true;
            xmlDoc.PrependChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", ""));
            XmlElement rootNode = xmlDoc.CreateElement("Quiz",xmlNS);
            rootNode.SetAttribute("QuizId", "1");
            rootNode.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            //rootNode.SetAttribute("xmlns", "urn:Question-Schema");
            xmlDoc.AppendChild(rootNode);
            rootNode = xmlDoc.DocumentElement;

            XmlNode detailsNode = xmlDoc.CreateElement("Details", xmlNS);
            rootNode.AppendChild(detailsNode);

            XmlElement Title = xmlDoc.CreateElement("Title", xmlNS);
            detailsNode.AppendChild(Title);

            XmlElement Subject = xmlDoc.CreateElement("Subject", xmlNS);
            detailsNode.AppendChild(Subject);

            XmlElement Category = xmlDoc.CreateElement("Course", xmlNS);
            detailsNode.AppendChild(Category);

            XmlElement Time = xmlDoc.CreateElement("Time", xmlNS);
            detailsNode.AppendChild(Time);

            XmlElement Difficulty = xmlDoc.CreateElement("Difficulty", xmlNS);
            detailsNode.AppendChild(Difficulty);

            Title.InnerText = txtTitle.Text;
            Subject.InnerText = txtSubject.Text;
            Category.InnerText = cmbCourse.SelectedValue.ToString();
            Time.InnerText = txtTime.Text;
            Difficulty.InnerText = cmbDiff.SelectedValue.ToString();

            //trying to append questions node to root node
            QuestionsNode = xmlDoc.CreateElement("Questions", xmlNS);
            rootNode.AppendChild(QuestionsNode);

            XmlElement MultipleChoiceNode = xmlDoc.CreateElement("MultipleChoice", xmlNS);
            QuestionsNode.AppendChild(MultipleChoiceNode);

            XmlElement FillBlanksNode = xmlDoc.CreateElement("FillBlanks", xmlNS);
            QuestionsNode.AppendChild(FillBlanksNode);

            XmlElement TrueFalseNode = xmlDoc.CreateElement("TrueFalse", xmlNS);
            QuestionsNode.AppendChild(TrueFalseNode);

            XmlElement longAnswer = xmlDoc.CreateElement("longAnswer", xmlNS);
            QuestionsNode.AppendChild(longAnswer);

        }

        private void AddMultipleChoice()
        { //im in editing the questions and the id number should be fix!!!
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("ns", "urn:Question-Schema");
            //ns or namespace is IMPORTANT on retrieving Values from XML file with Namespaces
            XmlNodeList nodes = xmlDoc.SelectNodes("/ns:Quiz/ns:Details", ns);

            //setting object reference here before using it
            //QuestionsNode = xmlDoc.DocumentElement;
        
            //XmlNodeList xnList = xmlDoc.SelectNodes("/Quiz/Questions/MultipleChoice");
           XmlNode Multi =  xmlDoc.SelectSingleNode("/ns:Quiz/ns:Questions/ns:MultipleChoice",ns);

           XmlElement Question = xmlDoc.CreateElement("Question", xmlNS);
            Multi.AppendChild(Question);

            XmlAttribute QuestionID = xmlDoc.CreateAttribute("ID");
            if (!isNew)
                QuestionID.Value = (AddQuestion.CielingId +1).ToString();       
            else
                QuestionID.Value = ID++.ToString();
            Question.Attributes.Append(QuestionID);
            //Question.InnerText = txtQuestion.Text;

            XmlNode Questio = xmlDoc.SelectSingleNode("/ns:Quiz/ns:Questions/ns:MultipleChoice/ns:Question",ns);

            XmlElement Questi = xmlDoc.CreateElement("Questi", xmlNS);
            Questi.InnerText = txtQuestion.Text;
            Questio.AppendChild(Questi);

            XmlElement Options = xmlDoc.CreateElement("Options", xmlNS);
            Questio.AppendChild(Options);

            XmlElement Option1 = xmlDoc.CreateElement("Option", xmlNS);
            XmlElement Option2 = xmlDoc.CreateElement("Option", xmlNS);
            XmlElement Option3 = xmlDoc.CreateElement("Option", xmlNS);
            XmlElement Option4 = xmlDoc.CreateElement("Option", xmlNS);

            Option1.InnerText = txtOption1.Text;
            Options.AppendChild(Option1);
            Option2.InnerText = txtOption2.Text;
            Options.AppendChild(Option2);
            Option3.InnerText = txtOption3.Text;
            Options.AppendChild(Option3);
            Option4.InnerText = txtOption4.Text;
            Options.AppendChild(Option4);

            XmlAttribute Correct = xmlDoc.CreateAttribute("Correct");
            Correct.Value = "yes";

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
            Question.AppendChild(Questi);
            Question.AppendChild(Options);
            
        }

        private void AddFillBlanks()
        {
            //setting object reference here before using it
            QuestionsNode = xmlDoc.DocumentElement;

            XmlNodeList xnList = xmlDoc.SelectNodes("/Quiz/Questions/FillBlanks");

            XmlNode xn = xmlDoc.SelectSingleNode("/Quiz/Questions/FillBlanks");

            XmlElement Question = xmlDoc.CreateElement("Question");
            xn.AppendChild(Question);

            XmlAttribute QuestionID = xmlDoc.CreateAttribute("ID");
            QuestionID.Value = ID++.ToString();
            Question.Attributes.Append(QuestionID);
            Question.InnerText = txtFillBlanks.Text;

            XmlElement Choice = xmlDoc.CreateElement("Choice");
            xn.AppendChild(Choice);

            XmlElement Choice1 = xmlDoc.CreateElement("Choice");
            XmlElement Choice2 = xmlDoc.CreateElement("Choice");
            XmlElement Choice3 = xmlDoc.CreateElement("Choice");
            XmlElement Choice4 = xmlDoc.CreateElement("Choice");

            Choice1.InnerText = txtChoice1.Text;
            Choice.AppendChild(Choice1);
            Choice2.InnerText = txtChoice2.Text;
            Choice.AppendChild(Choice2);
            Choice3.InnerText = txtChoice3.Text;
            Choice.AppendChild(Choice3);
            Choice4.InnerText = txtChoice4.Text;
            Choice.AppendChild(Choice4);

            XmlAttribute Correct = xmlDoc.CreateAttribute("Correct");
            Correct.Value = "Yes";

            if (cboChoice1.IsChecked == true)
                Choice1.Attributes.Append(Correct);
            if (cboChoice2.IsChecked == true)
                Choice2.Attributes.Append(Correct);
            if (cboChoice3.IsChecked == true)
                Choice3.Attributes.Append(Correct);
            if (cboChoice4.IsChecked == true)
                Choice4.Attributes.Append(Correct);
        }

        private void AddTrueFalse()
        {
            QuestionsNode = xmlDoc.DocumentElement;
            XmlNodeList xnList = xmlDoc.SelectNodes("/Quiz/Questions/TrueFalse");
            XmlNode xn = xmlDoc.SelectSingleNode("/Quiz/Questions/TrueFalse");
            XmlElement Question = xmlDoc.CreateElement("Question");
            xn.AppendChild(Question);

            XmlAttribute QuestionID = xmlDoc.CreateAttribute("ID");
            QuestionID.Value = ID++.ToString();
            Question.Attributes.Append(QuestionID);
            Question.InnerText = txtTrueFalse.Text;

            XmlElement Answer = xmlDoc.CreateElement("Answer");
            xn.AppendChild(Answer);

            XmlAttribute True = xmlDoc.CreateAttribute("True");
            True.Value = "Yes";

            XmlAttribute False = xmlDoc.CreateAttribute("False");
            False.Value = "Yes";

            if (rbFalse.IsChecked == true)
            {
                Answer.Attributes.Append(False);
            }
        }

        // this button adds Multiple choice question
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(NewFilePath))
                CreateQuiz();
            else
                isNew = false;
            if (ItemID == "1")
                AddMultipleChoice();
                   
            xmlDoc.Save(NewFilePath);
            filename = NewFilePath;
            LoadTreeView();
        }

        // this button adds True False Question
        private void btnTrueFalse_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(NewFilePath))
            {
                CreateQuiz();
            }

            AddTrueFalse();
            xmlDoc.Save(NewFilePath);
            filename = NewFilePath;
            LoadTreeView();
        }

        // this button adds Fill Blanks Question
        private void btnFillBlanks_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(NewFilePath))
            {
                CreateQuiz();
            }

            AddFillBlanks();
            xmlDoc.Save(NewFilePath);
            filename = NewFilePath;
            LoadTreeView();
        }


        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".xqz";
            dlg.Filter = "Exam File (.xqz)|*.xqz";
            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                status.Text = "";
                failed = false;
                filename = dlg.FileName;
                
                string xsd = FilePath + "validator.xsd";   //this is always default
                OpenValidate OV = new OpenValidate();
                OV.ValidateXml(filename, xsd);
                if (!OV.failed)
                {
                    MessageBox.Show("File Open Status: Success");
                    status.Text = "File is valid";
                    NewFilePath = filename;
                    LoadTreeView();
                    LoadItemsFromTreeView();
                    
                }
                else
                    status.Text = OV.status;
                //Paragraph paragraph = new Paragraph();
                //paragraph.Inlines.Add(System.IO.File.ReadAllText(filename));
                //FlowDocument document = new FlowDocument(paragraph);
                //FlowDocReader.Document = document;

                gridQuizSummary.Visibility = System.Windows.Visibility.Visible;
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
            QuizItemCount.Content = AddQuestion.CielingId;        
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
            //if (item == e.OriginalSource)
            if (item !=null)
            {
                if (item.Header.ToString().IndexOf("MultipleChoice") == 0) //if it is a multiple type
                    cmbQuestionType.SelectedValue = "Multiple Choice";
                else if (item.Header.ToString().IndexOf("fillin") == 0) //if it is a fill in type
                    cmbQuestionType.SelectedValue = "Fill in the blanks";
                else if (item.Header.ToString().IndexOf("longAnswer") == 0) //if it is a long Answer type
                    cmbQuestionType.SelectedValue = "Long Answer";

                int i = item.Header.ToString().IndexOf("Question no.");
                if (i == 0)
                    ID = Convert.ToInt16(item.Header.ToString().Replace("Question no. ", ""));
                    LoadItemsFromTreeView(ID.ToString());
               
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
                        gridMultipleChoice.Visibility = System.Windows.Visibility.Visible;
                        gridFillBlanks.Visibility = System.Windows.Visibility.Hidden;
                        gridTrueFalse.Visibility = System.Windows.Visibility.Hidden;
                        break;

                    case "Fill in the blanks":
                        gridFillBlanks.Visibility = System.Windows.Visibility.Visible;
                        gridMultipleChoice.Visibility = System.Windows.Visibility.Hidden;
                        gridTrueFalse.Visibility = System.Windows.Visibility.Hidden;  
                        break;

                    case "True False":
                        gridTrueFalse.Visibility = System.Windows.Visibility.Visible;
                        gridMultipleChoice.Visibility = System.Windows.Visibility.Hidden;
                        gridFillBlanks.Visibility = System.Windows.Visibility.Hidden;
                        break;
                }
            }

            // retrieving UID from selected ComboBox Item and saving it in a public string
            var comboBox = sender as ComboBox;
            if (null != comboBox)
            {
                var item = comboBox.SelectedItem as ComboBoxItem;
                if (null != item)
                {
                   ItemID = item.Uid;
                }
            }

        }

        private void New_Click(object sender, RoutedEventArgs e)
        {        
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".xqz";
            dlg.Filter = "Exam File (.xqz)|*.xqz";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                NewFilePath = filename;
                this.filename = NewFilePath;
            }

            gridQuizSummary.Visibility = System.Windows.Visibility.Visible;
        }

       
       

       
    }
}
