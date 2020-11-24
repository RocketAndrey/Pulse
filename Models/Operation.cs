using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

using Microsoft.EntityFrameworkCore;

    namespace Pulse.Models
{

    public class Operation
    {
        [Display(Name = "Типономинал")]
        public string TypeNominal { get; set; }
        [Display(Name = "Сотрудник")]
        public string EmployeeName { get; set; }
        [Display(Name = "Операция")]
        public string OperationName { get; set; }
        [Display(Name = "№ МК")]
        public string CardNumber { get; set; }
        [Display(Name = "Трудоемкость")]
        public double OperationLabor { get; set; }
        [Display(Name = "Дата  начала")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Дата окончания")]
        public DateTime? EndDate { get; set; }
         }
    
}
