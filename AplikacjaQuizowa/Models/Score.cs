using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AplikacjaQuizowa.Models
{
    public class Score
    {
        public int ScoreId { get; set; }

        public virtual ApplicationUser UserId { get; set; }

        public virtual Categorie CategoriId { get; set; }

        public double Result { get; set; }
    }
}