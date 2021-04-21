using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Pulse.Models
{
    public class ContractsFilter
    {
        public ContractsFilter()
            {
            StartDate = null;
            EndDate = null;
            }
        public string Number { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Начало")]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Конец")]
        public DateTime? EndDate { get; set; }
        public string Code { get; set; }
        public Int64 OrganizationID { get; set; }
        public Organization Client { get; set; }
    }
   
}
