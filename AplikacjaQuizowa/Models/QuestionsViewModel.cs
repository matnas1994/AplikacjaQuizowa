using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AplikacjaQuizowa.Models
{
    public class QuestionsViewModel:Question
    {
    
        [Display(Name = "Kategorie")]
        public List<CheckBoxViewModel> Categories { get; set; }
    }
}