using System;
using System.ComponentModel.DataAnnotations;
namespace Pulse.Models
{
    public class Employee
    {
        public System.Guid EmployeeID { get; set; }
        [Display(Name = "Сотрудник")]
        public string EmployeeName { get; set; }
        [Display(Name = "На руках")]
        public int OnHands { get; set; }
        [Display(Name = "Выполнено")]
        public int CurrentMonthCount { get; set; }
        [Display(Name = "Не нормировано")]
        public int MonthNolaborCount { get; set; }
        [Display(Name = "Трудоёмкость, час")]
        public Double CurrentMonthLabor { get; set; }

        public int TodayCount { get; set; }
        public int TodayNoLaborCount { get; set; }
        public Double TodayLabor { get; set; }
        public int QueryCount { get; set; }
    }
}
