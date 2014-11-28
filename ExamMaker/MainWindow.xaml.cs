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
using System.Diagnostics;

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
        private bool isAddNew = false;
        private bool isEdit = false;
        private bool isView = false;
        public string filename;
        public string ItemID;
        private TreeViewItem tree;
        private string FilePath = AppDomain.CurrentDomain.BaseDirectory;
        private string NewFilePath = null;
        XmlDocument xmlSources = new XmlDocument();
        private string FileSource = AppDomain.CurrentDomain.BaseDirectory + "SourceXML.xml";

        XmlDocument xmlDoc = new XmlDocument();
        XmlNode rootNode = null;
        XmlNode QuestionsNode;
        private int ID = 0;
        private int QuizId = 0;
        private string FillInOptionOld;

        public MainWindow()
        {
            InitializeComponent();
            filename = AddQuestion.arg;
            if (filename != null && filename != "")
                LoadFileAndValidate();

            //if (!string.IsNullOrEmpty(FileSource))
            //{
            //    provider.Source = new Uri(FileSource);
            //}
            loadCourses();

        }
        private void loadCourses()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(FileSource);
            XmlNodeList Courses = xDoc.GetElementsByTagName("Course");
            for (int i = 0; i < Courses.Count; i++)
            {
                cmbCourse.Items.Add(Courses[i].Attributes["Name"].InnerText);
            }

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
                txtTitle.IsReadOnly = true;
                txtSubject.IsReadOnly = true;
                txtTime.IsReadOnly = true;
                cmbDiff.IsEnabled = false;
                cmbCourse.IsEnabled = false;
                txtTitle.Text = xn["Title"].InnerText;
                txtSubject.Text = xn["Subject"].InnerText;
                txtTime.Text = xn["Time"].InnerText;
                string Diff = xn["Difficulty"].InnerText;
                string Course = xn["Course"].InnerText;
                switch (Diff)
                {
                    case "Beginner":
                        cmbDiff.SelectedValue = "Beginner";
                        break;
                    case "Intermediate":
                        cmbDiff.SelectedValue = "Intermediate";
                        break;
                    default:
                        cmbDiff.SelectedValue = "Advanced";
                        break;
                }

                //xmlSources.Load(xsd);
                cmbCourse.SelectedValue = Course;
                //switch (Course)
                //{
                //    case "Software and Database Developer":
                //        cmbCourse.SelectedValue = "Software and Database Developer";
                //        break;
                //    default:
                //        break;
                //}
            }
            if (QuestionId != null)
            {
                txtQuestion.IsReadOnly = true;
                txtOption1.IsReadOnly = true;
                txtOption2.IsReadOnly = true;
                txtOption3.IsReadOnly = true;
                txtOption4.IsReadOnly = true;

                //True False
                txtTrueFalse.IsReadOnly = true;
                GridQuestionType.Visibility = System.Windows.Visibility.Hidden;

                //Fill in the Blanks
                lbCorrectAnswers.Items.Clear();
                lbOtherOptions.Items.Clear();
                txtFillBlanks.IsReadOnly = true;
                btnAddFillinCorrectAnswers.Visibility = System.Windows.Visibility.Hidden;
                btnRemoveCorrectAnswers.Visibility = System.Windows.Visibility.Hidden;
                btnAddFillinOptions.Visibility = System.Windows.Visibility.Hidden;
                btnRemoveFillinOptions.Visibility = System.Windows.Visibility.Hidden;
                txtOptionFillin.Visibility = System.Windows.Visibility.Hidden;
                btnSubmitFillin.Visibility = System.Windows.Visibility.Hidden;
                GridQuestionType.Visibility = System.Windows.Visibility.Hidden;
                btnEditFillin.Visibility = System.Windows.Visibility.Visible;
                btnDeleteFillin.Visibility = System.Windows.Visibility.Visible;


                //loading for Multiple Type
                XmlNodeList GetQuestionMulti = xmlDoc.SelectNodes("/ns:Quiz/ns:Questions/ns:MultipleChoice/ns:Question[@ID=" + QuestionId + "]", ns);
                XmlNodeList GetTrueFalse = xmlDoc.SelectNodes("/ns:Quiz/ns:Questions/ns:TrueFalse/ns:Question[@ID=" + QuestionId + "]", ns);
                XmlNodeList GetFillIn = xmlDoc.SelectNodes("/ns:Quiz/ns:Questions/ns:FillBlanks/ns:Question[@ID=" + QuestionId + "]", ns);
                int a = GetQuestionMulti.Count;
                int b = GetTrueFalse.Count;
                int c = GetFillIn.Count;
                if (a != 0 && b == 0 && c == 0)
                {//it is a Multiple
                    foreach (XmlNode xn in GetQuestionMulti)
                    {
                        txtQuestion.Text = xn["Questi"].InnerText;
                        XmlNodeList GetMultiOptions = xmlDoc.SelectNodes("/ns:Quiz/ns:Questions/ns:MultipleChoice/ns:Question[@ID=" + QuestionId + "]/ns:Options", ns);
                        //TrueFalse;
                        foreach (XmlNode xno in GetMultiOptions)
                        {
                            string[] _option = new string[5];
                            int x = 0;
                            foreach (XmlNode xno2 in xno)
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
                else if (a == 0 && b != 0 && c == 0)
                {//if it is TrueFalse;
                    //New code!!!
                    foreach (XmlNode xn in GetTrueFalse)
                    {
                        txtTrueFalse.Text = xn["Questi"].InnerText;
                        XmlNodeList GetTF = xmlDoc.SelectNodes("/ns:Quiz/ns:Questions/ns:TrueFalse/ns:Question[@ID=" + QuestionId + "]", ns);
                        //TrueFalse;
                        foreach (XmlNode xno in GetTF)
                        {
                            string[] Answer = new string[3];
                            int x = 0;
                            foreach (XmlNode xno2 in xno)
                            {
                                Answer[x] = xno2.InnerText;
                                if (xno2.Name == "Answer")
                                {
                                    switch (x)
                                    {
                                        case 0: rbTrue.IsChecked = true;
                                            break;
                                        case 1: rbFalse.IsChecked = true;
                                            break;
                                    }
                                }
                                x++;
                            }
                        }
                    }
                }
                else if (a == 0 && b == 0 && c != 0)
                {
                    lbCorrectAnswers.Items.Clear();
                    lbOtherOptions.Items.Clear();
                    //if it is Fill in the Blanks
                    foreach (XmlNode xn in GetFillIn)
                    {
                        txtFillBlanks.Text = xn["Questi"].InnerText;
                        XmlNodeList GetFillinBlanks = xmlDoc.SelectNodes("/ns:Quiz/ns:Questions/ns:FillBlanks/ns:Question[@ID=" + QuestionId + "]/ns:Options", ns);
                        //
                        foreach (XmlNode xno2 in GetFillinBlanks)
                        {
                            foreach (XmlNode xno3 in xno2)
                            {
                                if ((xno3.Attributes["Correct"] != null) && (xno3.Attributes["Correct"].Value) == "yes")
                                {
                                    lbCorrectAnswers.Items.Add(xno3.InnerText);
                                }
                                else
                                {
                                    lbOtherOptions.Items.Add(xno3.InnerText);
                                }
                            }
                        }
                    }
                }

            }
        }
        // method to create and write to xml file 
        private void CreateQuiz()
        {
            xmlDoc.RemoveAll();
            ID = 1;
            GenerateQuizid();
            isNew = true;
            //xmlDoc.PrependChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", ""));
            xmlDoc.PrependChild(xmlDoc.CreateXmlDeclaration("1.0", null, ""));
            XmlElement rootNode = xmlDoc.CreateElement("Quiz", xmlNS);
            rootNode.SetAttribute("QuizId", QuizId.ToString());
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

            QuestionsNode = xmlDoc.CreateElement("Questions", xmlNS);
            rootNode.AppendChild(QuestionsNode);

            XmlElement MultipleChoiceNode = xmlDoc.CreateElement("MultipleChoice", xmlNS);
            QuestionsNode.AppendChild(MultipleChoiceNode);

            XmlElement FillBlanksNode = xmlDoc.CreateElement("FillBlanks", xmlNS);
            QuestionsNode.AppendChild(FillBlanksNode);

            XmlElement TrueFalseNode = xmlDoc.CreateElement("TrueFalse", xmlNS);
            QuestionsNode.AppendChild(TrueFalseNode);

            //XmlElement longAnswer = xmlDoc.CreateElement("longAnswer", xmlNS);
            //QuestionsNode.AppendChild(longAnswer);
        }
        private void AddMultipleChoice()
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("ns", "urn:Question-Schema");
            //ns or namespace is IMPORTANT on retrieving Values from XML file with Namespaces
            XmlNodeList nodes = xmlDoc.SelectNodes("/ns:Quiz/ns:Details", ns);
            XmlNode Multi = xmlDoc.SelectSingleNode("/ns:Quiz/ns:Questions/ns:MultipleChoice", ns);

            XmlElement Question = xmlDoc.CreateElement("Question", xmlNS);
            Multi.AppendChild(Question);

            XmlAttribute QuestionID = xmlDoc.CreateAttribute("ID");
            if (!isNew)//not newly create file
                QuestionID.Value = (AddQuestion.CielingId + 1).ToString();
            else//new File
                QuestionID.Value = (ID++).ToString();
            Question.Attributes.Append(QuestionID);

            XmlNode Questio = xmlDoc.SelectSingleNode("/ns:Quiz/ns:Questions/ns:MultipleChoice/ns:Question", ns);

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
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("ns", "urn:Question-Schema");
            QuestionsNode = xmlDoc.DocumentElement;

            XmlNode xn = xmlDoc.SelectSingleNode("/ns:Quiz/ns:Questions/ns:FillBlanks", ns);

            XmlElement Question = xmlDoc.CreateElement("Question", xmlNS);
            xn.AppendChild(Question);

            XmlAttribute QuestionID = xmlDoc.CreateAttribute("ID");
            if (!isNew)//not newly create file
                QuestionID.Value = (AddQuestion.CielingId + 1).ToString();
            else//new File
                QuestionID.Value = (ID++).ToString();
            Question.Attributes.Append(QuestionID);

            XmlNode Questio = xmlDoc.SelectSingleNode("/ns:Quiz/ns:Questions/ns:FillBlanks/ns:Question", ns);
            XmlElement Questi = xmlDoc.CreateElement("Questi", xmlNS);
            Questi.InnerText = txtFillBlanks.Text;
            Questio.AppendChild(Questi);

            XmlElement Options = xmlDoc.CreateElement("Options", xmlNS);
            Questio.AppendChild(Options);

            //XmlElement OptionCorrect = xmlDoc.CreateElement("Option", xmlNS);
            //XmlElement OtherOptions = xmlDoc.CreateElement("Option", xmlNS);




            for (int i = 0; i < lbCorrectAnswers.Items.Count; i++)
            {
                XmlElement OptionCorrect = xmlDoc.CreateElement("Option", xmlNS);
                OptionCorrect.InnerText = lbCorrectAnswers.Items[i].ToString();
                Options.AppendChild(OptionCorrect);

                XmlAttribute Correct = xmlDoc.CreateAttribute("Correct");
                Correct.Value = "yes";
                OptionCorrect.Attributes.Append(Correct);
            }

            for (int i = 0; i < lbOtherOptions.Items.Count; i++)
            {
                XmlElement OtherOptions = xmlDoc.CreateElement("Option", xmlNS);
                OtherOptions.InnerText = lbOtherOptions.Items[i].ToString();
                Options.AppendChild(OtherOptions);
            }

            Question.AppendChild(Questi);
            Question.AppendChild(Options);

            txtFillBlanks.Text = "";
            txtOptionFillin.Text = "";
            lbCorrectAnswers.Items.Clear();
            lbOtherOptions.Items.Clear();
        }
        private void AddTrueFalse()
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("ns", "urn:Question-Schema");

            QuestionsNode = xmlDoc.DocumentElement;
            XmlNode xn = xmlDoc.SelectSingleNode("/ns:Quiz/ns:Questions/ns:TrueFalse", ns);
            XmlElement Question = xmlDoc.CreateElement("Question", xmlNS);
            xn.AppendChild(Question);

            XmlAttribute QuestionID = xmlDoc.CreateAttribute("ID");
            if (!isNew)//not newly create file
                QuestionID.Value = (AddQuestion.CielingId + 1).ToString();
            else//new File
                QuestionID.Value = ID++.ToString();
            Question.Attributes.Append(QuestionID);

            XmlNode Questio = xmlDoc.SelectSingleNode("/ns:Quiz/ns:Questions/ns:TrueFalse/ns:Question", ns);
            XmlElement Questi = xmlDoc.CreateElement("Questi", xmlNS);
            Questi.InnerText = txtTrueFalse.Text;
            Questio.AppendChild(Questi);

            XmlElement Answer = xmlDoc.CreateElement("Answer", xmlNS);
            xn.AppendChild(Answer);
            if (rbFalse.IsChecked == true)
                Answer.InnerText = "False";
            else
                Answer.InnerText = "True";
            Question.AppendChild(Questi);
            Question.AppendChild(Answer);
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckErrors("multi"))
            {
                if (!File.Exists(NewFilePath))
                    CreateQuiz();
                else
                    isNew = false;
                if (ItemID == "1")
                    if (isEdit)
                        UpdateQuestion();
                    else
                        AddMultipleChoice();
                if (btnEdit.IsEnabled)
                {
                    MessageBox.Show("Your Question has been saved to the Tree View");
                }
                else
                    AddMultipleChoice();

                XmlTextWriter wr = new XmlTextWriter(NewFilePath, null);
                wr.Formatting = Formatting.None; // no new line spaces;

                xmlDoc.Save(wr);
                filename = NewFilePath;
                wr.Close();
                LoadTreeView();
            }
        }
        // this button adds Fill Blanks Question
        private void btnSubmitFillin_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(NewFilePath))
            {
                CreateQuiz();
            }
            else
                isNew = false;
            if (isEdit)
                UpdateFillinQuestion();
            else
            {
                AddFillBlanks();
            }



            XmlTextWriter wr = new XmlTextWriter(NewFilePath, null);
            wr.Formatting = Formatting.None; // no new line spaces;

            xmlDoc.Save(wr);
            filename = NewFilePath;
            wr.Close();
            LoadTreeView();
        }
        private void UpdateQuestion()
        {
            xmlDoc.Load(filename);
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("ns", "urn:Question-Schema");

            XmlNodeList nodes = xmlDoc.SelectNodes("/ns:Quiz/ns:Questions/ns:MultipleChoice/ns:Question[@ID=" + ID + "]", ns);
            foreach (XmlNode xn in nodes)
            {
                XmlNode NewQuesti = xmlDoc.CreateElement("Questi", xmlNS);
                NewQuesti.InnerText = txtQuestion.Text;
                xn.ReplaceChild(NewQuesti, xn["Questi"]);
                foreach (XmlNode xn2 in xn)
                {
                    if (xn2.Name == "Options")
                    {
                        for (int Counter = 0; Counter <= 3; Counter++)
                        {
                            XmlNodeList xna = xn2.ChildNodes;
                            XmlNode ab = xna.Item(Counter);
                            XmlNode y = xna.Item(Counter);
                            XmlAttribute Correct = xmlDoc.CreateAttribute("Correct");
                            Correct.Value = "yes";
                            XmlNode Option = xmlDoc.CreateElement("Option", xmlNS);
                            switch (Counter)
                            {
                                case 0: Option.InnerText = txtOption1.Text;
                                    if (rbOption1.IsChecked == true)
                                        Option.Attributes.Append(Correct);
                                    break;
                                case 1: Option.InnerText = txtOption2.Text;
                                    if (rbOption2.IsChecked == true)
                                        Option.Attributes.Append(Correct);
                                    break;
                                case 2: Option.InnerText = txtOption3.Text;
                                    if (rbOption3.IsChecked == true)
                                        Option.Attributes.Append(Correct);
                                    break;
                                case 3: Option.InnerText = txtOption4.Text;
                                    if (rbOption4.IsChecked == true)
                                        Option.Attributes.Append(Correct);
                                    break;
                            }
                            xn2.ReplaceChild(Option, y);
                        }
                    }
                }
                //xmlDoc.Save(filename);
            }
        }


        private void UpdateFillinQuestion()
        {
            xmlDoc.Load(filename);
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("ns", "urn:Question-Schema");

            XmlNodeList nodes = xmlDoc.SelectNodes("/ns:Quiz/ns:Questions/ns:FillBlanks/ns:Question[@ID=" + ID + "]", ns);
            foreach (XmlNode xn in nodes)
            {
                XmlNode NewQuesti = xmlDoc.CreateElement("Questi", xmlNS);
                NewQuesti.InnerText = txtFillBlanks.Text;
                xn.ReplaceChild(NewQuesti, xn["Questi"]);
                foreach (XmlNode xn2 in xn)
                {
                    if (xn2.Name == "Options")
                    {

                        xn2.RemoveAll(); // clear all previous nodes,, start new nodes
                        for (int i = 0; i < lbCorrectAnswers.Items.Count; i++)
                        {
                            XmlElement OptionCorrect = xmlDoc.CreateElement("Option", xmlNS);
                            OptionCorrect.InnerText = lbCorrectAnswers.Items[i].ToString();
                            xn2.AppendChild(OptionCorrect);

                            XmlAttribute Correct = xmlDoc.CreateAttribute("Correct");
                            Correct.Value = "yes";
                            OptionCorrect.Attributes.Append(Correct);
                            xn2.AppendChild(OptionCorrect);
                        }
                        for (int i = 0; i < lbOtherOptions.Items.Count; i++)
                        {
                            XmlElement OtherOptions = xmlDoc.CreateElement("Option", xmlNS);
                            OtherOptions.InnerText = lbOtherOptions.Items[i].ToString();
                            xn2.AppendChild(OtherOptions);
                        }

                        //Question.AppendChild(Questi);
                        //Question.AppendChild(Options);

                    }
                }
            }
        }

        // this button adds True False Question
        private void btnTrueFalse_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(NewFilePath))
            {
                CreateQuiz();
            }
            else
                isNew = false;
            if (isEdit)
                TrueFalseUpdateQuestion();
            else
                AddTrueFalse();
            if (btnTrueFalseEdit.IsEnabled)
            {
                MessageBox.Show("Your Question has been saved to the Tree View");
            }
            else
                AddTrueFalse();

            XmlTextWriter wr = new XmlTextWriter(NewFilePath, null);
            wr.Formatting = Formatting.None; // no new line spaces;

            xmlDoc.Save(wr);
            filename = NewFilePath;
            wr.Close();
            LoadTreeView();
        }
        private void TrueFalseUpdateQuestion()
        {
            xmlDoc.Load(filename);
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("ns", "urn:Question-Schema");

            XmlNodeList nodes = xmlDoc.SelectNodes("/ns:Quiz/ns:Questions/ns:TrueFalse/ns:Question[@ID=" + ID + "]", ns);
            foreach (XmlNode xn2 in nodes)
            {
                XmlNode NewQuesti = xmlDoc.CreateElement("Questi", xmlNS);
                NewQuesti.InnerText = txtTrueFalse.Text;
                xn2.ReplaceChild(NewQuesti, xn2["Questi"]);
                XmlElement Answer = xmlDoc.CreateElement("Answer", xmlNS);
                foreach (XmlNode xn3 in xn2)
                {
                    if (xn3.Name == "Answer")
                    {
                        if
                            (rbFalse.IsChecked == true)
                            Answer.InnerText = "false";
                        else
                            if (rbTrue.IsChecked == true)
                                Answer.InnerText = "true";
                    }
                }
                xn2.ReplaceChild(Answer, xn2["Answer"]);
            }
        }
        // this button adds Fill Blanks Question
        private void btnFillBlanks_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(NewFilePath))
            {
                CreateQuiz();
            }
            else
                isNew = false;
            if (isEdit)
                UpdateFillinQuestion();
            else
            {
                AddFillBlanks();
            }


            XmlTextWriter wr = new XmlTextWriter(NewFilePath, null);
            wr.Formatting = Formatting.None; // no new line spaces;

            xmlDoc.Save(wr);
            filename = NewFilePath;
            wr.Close();
            LoadTreeView();
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenDialog();
        }
        private void OpenDialog()
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
                LoadFileAndValidate();
            }
        }
        public void LoadFileAndValidate()
        {
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
                status.Text = "Status: Ok";
                status.Foreground = System.Windows.Media.Brushes.Green;
                status.Background = System.Windows.Media.Brushes.White;
                ErrorLogContent.Error = "";
                Save_As.IsEnabled = true;
                Save.IsEnabled = true;
                gridQuizSummary.Visibility = System.Windows.Visibility.Visible;
                FrontGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                status.Text = "";
                MessageBox.Show("File Open Status: Failed");
                status.Text = "Status: Error(s) found, Check error logs";
                status.Foreground = System.Windows.Media.Brushes.White;
                status.Background = System.Windows.Media.Brushes.Red;
                ErrorLogContent.Error = OV.status;
                Save_As.IsEnabled = false;
            }
        }
        public void LoadTreeView()
        {
            ClearAll("1");
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            // create XmlReader object
            XmlReader reader = XmlReader.Create(filename, settings);
            tree = new TreeViewItem(); // instantiate TreeViewItem
            tree.Header = "Quiz"; // assign name to TreeViewItem
            QuizTree.Items.Add(tree); // add TreeViewItem to TreeView
            BuildTreeView BT = new BuildTreeView();
            BT.BuildTree(reader, tree); // build node and tree hierarchy
            QuizItemCount.Content = AddQuestion.CielingId;// the highest ID found on the file
            reader.Close(); //needed to close the reader so there will be no conflict on saving the file
            if (filename != null)
            {
                Save_As.IsEnabled = true; Save.IsEnabled = true;
            }
            else
            {
                Save_As.IsEnabled = false; Save.IsEnabled = false;
            }
        }
        public void ClearAll(string selection = null)
        {
            switch (selection)
            {
                case "1":
                    QuizTree.Items.Clear();
                    break;
                case "2":
                    ID = 0;
                    isNew = true; ;
                    isEdit = false;
                    break;
                case "3":
                    txtSubject.Clear();
                    txtTime.Clear();
                    txtTitle.Clear();
                    cmbCourse.SelectedIndex = -1;
                    cmbDiff.SelectedIndex = -1;
                    goto case "1";  // first time using this, seems ok.
            }
            failed = false;
            txtQuestion.Clear();
            txtOption1.Clear();
            txtOption2.Clear();
            txtOption3.Clear();
            txtOption4.Clear();
            txtTrueFalse.Clear();
        }
        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {//any item selected in the treeview will result on loopings where it begins at the bottom hierarchy(current node selected) until it reaches the parent node
            TreeViewItem item = sender as TreeViewItem;
            //if (item == e.OriginalSource)
            if (item != null)
            {
                int i = item.Header.ToString().IndexOf("Question no.");
                if (i == 0)
                {// question is selected in the treeview. extract the question Id
                    ID = Convert.ToInt16(item.Header.ToString().Replace("Question no. ", "")); isView = true;
                    LoadItemsFromTreeView(ID.ToString());
                }
                else
                {
                    isView = false;
                    LoadItemsFromTreeView();
                }
                if (ID != 0)
                {
                    if (item.Header.ToString().IndexOf("MultipleChoice") == 0) //if it is a multiple type
                        cmbQuestionType.SelectedValue = "Multiple Choice";
                    else if (item.Header.ToString().IndexOf("FillBlanks") == 0) //if it is a fill in type
                        cmbQuestionType.SelectedValue = "Fill in the blanks";
                    //else if (item.Header.ToString().IndexOf("longAnswer") == 0) //if it is a long Answer type
                    //    cmbQuestionType.SelectedValue = "Long Answer";
                    else if (item.Header.ToString().IndexOf("TrueFalse") == 0) //if it is a long Answer type
                        cmbQuestionType.SelectedValue = "True False";
                    isAddNew = false;// show the gridAddDelete
                }
                else
                    isAddNew = true;//Hide The GridAddDelete
                ActivateGridEditDelete();
                btnTrueFalse.Visibility = System.Windows.Visibility.Hidden;
                btnSubmit.Visibility = System.Windows.Visibility.Hidden;
                btnTrueFalseDelete.Visibility = System.Windows.Visibility.Visible;
                btnTrueFalseEdit.Visibility = System.Windows.Visibility.Visible;
                btnEdit.Visibility = System.Windows.Visibility.Visible;
                btnDelete.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void cmbQuestionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string Type;
            int i = cmbQuestionType.SelectedIndex;
            if (i == -1)//means nothing is selected
                Type = null;
            else
                Type = (e.AddedItems[0] as ComboBoxItem).Content as string;
            if (Type != null)
            {
                switch (Type)
                {
                    case "Multiple Choice":
                        btnEdit.Visibility = System.Windows.Visibility.Hidden;
                        btnDelete.Visibility = System.Windows.Visibility.Hidden;
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
                        btnTrueFalse.Visibility = System.Windows.Visibility.Visible;
                        btnTrueFalseEdit.Visibility = System.Windows.Visibility.Hidden;
                        btnTrueFalseDelete.Visibility = System.Windows.Visibility.Hidden;
                        gridTrueFalse.Visibility = System.Windows.Visibility.Visible;
                        gridMultipleChoice.Visibility = System.Windows.Visibility.Hidden;
                        gridFillBlanks.Visibility = System.Windows.Visibility.Hidden;

                        break;
                    default:
                        gridFillBlanks.Visibility = System.Windows.Visibility.Hidden;
                        gridMultipleChoice.Visibility = System.Windows.Visibility.Hidden;
                        gridTrueFalse.Visibility = System.Windows.Visibility.Hidden;
                        break;
                }
                ClearAll();
            }
            // retrieving UID from selected ComboBox Item and saving it in a public string
            ActivateGridEditDelete();
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
        private void GenerateQuizid()
        {
            var i = new Random();
            QuizId = i.Next(1, 999999);
        }
        private void New_Click(object sender, RoutedEventArgs e)
        {
            NewDialog();
        }
        private void NewDialog()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".xqz";
            dlg.Filter = "Exam File (.xqz)|*.xqz";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                ClearAll("3");
                HideGridPanels();
                txtTitle.IsReadOnly = false;
                txtSubject.IsReadOnly = false;
                txtTime.IsReadOnly = false;
                cmbDiff.IsEnabled = true;
                cmbCourse.IsEnabled = true;
                string filename = dlg.FileName;
                NewFilePath = filename;
                this.filename = NewFilePath;
                Save_As.IsEnabled = true;
                Save.IsEnabled = true;
            }
            gridQuizSummary.Visibility = System.Windows.Visibility.Visible;
        }
        private void ActivateGridEditDelete()
        {
            if (isAddNew)
                gridEditDelete.Visibility = System.Windows.Visibility.Hidden;
            else
                gridEditDelete.Visibility = System.Windows.Visibility.Visible;
        }
        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            GridQuestionType.Visibility = System.Windows.Visibility.Visible;
            ClearAll("2");
            HideGridPanels();
            isAddNew = true;
            cmbQuestionType.SelectedIndex = -1;  //set the default choice to null
            ActivateMultipleGrid();
            btnSubmit.Visibility = System.Windows.Visibility.Visible;
            txtTrueFalse.IsReadOnly = false;

            //Fill in the Blanks
            txtFillBlanks.Text = "";
            lbCorrectAnswers.Items.Clear();
            lbOtherOptions.Items.Clear();
            txtFillBlanks.IsReadOnly = false;
            btnAddFillinCorrectAnswers.Visibility = System.Windows.Visibility.Visible;
            btnRemoveCorrectAnswers.Visibility = System.Windows.Visibility.Visible;
            btnAddFillinOptions.Visibility = System.Windows.Visibility.Visible;
            btnRemoveFillinOptions.Visibility = System.Windows.Visibility.Visible;
            txtOptionFillin.Visibility = System.Windows.Visibility.Visible;
            btnSubmitFillin.Visibility = System.Windows.Visibility.Visible;
            GridQuestionType.Visibility = System.Windows.Visibility.Visible;
        }
        private void HideGridPanels()
        {
            gridEditDelete.Visibility = System.Windows.Visibility.Hidden;
            gridFillBlanks.Visibility = System.Windows.Visibility.Hidden;
            gridMultipleChoice.Visibility = System.Windows.Visibility.Hidden;
            gridTrueFalse.Visibility = System.Windows.Visibility.Hidden;
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            status.Text = ID.ToString();
            isEdit = true;
            ActivateMultipleGrid();
            btnSubmit.Visibility = System.Windows.Visibility.Visible;
        }
        private void HowTo_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow win2 = new HelpWindow();
            win2.Show();
        }
        private void ActivateMultipleGrid()
        {
            txtQuestion.IsReadOnly = false;
            txtOption1.IsReadOnly = false;
            txtOption2.IsReadOnly = false;
            txtOption3.IsReadOnly = false;
            txtOption4.IsReadOnly = false;
        }
        private void ActivateTrueFalseGrid()
        {
            txtTrueFalse.IsReadOnly = false;
        }

        private void ActivateFillinGrid()
        {
            //Fill in the Blanks
            txtFillBlanks.IsReadOnly = false;
            btnAddFillinCorrectAnswers.Visibility = System.Windows.Visibility.Visible;
            btnRemoveCorrectAnswers.Visibility = System.Windows.Visibility.Visible;
            btnAddFillinOptions.Visibility = System.Windows.Visibility.Visible;
            btnRemoveFillinOptions.Visibility = System.Windows.Visibility.Visible;
            txtOptionFillin.Visibility = System.Windows.Visibility.Visible;
            btnSubmitFillin.Visibility = System.Windows.Visibility.Visible;
            GridQuestionType.Visibility = System.Windows.Visibility.Visible;

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Do you want to close this window?",
               "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("ns", "urn:Question-Schema");

            XmlNodeList nodes = xmlDoc.SelectNodes("/ns:Quiz/ns:Questions/ns:MultipleChoice/ns:Question[@ID=" + ID + "]", ns);

            XmlNode node = nodes[0];

            node.ParentNode.RemoveChild(node);

            MessageBox.Show("The Selected Question Has Been Deleted");

            XmlTextWriter wr = new XmlTextWriter(filename, null);
            wr.Formatting = Formatting.None; // no new line spaces;
            xmlDoc.Save(wr);
            wr.Close();
            LoadTreeView();
        }
        private void ErrorLog_Click(object sender, RoutedEventArgs e)
        {
            ErrorLog win2 = new ErrorLog();
            win2.Show();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (CheckErrors("multi"))
                MessageBox.Show("Errors found");
            if (CheckErrors())
                MessageBox.Show("e");
        }
        private bool CheckErrors(string choice = null)
        {
            BindingExpression be = txtSubject.GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();
            BindingExpression be1 = txtTitle.GetBindingExpression(TextBox.TextProperty);
            be1.UpdateSource();
            BindingExpression be2 = txtTime.GetBindingExpression(TextBox.TextProperty);
            be2.UpdateSource();
            BindingExpression be3 = cmbCourse.GetBindingExpression(ComboBox.SelectedValueProperty);
            be3.UpdateSource();
            BindingExpression be4 = cmbDiff.GetBindingExpression(ComboBox.SelectedValueProperty);
            be4.UpdateSource();
            bool isValid = false;     //return false, user's cant save the file if there are no question created, we cant have empty file!
            switch (choice)
            {
                case "multi":
                    BindingExpression mult = txtQuestion.GetBindingExpression(TextBox.TextProperty);
                    mult.UpdateSource();
                    BindingExpression mult1 = txtOption1.GetBindingExpression(TextBox.TextProperty);
                    mult1.UpdateSource();
                    BindingExpression mult2 = txtOption2.GetBindingExpression(TextBox.TextProperty);
                    mult2.UpdateSource();
                    BindingExpression mult3 = txtOption3.GetBindingExpression(TextBox.TextProperty);
                    mult3.UpdateSource();
                    BindingExpression mult4 = txtOption4.GetBindingExpression(TextBox.TextProperty);
                    mult4.UpdateSource();
                    if (be.HasError || be1.HasError || be2.HasError || be3.HasError || be4.HasError || mult.HasError || mult1.HasError || mult2.HasError || mult3.HasError || mult4.HasError)
                        isValid = true;        //return true if there is an error
                    else
                        isValid = false;       //all validations pass
                    break;
                case "FillIn":
                    break;
            }
            return isValid;
        }
        private void Save_As_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".xqz";
            dlg.Filter = "Exam File (.xqz)|*.xqz";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string SaveAsFilePath = dlg.FileName;
                XmlTextWriter wr = new XmlTextWriter(SaveAsFilePath, null);
                wr.Formatting = Formatting.None; // no new line spaces;
                xmlDoc.Save(wr);
                wr.Close();
            }
        }
        private void btnAddUnderline_Click(object sender, RoutedEventArgs e)
        {
            txtFillBlanks.Text += " ________________ ";
        }
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            NewDialog();
        }
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenDialog();
        }
        private void btnTrueFalseDelete_Click(object sender, RoutedEventArgs e)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("ns", "urn:Question-Schema");

            XmlNodeList nodes = xmlDoc.SelectNodes("/ns:Quiz/ns:Questions/ns:TrueFalse/ns:Question[@ID=" + ID + "]", ns);

            XmlNode node = nodes[0];

            node.ParentNode.RemoveChild(node);

            MessageBox.Show("The Selected Question Has Been Deleted");

            XmlTextWriter wr = new XmlTextWriter(filename, null);
            wr.Formatting = Formatting.None; // no new line spaces;
            xmlDoc.Save(wr);
            wr.Close();
            LoadTreeView();
        }
        private void TrueFalseEdit_Click(object sender, RoutedEventArgs e)
        {
            status.Text = ID.ToString();
            isEdit = true;
            ActivateTrueFalseGrid();
            btnTrueFalse.Visibility = System.Windows.Visibility.Visible;

        }

        private void btnAddCorrectAnswers_Click(object sender, RoutedEventArgs e)
        {
            lbCorrectAnswers.Items.Add(txtOptionFillin.Text);
        }

        private void btnAddFillinOptions_Click(object sender, RoutedEventArgs e)
        {
            lbOtherOptions.Items.Add(txtOptionFillin.Text);
        }

        private void btnRemoveCorrectAnswers_Click(object sender, RoutedEventArgs e)
        {
            if (this.lbCorrectAnswers.SelectedIndex >= 0)
                this.lbCorrectAnswers.Items.RemoveAt(this.lbCorrectAnswers.SelectedIndex);
        }

        private void btnRemoveFillinOptions_Click(object sender, RoutedEventArgs e)
        {
            if (this.lbOtherOptions.SelectedIndex >= 0)
                this.lbOtherOptions.Items.RemoveAt(this.lbOtherOptions.SelectedIndex);
        }

        private void btnEditFillin_Click(object sender, RoutedEventArgs e)
        {
            status.Text = ID.ToString();
            isEdit = true;
            ActivateFillinGrid();
            btnSubmitFillin.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnDeleteFillin_Click(object sender, RoutedEventArgs e)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("ns", "urn:Question-Schema");

            XmlNodeList nodes = xmlDoc.SelectNodes("/ns:Quiz/ns:Questions/ns:FillBlanks/ns:Question[@ID=" + ID + "]", ns);

            XmlNode node = nodes[0];

            node.ParentNode.RemoveChild(node);

            MessageBox.Show("The Selected Question Has Been Deleted");

            XmlTextWriter wr = new XmlTextWriter(filename, null);
            wr.Formatting = Formatting.None; // no new line spaces;
            xmlDoc.Save(wr);
            wr.Close();
            LoadTreeView();
        }

        private void btnUpdateFillin_Click(object sender, RoutedEventArgs e)
        {
            if (FillInOptionOld != null)
            {
                if (this.lbCorrectAnswers.SelectedIndex >= 0)
                {
                    lbCorrectAnswers.Items.Remove(FillInOptionOld);
                    //lbCorrectAnswers.Items.RemoveAt(lbCorrectAnswers.SelectedIndex);
                    lbCorrectAnswers.Items.Add(txtOptionFillin.Text);
                }
                if (this.lbOtherOptions.SelectedIndex >= 0)
                {
                    lbOtherOptions.Items.Remove(FillInOptionOld);
                    //lbCorrectAnswers.Items.RemoveAt(lbCorrectAnswers.SelectedIndex);
                    lbOtherOptions.Items.Add(txtOptionFillin.Text);
                }
            }
            txtOptionFillin.Visibility = System.Windows.Visibility.Hidden;

        }

        private void lbCorrectAnswers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbCorrectAnswers.SelectedIndex != -1)
            {
                txtOptionFillin.Visibility = System.Windows.Visibility.Visible;
                FillInOptionOld = lbCorrectAnswers.SelectedItem.ToString();
                txtOptionFillin.Text = lbCorrectAnswers.SelectedItem.ToString();
            }

        }

        private void lbOtherOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbOtherOptions.SelectedIndex != -1)
            {
                txtOptionFillin.Visibility = System.Windows.Visibility.Visible;
                FillInOptionOld = lbOtherOptions.SelectedItem.ToString();
                txtOptionFillin.Text = lbOtherOptions.SelectedItem.ToString();
            }
        }
    }
}
