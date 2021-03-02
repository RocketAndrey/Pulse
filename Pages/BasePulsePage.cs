using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Pulse.Pages
{
    public class BasePulsePage:PageModel 
    {
        protected Pulse.Data.AsuContext  _context;
        protected IWebHostEnvironment _appEnvironment;
        protected IConfiguration _configuration;
        protected Pulse.Data.AsuContext _asuContext;
 
        public string[] MonthName = new string[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };

        public BasePulsePage (Pulse.Data.AsuContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _configuration = configuration;
          

        }
    }
}
