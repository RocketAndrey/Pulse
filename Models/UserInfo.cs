using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Pulse.Models
{
    public class UserInfo
    {
        [Key]
        public System.Guid EmployeeID { get; set; }

        [Display(Name = "Сотрудник")]
        public string EmployeeName { get; set; }
      
    }
}
