using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplikacjaQuizowa.Models
{
    public class CorrectAnswerViewModel
    {
        public Categorie Categorie { get; set; }
        public List<CorrectAnswer> CorrectAnswer { get; set; }

        public CorrectAnswerViewModel()
        {
            CorrectAnswer =  new List<CorrectAnswer>();
        }

    }
}