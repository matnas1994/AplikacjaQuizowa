using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplikacjaQuizowa.Models
{
    public class QuestionToAnsersModelView
    {
        public Categorie Categorie { get; set; }
        public List<QuestionToAnswer> QuestionToAnswers;

        public QuestionToAnsersModelView()
        {
            QuestionToAnswers = new List<QuestionToAnswer>();
        }
    }
}