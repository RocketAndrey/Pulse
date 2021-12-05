using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pulse.Models.Estimator
{
    /// <summary>
    /// Дял работы с Программой испытаний из калькулятора
    /// </summary>
    public class TestProgram
    {
        [Column("TestProgramID")]
        [Key]
        public int TestProgramID { get; set; }
        public string Description { get; set; }
    }
}
