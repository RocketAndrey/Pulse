using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pulse.Data;
using Pulse.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Pulse.Pages.Contracts
{
    public class IndexModel : BasePulsePage
    {
        

        public IndexModel(Pulse.Data.AsuContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration) : base(context, appEnvironment, configuration)
        {
          
        }
        
        public string CurrentSort { get; set; }
        
        public string NumberSort { get; set; }
        public string StartDateSort { get; set; }
        public string EndDateSort { get; set; }
        public string ClientSort { get; set; }

        public string CurrentFilter { get; set; }
        public PaginatedList<Contract> ContractsVW { get;set; }

        public async Task OnGetAsync(string sortOrder, int? pageIndex)
        {

            CurrentSort = sortOrder;
            NumberSort = String.IsNullOrEmpty(sortOrder) ? "Number_desc" : "";
            StartDateSort = sortOrder == "StartDate" ? "StartDate_desc" : "StartDate";
            EndDateSort = sortOrder == "EndDate" ? "EndDate_desc" : "EndDate";
            ClientSort = sortOrder == "Client" ? "Client_desc" : "Client";

            string sql = "SELECT DISTINCT"+
                         " c.ContractId, c.Number, c.CreationDate, c.Code, c.Crypt, c.CloseDate , c.Priority," +
                         " c.ApprovedPI, c.PZ, d.Name AS ContractState, c.ValidityPeriod, c.SignatureDate, p.Number AS Parent, " +
                         "cl.ShortName AS Client, ex.ShortName AS Executor, sp.ShortName AS Supplier," +
                         " c.IsDeleted, c.EndDate FROM  dbo.Contract AS c LEFT OUTER JOIN" +
                          " dbo.Contract AS p ON c.ParentId = p.ContractId INNER JOIN" +
                         " dbo.DocumentState AS d ON c.ContractStateId = d.DocumentStateId INNER JOIN" +
                         " dbo.Organization AS cl ON c.ClientId = cl.OrganizationId INNER JOIN" +
                         " dbo.Organization AS ex ON c.ExecutorId = ex.OrganizationId INNER JOIN" +
                         " dbo.Organization AS sp ON c.SupplierId = sp.OrganizationId where c.IsDeleted< >1 and c.ContractId >1000";
            IList<Contract>contracts = await _context.Contracts.FromSqlRaw(sql).AsNoTracking().ToListAsync();

            contracts = sortOrder switch
            {
                "Number" => contracts.OrderBy(s => s.Number).ToList(),
                "Number_desc" => contracts.OrderByDescending (s => s.Number).ToList(),
                "StartDate" => contracts.OrderBy(s => s.CreationDate).ToList(),
                "StartDate_desc" => contracts.OrderByDescending(s => s.CreationDate).ToList(),
                "EndDate" => contracts.OrderBy(s => s.EndDate).ToList(),
                "EndDate_desc" => contracts.OrderByDescending(s => s.EndDate).ToList(),
                "Client" => contracts.OrderBy(s => s.Client).ToList(),
                "Client_desc" => contracts.OrderByDescending(s => s.Client).ToList(),
                _=> contracts.OrderByDescending(s => s.CreationDate).ToList()

            };

            int pageSize = 20;

            ContractsVW = await PaginatedList<Contract>.CreateAsync(contracts, pageIndex ?? 1, pageSize);
        }
    }
}
