using Microsoft.AspNetCore.Mvc.ModelBinding;
using NuGet.Protocol;
using System;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Pulse.Models.Views
{
    public class ContractMonthLabor

    {
        [Display(Name = "Месяц")]
        public string EndMonth { get; set; }
        [Display(Name = "Трудоёмкость,час")]
        public decimal? labor { get; set; }

        public int Month
        {
            get
            {
                int _month;
                string[] words = EndMonth.Split(new char[] { ' ' });

                if (words.Length == 2)
                {


                    if (int.TryParse(words[0],out _month))
                    {
                        return _month;  
                    }
                                    
                }
                return 0;
            }
        }
        public int Year
        {
            get
            {
                int _year;
                string[] words = EndMonth.Split(new char[] { ' ' });

                if (words.Length == 2)
                {


                    if (int.TryParse(words[1], out _year))
                    {
                        return _year;
                    }

                }
                return 0;
            }
        }

        public DateTime EndDate
        {
            get { return new DateTime(Year, Month, 28); }
        }
    }
}
