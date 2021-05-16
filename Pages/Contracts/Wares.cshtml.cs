using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pulse.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Pulse.Pages.Contracts
{
    public class WaresModel : BasePulsePage
    {
        public Contract Contract;
        public WaresModel(Pulse.Data.AsuContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration, ILogger<IndexModel> logger) : base(context, appEnvironment, configuration)
        {

        }
        public List<Ware> Wares;
        public List<TestProgram >Programs;
        public int ContractID;
        public string ContractName;
        public async Task<IActionResult> OnGetAsync(int? contractID)
        {
            if (contractID == null)
            {
                return NotFound();
            }
            string sql = "dbo.sp_PulseGetContractWares @ContractID";
            SqlParameter scontractID = new SqlParameter("@ContractID", contractID);
           
            Wares = _context.Wares.FromSqlRaw(sql, scontractID).ToList();

            Contract = _context.Contracts.FromSqlRaw("dbo.sp_PulseGetContracts @ContractID", new SqlParameter("@ContractID", contractID)).ToList()[0];

            sql = String.Format("select p.ProgramId, pc.ContractId, p.Number, p.Name,p.Ka from Program p, Program_Contract pc where p.ProgramId = pc.ProgramId and pc.ContractId ={0}", contractID );

            Programs = _context.TestPrograms.FromSqlRaw(sql).ToList(); 

            if (Contract == null)
            {
                return NotFound();
            }

            ContractID = (int)contractID;
            if (Wares.Count !=0 )
            {
                ContractName = Wares[0].ContractNumber; 
            }
            return Page();
        }
    }
}