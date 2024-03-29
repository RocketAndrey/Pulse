﻿using System;
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


namespace Pulse.Pages
{
    public class IndexModel : BasePulsePage 
    {
      
        public IList<Pulse.Models.Employee> EmployeesList { get; set; }
        
        public IndexModel(Pulse.Data.AsuContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration) : base(context, appEnvironment, configuration)
        {
            CurrentMonth =  DateTime.Now.Month;
            CurrentYear = DateTime.Now.Year; 
        }
        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set;  }
        public  IActionResult  OnGet()
        {
            
            SqlParameter currentMonth = new SqlParameter("@Month", CurrentMonth  );
            SqlParameter currentYear = new SqlParameter("@Year", CurrentYear );
            try
            {

                EmployeesList = _context.Employees.FromSqlRaw("sp_PulseUsersMonthWork @Month, @Year", currentMonth, currentYear).ToList();
                return Page(); 
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); 
            }


        }
    }
}
