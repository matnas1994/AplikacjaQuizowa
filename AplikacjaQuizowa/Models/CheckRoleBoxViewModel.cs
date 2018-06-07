using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplikacjaQuizowa.Models
{
    public class CheckRoleBoxViewModel
    {
        public int Id { get; set; }
        public bool Checked { get; set; }
        public string RoleName { get; set; }
        public string RoleId { get; set; }
    }
}