using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Pulse.Models
{
    public class RouteOperation
    {
       public Int64 RouteOperationID { get; set; }
       public System.Guid? UserID { get; set; }
        public Int64 LotID { get; set; }
        [Display(Name = "Дата начала:")]
        public DateTime? StartTime { get; set; }
        [Display(Name = "Дата окончания:")]
        public DateTime? EndTime { get; set; }
        [Display(Name = "Кол-во:")]
        public int QTY { get; set; }
        public Int64? TestID { get; set; }
        public string TestTypeID { get; set; }
        public Int64? MethodId { get; set; }
        public Int64? ModeId { get; set; }
        [Display(Name = "Номера изделий")]
        public string ProductNumbers { get; set; }

       public int Order { get; set; }
        [Display(Name = "Номер")]
        public string Number { get; set; }
        [Display(Name = "Код")]
        public string Code { get; set; }
        [Display(Name = "Пункт программы")]
        public string Description { get; set; }
        [Display(Name = "Результат")]
        public string Result { get; set; }
        public int Disabled { get; set; }
        public bool Complete 
        { 
            get
            {
                return (StartTime != null && EndTime != null);
            }
        }
        [Display(Name = "Исполнитель")]
        public string UserName { get; set; }

    }
}
