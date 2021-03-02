using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pulse.Models
{
    public class Room
    {
        public Int64 RoomID { get; set; }
        [Display(Name = "Рабочее место")]
        public string RoomName { get; set; }
        [Display(Name = "№ Рабочего места")]
        public string RoomNumber { get; set; }
    }
}
