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
    public class LotModel : BasePulsePage
    {
        public List<Lot> Lots;
        public Contract Contract;

        public int WareID;
        public Ware Ware; 
        public LotModel(Pulse.Data.AsuContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration, ILogger<IndexModel> logger) : base(context, appEnvironment, configuration)
        {

        }
        public Int64 CurrentLotID;
        public Lot CurrentLot;
        public async Task<IActionResult> OnGet(int? id, Int64? lotid)

        {
            if (id == null)
            {
                return NotFound();
            }
            if (lotid == null)
            {
                lotid = 0;
            }
            WareID = (int)id;
            //ХЗ почему так, но FirstOrDefault не работает  
            string sql =String.Format("dbo.sp_PulseGetContractWares @WareID={0}",WareID) ;
             List<Ware> Wares=_context.Wares.FromSqlRaw(sql).ToList();

          
            if (Wares.Count == 0)
            {
                return NotFound();
            }
            Ware = Wares[0];
           
            Contract = _context.Contracts.FromSqlRaw("dbo.sp_PulseGetContracts @ContractID", new SqlParameter("@ContractID", Ware.ContractID)).ToList()[0];


            if (Contract == null)
            {
                return NotFound();
            }

            Lots = _context.Lots.FromSqlRaw("dbo.sp_PulseGetWareLots @WareID", new SqlParameter("@WareID", WareID)).ToList();
            if (Lots.Count != 0)
            {
                if (Lots.Count == 1 || lotid == 0) { CurrentLotID = Lots[0].LotID; } else { CurrentLotID = (Int64)lotid; };

            
         
                CurrentLot = (Lot)Lots.FirstOrDefault(e => e.LotID == CurrentLotID);

                if (CurrentLot != null)
                {
                    CurrentLot.ChildLots = _context.Lots.FromSqlRaw("[dbo].[sp_PulseGetLotSubLots] @LotID", new SqlParameter("@LotID", CurrentLotID)).ToList();

                    foreach (Lot childlot in CurrentLot.ChildLots)
                    {
                        childlot.RouteOperations = _context.RouteOperations.FromSqlRaw("[dbo].[sp_PulseGetLotRO] @LotID", new SqlParameter("@LotID", childlot.LotID)).ToList();
                    }

                }
            }


            return Page();
        }
    }

            
        
}
