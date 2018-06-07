using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AplikacjaQuizowa.Models
{
    public class ChangeUserModelView
    {
        public string Id { get; set; }

        [Display(Name = "Nowe hasło")]
        [DataType(DataType.Password)]
        [MembershipPassword()]
        public string Password { get; set; }

        [Display(Name = "Adres Email")]
        [StringLength(128)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Uprawnienia")]
        public List<CheckRoleBoxViewModel> Roles { get; set; }

        public ChangeUserModelView()
        {
            Roles = new List<CheckRoleBoxViewModel>();
        }

    }
}