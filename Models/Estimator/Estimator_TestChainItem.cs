using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Pulse.Models.Estimator
{
    /// <summary>
    /// Для работы с элементом цепочки программы
    /// </summary>
    public class  TestChainItem
    {
        [Key]
        public int TestChainItemID { get; set; }
        public int OperationID { get; set;}
        public string Name { get; set; }
    }
}
