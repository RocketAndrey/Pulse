using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Pulse.Models
{
    public class Contract
    {
        [Key]
        public System.Int64 ContractId { get; set; }
        [Display(Name = "№ Договора")]
        public string Number { get; set; }
        [Display(Name = "Дата договора")]
        public DateTime? CreationDate { get; set; }
        [Display(Name = "Окончание работ")]
        public DateTime? EndDate { get; set;}
        [Display(Name = "Заказчик")]
        public string Client { get; set; }
        [Display(Name = "Поставщик")]
        public string Supplier { get; set; }
    }
}
