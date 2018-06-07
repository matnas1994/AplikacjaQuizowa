using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplikacjaQuizowa.Models
{
    public class Question1
    {
        public int ID { set; get; }
        public string QuestionText { set; get; }
        public List<Answer> Answers { set; get; }
        public string SelectedAnswer { set; get; }
        public Question1()
        {
            Answers = new List<Answer>();
        }
    }
}