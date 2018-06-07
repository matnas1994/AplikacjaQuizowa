using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplikacjaQuizowa.Models
{
    public class Question
    {

        [ScaffoldColumn(false)]
        public int QuestionId { get; set; }

        [Display(Name = "Pytanie")]
        [Required]
        [StringLength(250)]
        public string Contents { get; set; }

        [Display(Name = "Odpowiedź 1")]
        [Required]
        [StringLength(250)]
        public string Answer1 { get; set; }

        [Display(Name = "Odpowiedź 2")]
        [Required]
        [StringLength(250)]
        public string Answer2 { get; set; }

        [Display(Name = "Odpowiedź 3")]
        [Required]
        [StringLength(250)]
        public string Answer3 { get; set; }

        [Display(Name = "Poprawna odpowiedź")]
        [Required]
        [StringLength(250)]
        public string CorrectAnswer { get; set; }

        public virtual ICollection<QuestionToCategories> QuestionsToCategories { get; set; }
    }
}