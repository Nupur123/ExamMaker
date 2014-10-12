﻿using System;
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
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode rootNode = null;
        XmlNode QuestionsNode;
        int ID;
        string XmlPath = @"C:\Users\anshulika\Documents\";

        public MainWindow()
        {
            InitializeComponent();

            LoadQuiz();
        }

        private void LoadQuiz()
        {
            if (File.Exists(@"C:\Users\anshulika\Documents\testQuiz.xml"))
            {
                xmlDoc.Load(@"C:\Users\anshulika\Documents\testQuiz.xml");
                rootNode = xmlDoc.DocumentElement;
            }
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
            if (!File.Exists(@"C:\Users\anshulika\Documents\testQuiz.xml"))
            {
                CreateQuiz();
            }

            AddQuestion();

            xmlDoc.Save(@"C:\Users\anshulika\Documents\testQuiz.xml");

            // xmlDoc.Save(XmlPath + txtTitle.Text + ".xml");
        }

    }
}
