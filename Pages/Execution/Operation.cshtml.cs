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
using Pulse.Models;


namespace Pulse.Pages
{
    public class OperationModel : BasePulsePage 
    {
        public IList<Pulse.Models.Operation > Operations  { get; set; }
        public UserInfo UserInfo;
        public string Result;
        public OperationModel(Pulse.Data.AsuContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration, ILogger<IndexModel> logger) : base(context, appEnvironment, configuration)
        {
        }
        public void OnGet(string user, int month = 0 , int year=2020, int onhand = 0, int query=0, int nolabor = 0)
        {
            if (month == 0 | month > 12 ) { month = DateTime.Now.Month; }
            if (year <2020 ) { year = DateTime.Now.Year; }

            SqlParameter userP = new SqlParameter("@userID", user);
            SqlParameter currentMonth = new SqlParameter("@Month", month);
            SqlParameter currentYear = new SqlParameter("@Year", year);
            SqlParameter OnHand = new SqlParameter("@OnHand", onhand);
            SqlParameter NoLabor = new SqlParameter("@NoLabor", nolabor);
            SqlParameter Query = new SqlParameter("@Query", query);

            string userSQL = "select Userid as EmployeeID, LastName +' '+ FN as [EmployeeName] from UserInfo where Userid = {0}";
            Result = MonthName[month - 1] + ' ' + year +"."; 
            if (onhand != 0) { Result += "Операции на руках."; }
            if (nolabor == 1) { Result += " Не нормированы."; }
            if (query == 1) { Result += " Не принятые операции."; }

            UserInfo = _context.UserInfo.FromSqlRaw(userSQL, user).FirstOrDefault();

            Operations = _context.Operations.FromSqlRaw("[sp_PulseUserOperations] @userID, @Month, @Year,@OnHand,@NoLabor,@Query",
                userP, currentMonth, currentYear, OnHand, NoLabor,Query).ToList();
        }
        }
    }
