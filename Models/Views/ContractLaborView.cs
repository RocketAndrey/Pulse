using System;
using System.ComponentModel.DataAnnotations;

namespace Pulse.Models.Views
{
    public class ContractLaborView
    {
        public ContractLaborView() { }
        public Int64 ContractId {  get; set; }
        [Display(Name = "Класс")]
        public string ClassType { get; set; }
        [Display(Name = "Типономинал")]
        public string ElementType { get; set; }
        [Display(Name = "Исполнитель")]
        public string Employee { get; set; }
        [Display(Name = "Операция")]
        public string OperationName { get; set; }
        [Display(Name = "Трудоемкость")]
        public decimal? Operationlabor { get; set; }
        [Display(Name = "Маршрутная карта")]
        public string CardNumber { get; set; }
        [Display(Name = "Колличество")]
        public int? OperationQTY { get; set; }
        [Display(Name = "Месяц")]

        public string EndMonth { get; set; }
        [Display(Name = "Дата")]
        public DateTime OperationDate { get; set; }
        [Display(Name = "№ Договора")]
        public string ContractNumber { get; set; }
        [Display(Name = "Дата договора")]
        public DateTime ContractDate { get; set; }
        [Display(Name = "Заказчик")]
        public string Organization { get; set; }
    }
}
