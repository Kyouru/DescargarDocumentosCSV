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
                    //string extension = fileResp.ContentType.Split('/')[1];
                    string extension = "";

                    if (listaExtension.ContainsKey(fileResp.ContentType.ToLower()))
                    {
                        extension = listaExtension[fileResp.ContentType.ToLower()];
                    }
                    else
                    {
                        extension = fileResp.ContentType.Split('/')[1];
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


        static public Dictionary<string, string> listaExtension = new Dictionary<string, string>()
        {
            {"text/plain","TXT"},
            {"image/tiff","TIFF"},
            {"application/x-bittorrent","TORRENT"},
            {"application/x-font-ttf","TTF"},
            {"application/x-cdlink","VCD"},
            {"text/x-vcard","VCF"},
            {"application/xml","XML"},
            {"audio/x-wav","WAV"},
            {"audio/x-ms-wma","WMA"},
            {"video/x-ms-wmv","WMV"},
            {"application/wordperfect","WPD"},
            {"application/xhtml+xml","XHTML"},
            {"application/vnd.ms-excel","XLS"},
            {"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","XLSX"},
            {"text/xml","XML"},
            {"video/3gpp2","3G2"},
            {"video/3gpp","3GP"},
            {"application/illustrator","AI"},
            {"audio/x-aiff","AIF"},
            {"application/vnd.android.package-archive","APK"},
            {"video/x-ms-asf","ASF"},
            {"video/x-msvideo","AVI"},
            {"image/bmp","BMP"},
            {"text/x-csrc","C"},
            {"application/x-x509-ca-cert","CER"},
            {"text/x-c++src","CPP"},
            {"application/x-chrome-extension","CRX"},
            {"text/css","CSS"},
            {"text/csv","CSV"},
            {"image/vnd.ms-dds","DDS"},
            {"application/x-debian-package","DEB"},
            {"application/msword","DOC"},
            {"application/vnd.openxmlformats-officedocument.wordprocessingml.document","DOCX"},
            {"application/xml-dtd","DTD"},
            {"application/dxf","DXF"},
            {"video/x-flv","FLV"},
            {"application/octet-stream+fnt","FNT"},
            {"application/octet-stream+fon","FON"},
            {"image/gif","GIF"},
            {"application/gpx+xml","GPX"},
            {"application/x-gzip","GZ"},
            {"text/x-chdr","H"},
            {"application/mac-binhex40","HQX"},
            {"text/html","HTML"},
            {"image/x-icon","ICO"},
            {"text/calendar","ICS"},
            {"image/jpeg","JPG"},
            {"application/vnd.google-earth.kml+xml","KML"},
            {"application/vnd.google-earth.kmz","KMZ"},
            {"audio/x-mpegurl","M3U"},
            {"audio/mp4","M4A"},
            {"video/x-m4v","M4V"},
            {"audio/midi","MID"},
            {"video/quicktime","MOV"},
            {"audio/mpeg","MP3"},
            {"video/mp4","MP4"},
            {"video/mpeg","MPG"},
            {"application/vnd.oasis.opendocument.text","ODT"},
            {"application/vnd.oasis.opendocument.formula-template","OTF"},
            {"application/x-iwork-pages-sffpages","PAGES"},
            {"chemical/x-pdb","PDB"},
            {"application/pdf","PDF"},
            {"application/x-httpd-php","PHP"},
            {"image/png","PNG"},
            {"application/vnd.ms-powerpoint","PPT"},
            {"application/vnd.openxmlformats-officedocument.presentationml.presentation","PPTX"},
            {"application/pics-rules","PRF"},
            {"application/postscript","PS"},
            {"application/photoshop","PSD"},
            {"audio/x-pn-realaudio","RM"},
            {"application/x-rpm","RPM"},
            {"application/rtf","RTF"},
            {"application/x-sh","SH"},
            {"image/svg+xml","SVG"},
            {"application/x-shockwave-flash","SWF"},
            {"application/x-tex","TEX"},
            {"x-compressed-tar","TAR.GZ"},
            {"x-rar","RAR"}
        };

    }
}
