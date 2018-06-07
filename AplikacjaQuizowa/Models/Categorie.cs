﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AplikacjaQuizowa.Models
{
    public class Categorie
    {
        [ScaffoldColumn(false)]
        public int CategorieId { get; set; }

        [Display(Name = "Nazwa")]
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public virtual ICollection<QuestionToCategories> QuestionsToCategories { get; set; }
    }
}