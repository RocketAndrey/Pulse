using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace Pulse.Pages.LaborLink
{
    public class UnlinkedOperationsModel : BasePulsePage
    {
        public List<Pulse.Models.GroupLaborOperation> Operations { get; set; }

        public UnlinkedOperationsModel(Pulse.Data.AsuContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration) : base(context, appEnvironment, configuration)
        {}
        public async Task<IActionResult> OnGet()
        {
            try
            {
                 Operations =  await _context.LaborOperations.FromSqlRaw("[dbo].[sp_PulseGetEmptyLaborOpreations]").ToListAsync();

            }
            catch (Exception e)
            {
                return NotFound(e.Message);

            }
            return Page();
        }
    }
}
