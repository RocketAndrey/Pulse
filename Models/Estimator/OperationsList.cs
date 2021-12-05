using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Pulse.Models.Estimator
{
    /// <summary>
    /// Для отображения списка операций
    /// </summary>
    public class OperationsList
    {
        [Key]
        public Int64 RouteOperationId { get; set; }
        public Int64 ClassId { get; set;}
        public Int64 BaseOperationId { get; set; }
        [Display(Name = "Класс")]
        public string ClassType { get; set; }
        [Display(Name = "Операция")]
        public string OperationName { get; set; }
        [Display(Name = "Типономинал")]
        public string TypeNominal { get; set; }
        [Display(Name = "Маршрутная карта")]
        public string CardNumber { get; set; }
    }
}
