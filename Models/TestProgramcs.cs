using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Pulse.Models
{
    public class TestProgram
    {
        [Key]
        public Int64 ProgramID { get; set; }
        public Int64 ContractID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string KA { get; set; }
    }
}
