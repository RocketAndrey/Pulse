using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pulse.Models
{
    public class Lot
    {
        [NotMapped]
        public List<Lot>ChildLots{ get; set; }
        [NotMapped]
        public List<RouteOperation> RouteOperations { get; set; }
        public Int64 LotID { get; set; }
        public Int64 WareID { get; set;} 
        public Int64 ContractID { get; set; }
        [Display(Name = "№ МК")]
        public string CardNumber 
        { 
            get
            {
                return PrefixNumber +  (TestType!=""? ('-' + TestType) :"") + '-' + Number.ToString() + SuffixNumber; 

            }
        }
        public string PrefixNumber { get; set; }
        [Display(Name = "Тест")]
        public string TestType { get; set; }
        [Display(Name = "Номер")]
        public Int64 Number { get; set; }
        
        public string SuffixNumber { get; set; }
        [Display(Name = "Кол-во")]
        public int QTY { get; set; }
        [Display(Name = "Номера изделий:")]
        public string ProductNumbers { get; set; }
        [Display(Name = "Типономинал:")]
        public string TypeNominal { get; set; }
        public string ClassName { get; set; }
        [Display(Name = "Партия:")]
        public string PartNumber { get; set; }
        [Display(Name = "Дата производства:")]
        public string ManufacturingDate { get; set; }
        public int EndCount { get; set; }
        public int RouteOperationCount { get; set; }

        [Display(Name = "Операций/выполнено:")]
        public string CompleteString
        {
            get
            {
                return RouteOperationCount.ToString() + "/" + EndCount.ToString();
            }
        }
        public bool Complete
        {
            get
            {  
                    return ((RouteOperationCount - EndCount == 0) & RouteOperationCount > 0);
               
            }
        }
        public string IDKey
        {
            get 
            {
                string[] letter = "a b c d f g h t y r i".Split();
                string lotid = LotID.ToString();
                string result = ""; 
                for (int i = 0;  i< lotid.Length;i++)
                {
                    result+= letter[int.Parse(lotid.Substring(i,1)) ];
                }
                return result; 

            }
        }
    }
}
