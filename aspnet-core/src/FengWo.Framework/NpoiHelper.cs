using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using System.Collections;
using NPOI.XSSF.UserModel;

namespace FengWo.App.Framework
{
    /// <summary>
    /// NPOI Office操作类
    /// </summary>
    public class NpoiHelper
    {
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt"></param>
        public static byte[] Export(DataTable dt)
        {
            byte[] Result = null;

            if (dt != null && dt.Rows.Count > 0)
            {
                var book = new HSSFWorkbook();

                #region Sheet名称
                var TableName = dt.TableName;
                var ExtendedProperties = dt.ExtendedProperties;
                if (ExtendedProperties != null && ExtendedProperties.Count > 0)
                {
                    if (ExtendedProperties.ContainsKey("CommentName") == true)
                    {
                        TableName = Convert.ToString(ExtendedProperties["CommentName"]);
                    }
                }
                ISheet sheet = book.CreateSheet(dt.TableName == "" ? "Sheet1" : dt.TableName);
                #endregion

                #region 创建列头
                IRow rowHead = sheet.CreateRow(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var dc = dt.Columns[i];
                    rowHead.CreateCell(i).SetCellValue(dc.Caption == "" ? dc.ColumnName : dc.Caption);
                }
                #endregion

                #region 写入数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow rowBody = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = rowBody.CreateCell(j);
                        cell.SetCellValue(Convert.ToString(dt.Rows[i][j]));
                    }
                }
                #endregion

                #region 设置列宽
                //列宽自适应，只对英文和数字有效
                for (int i = 0; i <= dt.Rows.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                //获取当前列的宽度，然后对比本列的长度，取最大值
                for (int columnNum = 0; columnNum <= dt.Rows.Count; columnNum++)
                {
                    int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                    for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                    {
                        IRow currentRow;
                        //当前行未被使用过
                        if (sheet.GetRow(rowNum) == null)
                        {
                            currentRow = sheet.CreateRow(rowNum);
                        }
                        else
                        {
                            currentRow = sheet.GetRow(rowNum);
                        }

                        if (currentRow.GetCell(columnNum) != null)
                        {
                            ICell currentCell = currentRow.GetCell(columnNum);
                            int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                            if (columnWidth < length)
                            {
                                columnWidth = length;
                            }
                        }
                    }
                    sheet.SetColumnWidth(columnNum, columnWidth * 256);
                }
                #endregion

                #region 生成对象
                // 写入到客户端  
                using (MemoryStream ms = new MemoryStream())
                {
                    book.Write(ms);
                    Result = ms.ToArray();
                    book = null;
                }
                #endregion
            }

            return Result;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FileFullName"></param>
        public static string Export(DataTable dt, string FileFullName)
        {
            return Export(dt, FileFullName, 0, 1, true);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FileFullName"></param>
        /// <param name="IsShowTableColumn"></param>
        /// <returns></returns>
        public static string Export(DataTable dt, string FileFullName, bool IsShowTableColumn)
        {
            return Export(dt, FileFullName, 0, 0, IsShowTableColumn);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FileFullName"></param>
        /// <param name="ColumnsStartRowIndex"></param>
        public static string Export(DataTable dt, string FileFullName, int ColumnsStartRowIndex)
        {
            return Export(dt, FileFullName, ColumnsStartRowIndex, ColumnsStartRowIndex + 1, true);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FileFullName"></param>
        /// <param name="ColumnsStartRowIndex"></param>
        /// <param name="DatasStartRowIndex"></param>
        /// <param name="IsShowTableColumn"></param>
        /// <returns></returns>
        public static string Export(DataTable dt, string FileFullName, int ColumnsStartRowIndex, int DatasStartRowIndex, bool IsShowTableColumn)
        {
            if (string.IsNullOrWhiteSpace(FileFullName) == true)
            {
                FileFullName = AppDomain.CurrentDomain.BaseDirectory + @"TempFiles\" + DateTime.Now.ToString("yyyyMMdd") + @"\" + Guid.NewGuid().ToString() + ".xls";
            }

            var fileInfo = new FileInfo(FileFullName);
            fileInfo.Directory.Create();

            if (dt != null)
            {
                IWorkbook book = null;

                var fileExt = fileInfo.Extension.ToLower();
                //XSSFWorkbook 适用XLSX格式，HSSFWorkbook 适用XLS格式
                if (fileExt == ".xlsx")
                {
                    book = new XSSFWorkbook();
                }
                else if (fileExt == ".xls")
                {
                    book = new HSSFWorkbook();
                }
                else
                {
                    book = null;
                }

                if (book == null)
                {
                    return null;
                }

                #region Sheet名称
                var TableName = dt.TableName;
                var ExtendedProperties = dt.ExtendedProperties;
                if (ExtendedProperties != null && ExtendedProperties.Count > 0)
                {
                    if (ExtendedProperties.ContainsKey("CommentName") == true)
                    {
                        TableName = Convert.ToString(ExtendedProperties["CommentName"]);
                    }
                }
                ISheet sheet = book.CreateSheet(dt.TableName == "" ? "Sheet1" : dt.TableName);
                #endregion

                #region 创建列头
                if(IsShowTableColumn == true)
                {
                    IRow rowHead = sheet.CreateRow(ColumnsStartRowIndex);
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        var dc = dt.Columns[i];
                        rowHead.CreateCell(i).SetCellValue(dc.Caption == "" ? dc.ColumnName : dc.Caption);
                    }
                }
                #endregion

                #region 写入数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow rowBody = sheet.CreateRow(DatasStartRowIndex + i);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        rowBody.CreateCell(j).SetCellValue(Convert.ToString(dt.Rows[i][j]));
                    }
                }
                #endregion

                #region 设置列宽
                //列宽自适应，只对英文和数字有效
                for (int i = 0; i <= dt.Rows.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                //获取当前列的宽度，然后对比本列的长度，取最大值
                for (int columnNum = 0; columnNum <= dt.Rows.Count; columnNum++)
                {
                    int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                    for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                    {
                        IRow currentRow;
                        //当前行未被使用过
                        if (sheet.GetRow(rowNum) == null)
                        {
                            currentRow = sheet.CreateRow(rowNum);
                        }
                        else
                        {
                            currentRow = sheet.GetRow(rowNum);
                        }

                        if (currentRow.GetCell(columnNum) != null)
                        {
                            ICell currentCell = currentRow.GetCell(columnNum);
                            int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                            if (columnWidth < length)
                            {
                                columnWidth = length;
                            }
                        }
                    }
                    sheet.SetColumnWidth(columnNum, columnWidth * 256);
                }
                #endregion

                #region 生成对象
                // 写入到客户端  
                using (MemoryStream ms = new MemoryStream())
                {
                    book.Write(ms);
                    using (FileStream fs = new FileStream(FileFullName, FileMode.Create, FileAccess.Write))
                    {
                        byte[] data = ms.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                    book = null;
                }
                #endregion
            }

            return FileFullName;
        }
    }
}
