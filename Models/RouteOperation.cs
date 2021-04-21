using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pulse.Models
{
    public class RouteOperation
    {
       public Int64 RouteOperationID { get; set; }
       public System.Guid? UserID { get; set; }
        public Int64 LotID { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int QTY { get; set; }
        public Int64? TestID { get; set; }
        public string TestTypeID { get; set; }
        public Int64? MethodId { get; set; }
        public Int64? ModeId { get; set; }
        public string? ProductNumbers { get; set; }
       public int Order { get; set; }
        public string Number { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Disabled { get; set; }
        public bool Complete 
        { 
            get
            {
                return (StartTime != null && EndTime != null);
            }
        }

    }
}
