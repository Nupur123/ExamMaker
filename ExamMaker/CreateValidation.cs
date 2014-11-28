using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ExamMaker
{
    public class CreateValidation
    {
        private string _subject;
        private string _title;
        private int _time;
        private string _course;
        private string _diff;
        private string _multiQuestion;
        private string _option1;
        private string _option2;
        private string _option3;
        private string _option4;
        private string _TrueFalse;


        public string Subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("Customer name is mandatory.");
                }
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("Title is mandatory.");
                }
            }
        }

        public int Time
        {
            get { return _time; }

            set
            {
                _time = value;
                if (!int.TryParse(value.ToString(), out _time))
                {/// not working yet
                    throw new Exception("Time not in correct Format.");
                }
            }
        }
        public string Course
        {
            get { return _course; }
            set
            {
                _course = value;
                if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("Course is empty.");
                }
            }
        }
        public string Difficulty
        {
            get { return _diff; }
            set
            {
                _diff = value; if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("Difficulty must be set.");
                }
            }
        }
        public string MultiQuestion
        {
            get { return _multiQuestion; }
            set
            {
                _multiQuestion = value; if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("Question should not be empty");
                }
            }
        }
        public string Option1
        {
            get { return _option1; }
            set
            {
                _option1 = value; if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("Option text is required");
                }
            }
        }
        public string Option2
        {
            get { return _option2; }
            set
            {
                _option2 = value; if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("Option text is required");
                }
            }
        }
        public string Option3
        {
            get { return _option3; }
            set
            {
                _option3 = value; if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("Option text is required");
                }
            }
        }
        public string Option4
        {
            get { return _option4; }
            set
            {
                _option4 = value; if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("Option text is required");
                }
            }
        }
        public string TrueFalse
        {
            get { return _TrueFalse; }
            set
            {
                _TrueFalse = value; if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Please enter a question");
                }
            }

        }



        public void ClearAll()
        {
            _subject = null;
            _title = null;
            _time = 0;
            _course = null;
            _diff = null;
            _multiQuestion = null;
            _option1 = null;
            _option2 = null;
            _option3 = null;
            _option4 = null;
            _TrueFalse = null;
        }



    }
}
