using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Pulse.Models
{
    public class Ware
    {
        public Int64  WareID { get; set; }
        public Int64 ContractID { get; set; }
        public string TypeNominal { get; set; }
        public int QTY { get; set; }
        public string ClassName { get; set; }
        public string ContractNumber {get;set;} 
        public int InStoreQTY { get; set; }
        public int EndCount { get; set; }
        public int RouteOperationCount { get; set; }
        [Display(Name = "МК")]
        public int LotCount { get; set; }

        public Decimal ReadyRatio
        {
            get 
            {
                if (EndCount == 0) return 0;
                if  ((EndCount / RouteOperationCount)>1) return 1;
                return EndCount / RouteOperationCount;
            }
        }
        [Display(Name = "Операций/выполнено:")]
        public string CompleteString
        { 
            get
            {
                return RouteOperationCount.ToString() + "/" + EndCount.ToString(); 
            }
        }
        [Display(Name = "Всего/получено:")]
        public string InStoreString
        {
            get
            {
                return QTY.ToString() + "/" + InStoreQTY.ToString();
            }
        }
        public bool Complete
        {
            get
            {
                if (QTY - InStoreQTY != 0 | InStoreQTY == 0) return false;
                else
                {
                    return ((RouteOperationCount - EndCount == 0) & RouteOperationCount > 0);
                }
            }
        }
        public bool InProgress
        {
            get 
            {
                return (QTY - InStoreQTY != 0 | (RouteOperationCount - EndCount != 0));
             
            }
        }
    }
}
