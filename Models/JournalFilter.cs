using System;
using System.ComponentModel.DataAnnotations;

namespace Pulse.Models
{
    public class JournalFilter
    {
        public JournalFilter(DateTime currentDate)
        {
            CurrentDate = currentDate; 
        }
        public JournalFilter()
        {
            CurrentDate = DateTime.Now;
        }

        public Int64 RoomID { get; set; }
        public Room Room { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата")]
        public DateTime CurrentDate { get;set;}

    }
}
