using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using System.Linq;

namespace Pulse.Pages
{
    public class ReportsModel : PageModel
    {
        
        public void OnGet()
        {
            //SelectList mlist = new SelectList();

            ViewData["Month"] = new SelectList(MonthNames.ToList(), CurentMonth);
            ViewData["Year"] = new SelectList(Years, CurentYear );
        }
        public string[] MonthNames
        { 
            get
            {
                string[] values= System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                string[] result = new string[12];
                for (int i = 0; i <12; i++)
                {
                    result[i] = values[i];
                }
                return result;


            }
        }
        public string CurentMonth
        {
            
                get
                {
                return MonthNames[System.DateTime.Now.Month-1];
                }
            
        }
        public int CurentYear
        {

            get
            {
                return System.DateTime.Now.Year;
            }

        }
        public string[] Years
        {
            get
            {
                int year = System.DateTime.Now.Year;
                string[] returnValue = new string[year-2019+1];   

                for (int i=2019;i<=year;i++ )

                {
                    returnValue[i - 2019] = i.ToString();
                }
                return returnValue; 
            }
        }

    }
}
