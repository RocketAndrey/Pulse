using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Pulse.Models
{
    /// <summary>
    /// Описывает Операцию для связи-  калькулятор-АСУ
    /// </summary>
    public class GroupLaborOperation
    {

        [Display(Name = "Операция")]
        public string Name { get; set; }
        [Display(Name = "Класс изделий")]
        public string ClassType { get; set; }
        public Int64 ClassId { get; set; }
        public Int64 BaseOperationID { get; set; }
        [Display(Name = "Количество операций")]
        public int Count_Op { get; set;  }
    }
}
