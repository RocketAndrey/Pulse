using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Pulse.Models.Estimator
{
    /// <summary>
    /// Для работы с классом ЭКБ из калькулятора
    /// </summary>
    public class ElementType
    {
        [Key]
        public int ElementTypeID { get; set; }
        public string Name { get; set; }
        public Int32 ProgramID { get; set; }
    }
}
