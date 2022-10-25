using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace DescargarDocAcceso
{
    internal class Functions
    {
        public static System.Data.DataTable ConvertExcelToDataTable(string FileName, int index)
        {
            try
            {
                if (!File.Exists(FileName))
                    return null;

                FileInfo fi = new FileInfo(FileName);
                long filesize = fi.Length;

                Microsoft.Office.Interop.Excel.Application xlApp;
                Workbook xlWorkBook;
                Worksheet xlWorkSheet;
                Range range;
                var misValue = Type.Missing;

                // abrir el documento 
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(FileName, misValue, misValue,
                    misValue, misValue, misValue, misValue, misValue, misValue,
                    misValue, misValue, misValue, misValue, misValue, misValue);

                // seleccion de la hoja de calculo
                // get_item() devuelve object y numera las hojas a partir de 1
                xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(index);

                // seleccion rango activo
                range = xlWorkSheet.UsedRange;

                int rows = range.Rows.Count;

                System.Data.DataTable dt = new System.Data.DataTable();

                int i = 1;

                //no mas de 50 columnas
                while (i < 50 && xlWorkSheet.Cells[1, i].Text != "")
                {
                    dt.Columns.Add(Convert.ToString(xlWorkSheet.Cells[1, i].Text));
                    ++i;
                }
                --i;

                for (int row = 2; row <= rows; row++)
                {
                    DataRow newrow = dt.NewRow();
                    for (int col = 1; col <= i; col++)
                    {
                        // lectura como cadena
                        string cellText = xlWorkSheet.Cells[row, col].Text;
                        cellText = Convert.ToString(cellText);
                        //cellText = cellText.Replace("'", ""); // Comillas simples no pueden pasar en el Texto

                        newrow[col - 1] = cellText;
                    }
                    dt.Rows.Add(newrow);
                }

                xlWorkBook.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();

                // liberar
                ReleaseObject(xlWorkSheet);
                ReleaseObject(xlWorkBook);
                ReleaseObject(xlApp);

                return dt;
            }
            catch (Exception ex)
            {
                //GlobalFunctions.casoError(ex, "ConvertExcelToDataTable");
                return null;
            }
        }
        private static void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to release the object(object:{0})\n" + ex.Message, obj.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        public static System.Data.DataTable CSVToDataTable(string path)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string csvData;
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    csvData = sr.ReadToEnd().ToString();
                    
                    string[] row = csvData.Split('\n');
                    for (int i = 0; i < row.Length - 1; i++)
                    {
                        string[] rowData = row[i].Split(',');
                        {
                            if (i == 0)
                            {
                                for (int j = 0; j < rowData.Length; j++)
                                {
                                    dt.Columns.Add(rowData[j].Trim());
                                }
                            }
                            else
                            {
                                DataRow dr = dt.NewRow();
                                for (int k = 0; k < rowData.Length; k++)
                                {
                                    dr[k] = rowData[k].ToString();
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                    }

                    return dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public static System.Data.DataTable codificacionToDataTable()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                using (var reader = new StreamReader(Path.Combine(System.Windows.Forms.Application.StartupPath, "codificacion.csv")))
                {
                    string csv = reader.ReadToEnd();
                    string[] row = csv.Split('\n');
                    for (int i = 0; i < row.Length - 1; i++)
                    {
                        string[] rowData = row[i].Split(',');
                        {
                            if (i == 0)
                            {
                                for (int j = 0; j < rowData.Length; j++)
                                {
                                    dt.Columns.Add(rowData[j].Trim());
                                }
                            }
                            else
                            {
                                DataRow dr = dt.NewRow();
                                for (int k = 0; k < rowData.Length; k++)
                                {
                                    dr[k] = rowData[k].ToString();
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dt;
        }

        public static string SaveUrlToFile(string uri, string filePath, string fileName)
        {
            var fileReq = HttpWebRequest.Create(uri) as HttpWebRequest;

            try
            {
                var fileResp = (HttpWebResponse)fileReq.GetResponse();

                using (var stream = fileResp.GetResponseStream())
                {
                    string extension = fileResp.ContentType.Split('/')[1];
                    if (extension == "x-compressed-tar")
                    {
                        extension = "tar.gz";
                    }
                    else if (extension == "x-rar")
                    {
                        extension = "rar"; 
                    }
                    int cont = 1;
                    string path = filePath + "\\" + fileName + "." + extension;
                    while (File.Exists(path) & cont < 50)
                    {
                        path = filePath + "\\" + fileName + " (" + cont + ")" + "." + extension;
                        cont++;
                    }
                    using (var fileStream = File.OpenWrite(path))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                return "OK";
            }
            catch (WebException ex)
            {
                if (!(ex.Response is null))
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        //return ex.Message + "\n" + reader.ReadToEnd();
                        return ex.Message;
                    }
                }
                else
                {
                    return ex.Message;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static void ExportarDataGridViewCSV(DataGridView dgv, string fileName)
        {
            if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\Exportar\\"))
            {
                Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\\Exportar\\");
            }
            if (fileName is null)
            {
                fileName = System.Windows.Forms.Application.StartupPath + "\\Exportar\\" + "EXPORTAR_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";
            }

            try
            {
                string[] outputCsv = new string[dgv.Rows.Count + 1];
                string columnNames = "";
                outputCsv = new string[dgv.Rows.Count + 1];

                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    columnNames += dgv.Columns[i].HeaderText.ToString() + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
                }
                outputCsv[0] += columnNames;

                //Recorremos el DataTable rellenando la hoja de trabajo
                for (int i = 1; i <= dgv.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dgv.Columns.Count; j++)
                    {
                        if (dgv.Rows[i - 1].Cells[j] != null)
                        {
                            if (dgv.Rows[i - 1].Cells[j].Value != null)
                            {
                                outputCsv[i] += dgv.Rows[i - 1].Cells[j].Value.ToString() + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
                            }
                        }
                    }
                }
                File.WriteAllLines(fileName, outputCsv, Encoding.UTF8);

                Process.Start(fileName);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        public static bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
    }
}
