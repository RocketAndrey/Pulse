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

namespace Pulse.Pages.Contracts
{
    public class IndexModel : BasePulsePage
    {
        

        public IndexModel(Pulse.Data.AsuContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration) : base(context, appEnvironment, configuration)
        {
          
        }
        [BindProperty]
        public string CurrentMode { get; set; }
        [BindProperty]
        public string CurrentSort { get; set; }
        
        public string NumberSort { get; set; }
        public string StartDateSort { get; set; }
        public string EndDateSort { get; set; }
        public string ClientSort { get; set; }
        public string CodeSort { get; set; }

        public string CurrentFilter { get; set; }

        [BindProperty]
        public ContractsFilter contractsFilter { get; set; }

        public PaginatedList<Contract> ContractsVW { get;set; }
        public List<Organization> Organizations { get; set; }

        private async void setPage(string sortOrder, string pageIndex, string mode)
        {
            ///индекс страницы
            string page = GetFromCookie(pageIndex, "index");

            int index;
            if (page == "all")
            {
                index = 1;
            }
            else
            {
                if (!int.TryParse(page, out index))
                { index = 1; }
                ;
            }

            ///режим показа (все или только в работе )
            CurrentMode = GetFromCookie(mode, "mode");
            /// сортировка 
            CurrentSort = GetFromCookie(sortOrder, "sort");

            NumberSort = CurrentSort == "Number" ? "Number_desc" : "Number";
            StartDateSort = CurrentSort == "StartDate" ? "StartDate_desc" : "StartDate";
            EndDateSort = CurrentSort == "EndDate" ? "EndDate_desc" : "EndDate";
            ClientSort = CurrentSort == "Client" ? "Client_desc" : "Client";
            CodeSort = CurrentSort == "Code" ? "Code_desc" : "Code";



            ///Фильтр 

            SetFilter();
            
            IList<Contract> contracts =  _context.Contracts.FromSqlRaw("dbo.sp_PulseGetContracts").AsNoTracking().ToList();

            contracts = CurrentSort switch
            {
                "Number" => contracts.OrderBy(s => s.Number).ToList(),
                "Number_desc" => contracts.OrderByDescending(s => s.Number).ToList(),
                "StartDate" => contracts.OrderBy(s => s.CreationDate).ToList(),
                "StartDate_desc" => contracts.OrderByDescending(s => s.CreationDate).ToList(),
                "EndDate" => contracts.OrderBy(s => s.EndDate).ToList(),
                "EndDate_desc" => contracts.OrderByDescending(s => s.EndDate).ToList(),
                "Client" => contracts.OrderBy(s => s.Client).ToList(),
                "Client_desc" => contracts.OrderByDescending(s => s.Client).ToList(),
                "Code" => contracts.OrderBy(s => s.Code).ToList(),
                "Code_desc" => contracts.OrderByDescending(s => s.Code).ToList(),

                _ => contracts.OrderByDescending(s => s.CreationDate).ToList()

            };
            if (CurrentMode != "all")
            {
                contracts = contracts.Where(e => e.Complete == false ).ToList();

            }
            
            if (contractsFilter.Number !="all")
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(contractsFilter.Number);

                contracts = contracts.Where(e => regex.IsMatch (e.Number)).ToList();
            }
            if (contractsFilter.Code != "all")
            {
                System.Text.RegularExpressions.Regex regex  = new System.Text.RegularExpressions.Regex(contractsFilter.Code);

                contracts = contracts.Where(e => regex.IsMatch(e.Code)).ToList();
            }
            if (contractsFilter.OrganizationID != 0 )
            {
                
                            contracts = contracts.Where(e => e.ClientID ==contractsFilter.OrganizationID ).ToList();
            }
            int pageSize = 20;

            ContractsVW = await PaginatedList<Contract>.CreateAsync(contracts, index, pageSize);

        }
        public async Task OnGetAsync(string sortOrder, string pageIndex, string mode)
        {
             setPage(sortOrder, pageIndex, mode);

        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                setPage(CurrentSort, "1", CurrentMode);
       
            }
            catch (Exception e)
            {
                return NotFound(e.Message);

            }
            return Page();

        }

        /// <summary>
        /// Возвращает значение из пирожка, сохраняет новое в пирожок
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetFromCookie (string? value, string key)
        {
            string returnvalue; 

            if (value == null)
            {
                if (Request.Cookies.ContainsKey("Contracts_" +key))
                {
                    returnvalue = Request.Cookies["Contracts_"+ key];

                }
                else
                {
                    returnvalue = "all";
                }
            }
            else { returnvalue = value; }

            if (Request.Cookies.ContainsKey("Contracts_"+key))
            {
                Response.Cookies.Delete("Contracts_"+key);
            }
            var cookieOptions = new CookieOptions
            {
                // Set the secure flag, which Chrome's changes will require for SameSite none.
                // Note this will also require you to be running on HTTPS.
               
                //.Secure = true,
                
                // Set the cookie to HTTP only which is good practice unless you really do need
                // to access it client side in scripts.
                HttpOnly = true,

                // Add the SameSite attribute, this will emit the attribute with a value of none.
                // To not emit the attribute at all set
                // SameSite = (SameSiteMode)(-1)
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("Contracts_"+key, returnvalue , cookieOptions);
           
            return returnvalue; 
        }
        private void SetFilter()
        {
            string orgID = null;

            if (contractsFilter == null)
            {
                contractsFilter = new ContractsFilter();
            }
            else 
            {
                contractsFilter.Number = contractsFilter.Number ?? "all";
                contractsFilter.Code = contractsFilter.Code ?? "all";
                orgID = contractsFilter.OrganizationID.ToString();

            }
            //пoлучаем значение фильтров из куков
            contractsFilter.Number = GetFromCookie(contractsFilter.Number, "ContractNumder");
            contractsFilter.Code = GetFromCookie(contractsFilter.Code, "ContractCode");
            
             orgID= GetFromCookie(orgID, "ContractOrganizationID");
            
            if (orgID == "all") { contractsFilter.OrganizationID = 0; }
            else { contractsFilter.OrganizationID = long.Parse(orgID); }


            string sql = "select Distinct  o.OrganizationId, o.ShortName from Organization o " +
            "inner join[Contract] c on c.ClientId = o.OrganizationId and c.ContractId > 1000 order by ShortName"; 
            
            Organizations =  _context.Organizations.FromSqlRaw(sql).AsNoTracking().ToList();
            Organizations.Add(new Organization { OrganizationID = 0, ShortName = "All" });

            ViewData["OrganizationID"] = new SelectList(Organizations.OrderBy(e=>e.ShortName ), "OrganizationID", "ShortName");
        }
    }
}
