using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pulse.Models.Estimator
{
    /// <summary>
    /// Для отображения ранее сохраненного элемента и программы 
    /// </summary>
    public class ChainItemFilter
    {
        public int ProgramID { get; set; }
        public int ElementTypeID { get; set; }
    }
}
