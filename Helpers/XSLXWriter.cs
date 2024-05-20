using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Reflection;
using System.IO;
using Pulse.Models;
using System.ComponentModel.DataAnnotations;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;



namespace Pulse.Helpers
{
    public class XSLXWriter
    {
        public XSLXWriter(string fileName) 
        {
            FileName = fileName;
        } 
        private  void CreateCell(IRow CurrentRow, int CellIndex, string Value, XSSFCellStyle Style)
        {
            NPOI.SS.UserModel.ICell Cell = CurrentRow.CreateCell(CellIndex);
            Cell.SetCellValue(Value);
            Cell.CellStyle = Style;
           // Cell.fo
            
        }
        private void CreateFormulaCell(IRow CurrentRow, int CellIndex, string formulaValue, XSSFCellStyle Style)
        {
            NPOI.SS.UserModel.ICell Cell = CurrentRow.CreateCell(CellIndex);
            Cell.SetCellType(CellType.Formula);
            Cell.CellFormula= formulaValue; 
          
            IDataFormat dataFormatCustom = CurrentRow.Sheet.Workbook.CreateDataFormat();
            XSSFCellStyle st = (XSSFCellStyle)Style.Clone();
            st.DataFormat = dataFormatCustom.GetFormat("0.00");// dataFormatCustom.GetFormat("0,00");
            Cell.CellStyle = st;

        }
        private void CreateDecimalCell(IRow CurrentRow, int CellIndex, decimal Value, XSSFCellStyle Style)
        {
            NPOI.SS.UserModel.ICell Cell = CurrentRow.CreateCell(CellIndex);
            Cell.SetCellType(CellType.Numeric);
            Cell.SetCellValue((double)Value);
            IDataFormat dataFormatCustom = CurrentRow.Sheet.Workbook.CreateDataFormat();
            XSSFCellStyle st = (XSSFCellStyle)Style.Clone();
            st.DataFormat = dataFormatCustom.GetFormat("0.00");// dataFormatCustom.GetFormat("0,00");
            Cell.CellStyle = st;
        }
    
    private void CreateIntegerCell(IRow CurrentRow, int CellIndex, decimal Value, XSSFCellStyle Style)
    {
        NPOI.SS.UserModel.ICell Cell = CurrentRow.CreateCell(CellIndex);
        Cell.SetCellType(CellType.Numeric);
        Cell.SetCellValue((double)Value);
        IDataFormat dataFormatCustom = CurrentRow.Sheet.Workbook.CreateDataFormat();

            XSSFCellStyle st = (XSSFCellStyle)Style.Clone();
            st.DataFormat = dataFormatCustom.GetFormat("0");
            Cell.CellStyle = st;
        }


