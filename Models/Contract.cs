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
        [Display(Name = "Начало")]
        public DateTime? CreationDate { get; set; }
        [Display(Name = "Окончание")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Заказчик")]
        public string Client { get; set; }
        [Display(Name = "Поставщик")]
        public string Supplier { get; set; }
        [Display(Name = "Код")]
        public string Code { get; set; }
        public int WaresCount { get; set; }
        public int WaresNoEnd { get; set; }
        public Int64 ClientID { get; set; }
        public int NotStartedWareCount { get; set; }
        [Display(Name = "Выполнение")]
        public Decimal ReadyRatio
        {
            get
            {
                return (decimal)(WaresCount - WaresNoEnd - NotStartedWareCount) / (decimal)WaresCount;

            }
        }
        [Display(Name = "Всего/выполнено:")]
        public string CompleteString
            {
            get
            {
                return WaresCount.ToString() + "/" + (WaresCount-WaresNoEnd - NotStartedWareCount).ToString();
            }
            }

        public bool Complete
        {
            get
            {
                if (WaresCount == 0) { return false; }
                return (WaresNoEnd == 0 && NotStartedWareCount ==0) ; 
            }
        }
        public bool MissedEndDate 
        {
            get
            {
                return Complete | (DateTime.Compare( DateTime.Now, EndDate ?? DateTime.Now.AddDays(1)) > 0);
            }
        }

    }
}
