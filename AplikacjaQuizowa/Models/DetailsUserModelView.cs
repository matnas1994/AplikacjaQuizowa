using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AplikacjaQuizowa.Models
{
    public class DetailsUserModelView
    {
        public string Id { get; set; }

        [Display(Name = "Adres Email")]
        public string Email { get; set; }

        [Display(Name = "Uprawnienia")]
        public List<CheckRoleBoxViewModel> Roles { get; set; }

        public List<Score> Scores { get; set; }

        public DetailsUserModelView(){
            Scores = new List<Score>();
            Roles = new List<CheckRoleBoxViewModel>();
        }

    }
}