    public string FileName { get; set; }
           /// <summary>
        /// выводит весь список с даннми для рассчета в Excel  файл 
        /// </summary>
        /// <param name="elementImport"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool CreateXSLXFileContractLabor(List<Pulse.Models.Views.ContractLaborView> labors, out string errorMessage)
        {
            errorMessage = "";

            if (labors == null)
            {
                errorMessage = "ContractLaborView не найден!";
                return false;
            }
          
            try
            {
                IWorkbook workbook = new XSSFWorkbook();
                XSSFFont myFont = (XSSFFont)workbook.CreateFont();
                myFont.FontHeightInPoints = 11;
                myFont.FontName = "Calibri";


                // Defining a border
                XSSFCellStyle borderedCellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                borderedCellStyle.SetFont(myFont);
                borderedCellStyle.BorderLeft = BorderStyle.Medium;
                borderedCellStyle.BorderTop = BorderStyle.Medium;
                borderedCellStyle.BorderRight = BorderStyle.Medium;
                borderedCellStyle.BorderBottom = BorderStyle.Medium;
                borderedCellStyle.VerticalAlignment = VerticalAlignment.Center;



                ISheet Sheet = workbook.CreateSheet("Report");
                //Creat The Headers of the excel
                IRow HeaderRow = Sheet.CreateRow(0);


                Type itemType = labors[0].GetType();
                // Описание заголовка 
# region описание заголовка 
                int columnIndex = 0;
                var prop = itemType.GetProperty("ContractNumber");
                if (prop != null) CreateCell(HeaderRow, columnIndex, GetProrertyDisplayName(prop), borderedCellStyle);
                columnIndex++;
                prop = itemType.GetProperty("ContractDate");
                if (prop != null) CreateCell(HeaderRow, columnIndex, GetProrertyDisplayName(prop), borderedCellStyle);

                columnIndex++;
                prop = itemType.GetProperty("Organization");
                if (prop != null) CreateCell(HeaderRow, columnIndex, GetProrertyDisplayName(prop), borderedCellStyle);
                columnIndex++;

                prop = itemType.GetProperty("ClassType");
                if (prop != null) CreateCell(HeaderRow, columnIndex, GetProrertyDisplayName(prop), borderedCellStyle);

                columnIndex++;
               
                    prop = itemType.GetProperty("ElementType");
                    if (prop != null) CreateCell(HeaderRow, columnIndex, GetProrertyDisplayName(prop), borderedCellStyle);
                    columnIndex++;
                    prop = itemType.GetProperty("Employee");
                    if (prop != null) CreateCell(HeaderRow, columnIndex, GetProrertyDisplayName(prop), borderedCellStyle);
                    columnIndex++;
              
                     prop = itemType.GetProperty("OperationName");
                    if (prop != null) CreateCell(HeaderRow, columnIndex, GetProrertyDisplayName(prop), borderedCellStyle);
                    columnIndex++;

                prop = itemType.GetProperty("CardNumber");
                if (prop != null) CreateCell(HeaderRow, columnIndex, GetProrertyDisplayName(prop), borderedCellStyle);
                columnIndex++;

                prop = itemType.GetProperty("Operationlabor");
                    if (prop != null) CreateCell(HeaderRow, columnIndex, GetProrertyDisplayName(prop), borderedCellStyle);
                    columnIndex++;
                
                
                    // есть  дочерняя заявка 
                    prop = itemType.GetProperty("OperationQTY");
                    if (prop != null) CreateCell(HeaderRow, columnIndex, GetProrertyDisplayName(prop), borderedCellStyle);
                    columnIndex++;
                
                prop = itemType.GetProperty("EndMonth");
                if (prop != null) CreateCell(HeaderRow, columnIndex, GetProrertyDisplayName(prop), borderedCellStyle);
                columnIndex++;
                prop = itemType.GetProperty("OperationDate");
                if (prop != null) CreateCell(HeaderRow, columnIndex, GetProrertyDisplayName(prop), borderedCellStyle);
             
                columnIndex++;
#endregion

                //Tзаполнение занными 
                int RowIndex = 1;

                //Iteration through some collection
                foreach (var item in labors)
                {
                    //Creating the CurrentDataRow
                    IRow CurrentRow = Sheet.CreateRow(RowIndex);
                    int colIndex = 0;

                    CreateCell(CurrentRow, colIndex++, item.ContractNumber.ToString(), borderedCellStyle);
                 
                    CreateCell(CurrentRow, colIndex++, item.ContractDate.ToString("d"), borderedCellStyle);
              
                    CreateCell(CurrentRow, colIndex++, item.Organization.ToString(), borderedCellStyle);

                    CreateCell(CurrentRow, colIndex++, item.ClassType.ToString(), borderedCellStyle);
                    CreateCell(CurrentRow, colIndex++, item.ElementType.ToString(), borderedCellStyle);
                    CreateCell(CurrentRow, colIndex++, item.Employee.ToString(), borderedCellStyle);
                    CreateCell(CurrentRow, colIndex++, item.OperationName.ToString(), borderedCellStyle);
                    CreateCell(CurrentRow, colIndex++, item.CardNumber.ToString(), borderedCellStyle);
                    CreateDecimalCell(CurrentRow, colIndex++, item.Operationlabor??0, borderedCellStyle);
                     
                    CreateIntegerCell(CurrentRow, colIndex++, item.OperationQTY??0, borderedCellStyle);
                    CreateCell(CurrentRow, colIndex++, item.EndMonth.ToString(), borderedCellStyle);
                    CreateCell(CurrentRow, colIndex++, item.OperationDate.ToString(), borderedCellStyle);


  
                    RowIndex++;
                }
                // Auto sized all the affected columns
                int lastColumNum = Sheet.GetRow(0).LastCellNum;

                for (int i = 0; i <= lastColumNum; i++)
                {
                    Sheet.AutoSizeColumn(i);
                    GC.Collect();
                }
                if (!String.IsNullOrEmpty(FileName))
                {
                    // Write Excel to disk 
                    using var fileData = new FileStream(FileName, FileMode.Create, FileAccess.Write);


                    workbook.Write(fileData);
                    fileData.Close();
                }
                return true;
            }
            catch (Exception ex) 
            { 
                errorMessage = ex.Message;
                return false;
            }
        }
   
        private string GetProrertyDisplayName(PropertyInfo pInfo)
        {
            // получаем все атрибуты класса
            object[] attributes = pInfo.GetCustomAttributes(false);

            // проходим по всем атрибутам
            foreach (Attribute attr in attributes)
            {
                // если атрибут представляет тип DisplayAttribute
                if (attr is DisplayAttribute dAttribute)
                    // возвращаем значение свойства Имя 
                    return  dAttribute.Name;
            }
            return "";
        }
        
    }
}
