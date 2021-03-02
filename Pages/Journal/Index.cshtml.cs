using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Pulse.Pages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pulse.Pages.Journal
{
    public class IndexModel : BasePulsePage
    {
         
      
        public IList<Pulse.Models.Journal> JournalList { get; set; }
        public IList<Pulse.Models.Room> Rooms { get; set; }

        [BindProperty]
        public Pulse.Models.JournalFilter JournalFilter { get; set; }
        public IndexModel(Pulse.Data.AsuContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration) : base(context, appEnvironment, configuration)
        {
            JournalFilter = new Models.JournalFilter();
        }



        public IActionResult OnGet()
        {

            //EmployeesList = _context.Employees.FromSqlRaw("sp_PulseUsersMonthWork @Month, @Year", currentMonth, currentYear).ToList();
            try 
            {
               FillJournal();
                FillRooms();
                ViewData["RoomID"] = new SelectList(Rooms.OrderBy(e => e.RoomName), "RoomID", "RoomName");
            }
            catch (Exception e )
            {
                return NotFound(e.Message);

            }
            return Page();

        }
        private void FillJournal()
        {
            SqlParameter operDate = new SqlParameter("@Operdate", JournalFilter.CurrentDate.ToString("yyyy-MM-dd"));
            SqlParameter roomID = new SqlParameter("@RoomID", JournalFilter.RoomID );
            JournalList = _context.Journal.FromSqlRaw("[sp_PulseOperationJournal] @operdate,@RoomID", operDate,roomID).ToList();
        }
        private void FillRooms()
        {
            string sql = "select distinct r.Roomid, r.Name as RoomName,r.Number as RoomNumber  from Room r " +
            "inner join Equipment e on e.RoomId = r.RoomId " +
            "where e.Breaked = 0 ";
            Rooms = _context.Rooms.FromSqlRaw(sql).ToList();
            Rooms.Add(new Models.Room { RoomID = -1, RoomName = "—ÍÎ‡‰ › ¡" });
            Rooms.Add(new Models.Room { RoomID = 0, RoomName = "¬ÒÂ" });
            Rooms.Add(new Models.Room { RoomID = -2, RoomName = "¿—” »÷" });
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                FillJournal();
                FillRooms();
                ViewData["RoomID"] = new SelectList(Rooms.OrderBy(e => e.RoomName), "RoomID", "RoomName");
            }
            catch (Exception e)
            {
                return NotFound(e.Message);

            }
            return Page();

        }
    }
}