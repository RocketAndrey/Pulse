using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pulse.Models.Estimator; 

namespace Pulse.Pages.LaborLink
{
    public class DetailsModel : BasePulsePage 
    {
        [BindProperty]
        public int ClassId {get;set;}
        [BindProperty]
        public int OperationID { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ClassName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string OperationName { get; set; }
        [BindProperty]
        public int ProgramID { get; set; }
        [BindProperty]
        public int ElementTypeID { get; set; }
        [BindProperty]
        public int TestChainItemID { get; set; }

        private List<TestProgram> _estimatorPrograms;
        private List<ElementType> _estimatorElementTypes;
        private List<TestChainItem > _estimatorTestChainItems;
        public List<OperationsList> _asuOperations;

        public DetailsModel(Pulse.Data.AsuContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration) : base(context, appEnvironment, configuration)
        {
            
        }
        public void OnGet(int operationID, int classId)
        {
            this.OperationID = operationID;
            this.ClassId = classId;
            //Ищем в БД пррограмму и элемент для данного класса
            string sql = "select Top(1) et.ProgramID, et.ElementTypeID from Estimator_TestChainItem e," +
                " Estimator_TestChainItemData t,Estimator_ElementType et " +
                "where t.TestChainItemID = e.TestChainItemID and et.ElementTypeID = t.ElementTypeID " +
                " and e.AsuClassID ={0} ";
            sql = String.Format(sql, ClassId);
            List<Pulse.Models.Estimator.ChainItemFilter> filters = _context.ChainItemFilter.FromSqlRaw(sql).ToList();
            if (filters.Count>0)
            {
                ProgramID = filters[0].ProgramID;
                ElementTypeID = filters[0].ElementTypeID; 
            }

            FillPrograms(); 
        }
        public IActionResult OnPost(int? action)
        {
            if (action == null)
            {
                FillPrograms();
                return Page(); 
            }
            else 
            {
                string sql = "select * from Estimator_TestChainItem " +
                "where TestChainItemID = {0} and AsuClassID = {1} and AsuBaseOperationID = {2}";
                sql = String.Format(sql, TestChainItemID, ClassId, OperationID);
                int count = _context.Database.ExecuteSqlRaw(sql); 
                if (count < 1)
                {

                    sql = "INSERT INTO[dbo].[Estimator_TestChainItem] ([TestChainItemID] ,[AsuClassID],[AsuBaseOperationID] )" +
                    "VALUES ({0},{1},{2})";
                    sql = String.Format(sql, TestChainItemID, ClassId, OperationID);

                    count = _context.Database.ExecuteSqlRaw(sql);
                    
                }
                return RedirectToPage("UnlinkedOperations");
            }

        }
        private void FillPrograms ()
        {
            string sql = "select * from Estimator_TestProgram";
            _estimatorPrograms = _context.EstimatorTestPrograms.FromSqlRaw(sql).ToList();
            ViewData["ProgramID"] = new SelectList(_estimatorPrograms, "TestProgramID", "Description");
            
            if(_estimatorPrograms.Count>0 && ProgramID ==0)
            {
                ProgramID = _estimatorPrograms[0].TestProgramID;  
            }
            if (ProgramID > 0 )
            {
                sql = String.Format("select * from Estimator_ElementType where ProgramID = {0}",ProgramID) ;
                _estimatorElementTypes = _context.Estimator_ElementTypes.FromSqlRaw(sql).ToList();
                ViewData["ElementTypeID"] = new SelectList(_estimatorElementTypes , "ElementTypeID", "Name");
            }
            if (_estimatorElementTypes.Count > 0 && ElementTypeID ==0 )
            {
                ElementTypeID = _estimatorElementTypes[0].ElementTypeID; 
            }
            if (ElementTypeID >0)
            {
                sql =String.Format("select tci.TestChainItemID,eo.OperationID,eo.Name " +
                " from Estimator_TestChainItemData tci, Estimator_Operation eo " +
                " where eo.OperationID = tci.OperationID and tci.ElementTypeID = {0} Order BY tci.[Order]",ElementTypeID );
                _estimatorTestChainItems = _context.ChainItems.FromSqlRaw(sql).ToList();
                ViewData["TestChainItemID"] = new SelectList(_estimatorTestChainItems, "TestChainItemID", "Name");
            }
            //ВЫВОДИМ  список операций 
            sql = "select ro.RouteOperationId, cl.ClassId ,cl.Name as ClassType,bo.BaseOperationId, " +
            " w.TypeNominal, ui.LastName + ' ' + ui.FN as [Employee], bo.Name as [OperationName], " +
            " l.PrefixNumber + '-' + t.TestTypeId + '-' + CAST(l.Number AS varchar) + ISNULL(CASE WHEN l.SuffixNumber LIKE 'о' THEN NULL ELSE l.SuffixNumber END, '') as [CardNumber], " +
            " w.QTY,l.QTY as OperationQTY,  convert(nvarchar(2), MONTH(ro.EndTime)) + ' ' + convert(nvarchar(4), YEAR(ro.EndTime)) as EndMonth, " +
            " CAST(ro.EndTime as DATE) as [Дата],w.WareId," +
            " ct.Number as ContractNumber, ct.CreationDate as ContractDate," +
            " org.ShortName as Organization from RouteOperation as ro " +
            " INNER JOIN UserInfo as ui on ro.UserID = ui.UserId " +
            " INNER JOIN Operation as o on ro.OperationId = o.OperationId " +
            " INNER JOIN Test as t on t.TestId = o.TestId " +
            " INNER JOIN BaseOperation as bo on bo.BaseOperationId = o.baseOperationId " +
            " INNER JOIN Lot as l on ro.LotId = l.LotId " +
            " INNER JOIN Wares as w on l.WareId = w.WareId " +
            " Inner JOIN[Contract] ct on ct.ContractId = w.ContractId " +
            " inner join Organization org on org.OrganizationId = ct.SupplierId " +
            "  inner join ClassType cl on cl.ClassId = w.ClassId " +
            " left OUTER  join Estimator_TestChainItem  eet on eet.AsuClassID = cl.ClassId and eet.[AsuBaseOperationID] = bo.BaseOperationId " +
            " left outer join(select eta.TestChainItemID, sum(eta.BatchLabor) as banchLabor, sum(eta.ItemLabor) as ItemLabor " +
            " from Estimator.dbo.TestAction eta group by eta.TestChainItemID) labor on " +
            " labor.TestChainItemID = eet.[TestChainItemID] " +
            " where isnull(ro.Disabled, 0) = 0 " +
            " and ro.EndTime > '2021-06-01' " +
            " and isnull(ro.EndTime,0)<> 0 " +
            " and IsNull(labor.banchLabor,-1)= -1 " +
            " and cl.ClassId = {0} AND  bo.BaseOperationId = {1}";
            sql = String.Format(sql, ClassId, OperationID);
            _asuOperations = _context.OperationsList.FromSqlRaw(sql).ToList();
        }

    }
}
