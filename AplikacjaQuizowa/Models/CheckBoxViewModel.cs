using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplikacjaQuizowa.Models
{
    public class CheckBoxViewModel:Categorie
    {
        public int Id { get; set; }
        public bool Checked { get; set; }
    }
}