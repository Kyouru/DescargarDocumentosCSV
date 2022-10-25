using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Threading;

namespace DescargarDocAcceso
{
    public partial class MainForm : Form
    {
        System.Data.DataTable dt;
        System.Data.DataTable dtcod;
        System.Data.DataTable dterror;
        string archivo;
        public MainForm()
        {
            InitializeComponent();
            dterror = new System.Data.DataTable();
            dterror.Columns.Add("ID");
            dterror.Columns.Add("COD");
            dterror.Columns.Add("TIPO");
            dterror.Columns.Add("ERROR");
            dterror.Columns.Add("URL");
        }

        private void btSeleccionarArchivo_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = "Excel Workbook|*.xlsx|All files (*.*)|*.*";
            ofd.Filter = "CSV delimitado por comas|*.csv|All files (*.*)|*.*";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            string rutadescarga = tbRutaDescarga.Text;

            if (!Directory.Exists(rutadescarga))
            {
                MessageBox.Show("Ruta Descarga no existe");
                return;
            }

            if (tbColumnaID.Text == "")
            {
                MessageBox.Show("Columna ID invalida");
                return;
            }
            else if (!Functions.IsNumeric(tbColumnaID.Text))
            {
                MessageBox.Show("Columna ID no numerico");
                return;
            }
            if (tbColumnaURL.Text == "")
            {
                MessageBox.Show("Columna URL invalida");
                return;
            }
            else if (!Functions.IsNumeric(tbColumnaURL.Text))
            {
                MessageBox.Show("Columna URL no numerico");
                return;
            }

            if (tbColumnaTipo.Text == "")
            {
                MessageBox.Show("Columna Tipo invalida");
                return;
            }
            else if (!Functions.IsNumeric(tbColumnaTipo.Text))
            {
                MessageBox.Show("Columna Tipo no numerico");
                return;
            }

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                dgv.DataSource = null;
                lbProgreso.Visible = true;
                archivo = ofd.FileName;
                Thread trd = new Thread(new ThreadStart(this.procesarDataTable));
                trd.IsBackground = true;
                trd.Start();
            }
        }

        private void btRutaDescarga_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    tbRutaDescarga.Text = fbd.SelectedPath;
                }
            }
        }

        private void procesarDataTable ()
        {
            try
            {
                string rutadescarga = tbRutaDescarga.Text;
                dt = Functions.CSVToDataTable(archivo);
                if (dt == null)
                    return;
                dtcod = Functions.codificacionToDataTable();
                if (dtcod == null)
                    return;
                int maxrowcod = dtcod.Rows.Count;
                int idcol = Int32.Parse(tbColumnaID.Text) - 1;
                int tipocol = Int32.Parse(tbColumnaTipo.Text) - 1;
                int urlcol = Int32.Parse(tbColumnaURL.Text) - 1;
                int contador = 0;
                dterror.Rows.Clear();
                lbProgreso.Invoke((MethodInvoker)delegate {
                    lbProgreso.Text = contador + " de " + dt.Rows.Count.ToString();
                });
                DataRow[] foundRows;

                foreach (DataRow dr in dt.Rows)
                {
                    if (!Directory.Exists(Path.Combine(rutadescarga, dr[idcol].ToString())))
                    {
                        Directory.CreateDirectory(Path.Combine(rutadescarga, dr[idcol].ToString()));
                    }
                    string cod;
                    foundRows = dtcod.Select("Nombre = '" + dr[tipocol].ToString() + "'");
                    if (foundRows.Length > 0)
                    {
                        cod = foundRows[0][1].ToString().Replace("\r", "").Replace("\n", "").Replace("\r\n", "");
                    }
                    else
                    {
                        maxrowcod++;
                        cod = (maxrowcod).ToString().PadLeft(3, '0');
                    }
                    string result = Functions.SaveUrlToFile(dr[urlcol].ToString(), Path.Combine(rutadescarga, dr[idcol].ToString()), cod);
                    if (result != "OK")
                    {
                        DataRow drerror = dterror.NewRow();
                        drerror["ID"] = dr[idcol].ToString();
                        drerror["COD"] = cod;
                        drerror["TIPO"] = dr[tipocol].ToString();
                        drerror["ERROR"] = result;
                        drerror["URL"] = dr[urlcol].ToString();
                        dterror.Rows.Add(drerror);
                        dgv.Invoke((MethodInvoker)delegate {
                            dgv.DataSource = null;
                            dgv.DataSource = dterror;
                        });
                    }
                    contador++;
                    lbProgreso.Invoke((MethodInvoker)delegate {
                        lbProgreso.Text = contador + " de " + dt.Rows.Count.ToString();
                    });
                }
                MessageBox.Show("Proceso Completado");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btExportarDgv_Click(object sender, EventArgs e)
        {
            if (dgv.Rows.Count > 0)
            {
                Functions.ExportarDataGridViewCSV(dgv, null);
            }
            else
            {
                MessageBox.Show("dgv vacio");
            }
        }
    }
}
