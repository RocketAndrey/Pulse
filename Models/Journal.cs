using System;
using System.ComponentModel.DataAnnotations;

namespace Pulse.Models
{
    public class Journal
    {
        [Display(Name = "Событие")]
        public string WorkName { get; set; }
        [Display(Name = "Типономинал")]
        public string TypeNominal { get; set; }
        [Display(Name = "Операция")]
        public string OperationName { get; set; }
        [Display(Name = "№ МК")]
        public string CardNumber { get; set; }
        [Display(Name = "Время")]
        public DateTime EventTime { get; set; }


        public System.Guid userID { get; set; }
        [Display(Name = "Сотрудник")]
        public string UserName { get; set; }
        [Display(Name = "Колличество")]
        public int QTY { get; set; }
        [Display(Name = "Раб.место")]
        public string RoomName { get; set; }
        [Display(Name = "№ Раб. места")]

        public string RoomNumber { get; set; }
        [Display(Name = "Оборудование")]
        public string EquipmentName { get; set; }

        public string OperationDescription
        {
            get
            {
                return OperationName + ";  " + TypeNominal + "; " + QTY +" шт."; 
            }
        }
    }
}
