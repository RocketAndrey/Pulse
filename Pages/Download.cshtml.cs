using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pulse.Data;
using Pulse.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NonFactors.Mvc.Grid;
using NonFactors.Mvc;
using NPOI.HPSF;
using static NPOI.HSSF.Util.HSSFColor;
using Pulse.Helpers;
using Microsoft.Data.SqlClient;

namespace Pulse.Pages
{
    public class DownloadModel : BasePulsePage
    {
        public DownloadModel(Pulse.Data.AsuContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration) : base(context, appEnvironment, configuration)
        {

        }
        public string ErrorMessage { get; set; }
        public IActionResult OnGet(string reportType,int id, int month,int year)
        {
            string errorMessage = "";

            XSLXWriter writer;
            if (reportType =="contractLabor")
            {
                try
                {
                    string fileName = _appEnvironment.WebRootPath + "/Files/" + reportType + id.ToString() + month.ToString() + year.ToString() + ".xlsx";

                    writer = new XSLXWriter(fileName);
                    
                    SqlParameter contractID = new SqlParameter("@ConractID", id);
                    SqlParameter yearParam = new SqlParameter("@Year", year);
                    SqlParameter monthParam = new SqlParameter("@Month", month);
                    string sql = "exec dbo.sp_PulseGetContractLabor {0},{1},{2}";
                    sql = String.Format(sql,  month, year, id);    
                    // трудоемкость
                    //List<Pulse.Models.Views.ContractLaborView> laborview = _context.ContractLaborViewList.FromSqlRaw("dbo.sp_PulseGetContractLabor @Month,@Year, @ConractID",monthParam, yearParam, contractID)
                    //    .IgnoreQueryFilters()
                    //    .OrderBy(e => e.OperationDate).AsEnumerable().ToList();
                    List<Pulse.Models.Views.ContractLaborView> laborview = _context.ContractLaborViewList.FromSqlRaw(sql)
                        .AsEnumerable()
                        .OrderBy(e => e.OperationDate).ToList();

                    if (laborview.Count == 0)
                    {
                        errorMessage = String.Format("ƒл€ мес€ца {0} года {1} записей нет", month, year);
                    }
                    else
                    {
                        if (writer.CreateXSLXFileContractLabor(laborview, out errorMessage))
                        {

                            return File("files/" + reportType + id.ToString() + month.ToString() + year.ToString() + ".xlsx", "text/plain", reportType + id.ToString() + month.ToString() + year.ToString() + ".xlsx");
                        }
                    }
                }
                catch (Exception ex) 
                { 
                    errorMessage = ex.Message;  
                }
            }
            else
            {
                errorMessage = "ќтчет не найден!";
            }

            ErrorMessage = errorMessage;
            return Page();
        }
    }
}
