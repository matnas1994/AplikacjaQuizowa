using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplikacjaQuizowa.Models
{
    public class QuestionToCategories
    {
        public int QuestionToCategoriesId { get; set; }
        public int CategorieId { get; set; }
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
        public virtual Categorie Categorie { get; set; }
    }
}