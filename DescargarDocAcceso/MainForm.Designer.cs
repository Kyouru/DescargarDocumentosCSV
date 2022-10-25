namespace DescargarDocAcceso
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btSeleccionarArchivo = new System.Windows.Forms.Button();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.tbRutaDescarga = new System.Windows.Forms.TextBox();
            this.btRutaDescarga = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbColumnaID = new System.Windows.Forms.TextBox();
            this.tbColumnaTipo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbColumnaURL = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbProgreso = new System.Windows.Forms.Label();
            this.btExportarDgv = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // btSeleccionarArchivo
            // 
            this.btSeleccionarArchivo.Location = new System.Drawing.Point(15, 134);
            this.btSeleccionarArchivo.Name = "btSeleccionarArchivo";
            this.btSeleccionarArchivo.Size = new System.Drawing.Size(220, 39);
            this.btSeleccionarArchivo.TabIndex = 0;
            this.btSeleccionarArchivo.Text = "Seleccionar Archivo a Procesar";
            this.btSeleccionarArchivo.UseVisualStyleBackColor = true;
            this.btSeleccionarArchivo.Click += new System.EventHandler(this.btSeleccionarArchivo_Click);
            // 
            // dgv
            // 
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(12, 191);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(608, 174);
            this.dgv.TabIndex = 1;
            // 
            // tbRutaDescarga
            // 
            this.tbRutaDescarga.Location = new System.Drawing.Point(12, 12);
            this.tbRutaDescarga.Name = "tbRutaDescarga";
            this.tbRutaDescarga.Size = new System.Drawing.Size(270, 20);
            this.tbRutaDescarga.TabIndex = 2;
            this.tbRutaDescarga.Text = "C:\\Temp";
            // 
            // btRutaDescarga
            // 
            this.btRutaDescarga.Location = new System.Drawing.Point(288, 6);
            this.btRutaDescarga.Name = "btRutaDescarga";
            this.btRutaDescarga.Size = new System.Drawing.Size(111, 30);
            this.btRutaDescarga.TabIndex = 3;
            this.btRutaDescarga.Text = "Ruta Descarga";
            this.btRutaDescarga.UseVisualStyleBackColor = true;
            this.btRutaDescarga.Click += new System.EventHandler(this.btRutaDescarga_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Columna ID";
            // 
            // tbColumnaID
            // 
            this.tbColumnaID.Location = new System.Drawing.Point(90, 43);
            this.tbColumnaID.Name = "tbColumnaID";
            this.tbColumnaID.Size = new System.Drawing.Size(34, 20);
            this.tbColumnaID.TabIndex = 5;
            this.tbColumnaID.Text = "1";
            // 
            // tbColumnaTipo
            // 
            this.tbColumnaTipo.Location = new System.Drawing.Point(90, 69);
            this.tbColumnaTipo.Name = "tbColumnaTipo";
            this.tbColumnaTipo.Size = new System.Drawing.Size(34, 20);
            this.tbColumnaTipo.TabIndex = 7;
            this.tbColumnaTipo.Text = "5";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Columna Tipo";
            // 
            // tbColumnaURL
            // 
            this.tbColumnaURL.Location = new System.Drawing.Point(90, 95);
            this.tbColumnaURL.Name = "tbColumnaURL";
            this.tbColumnaURL.Size = new System.Drawing.Size(34, 20);
            this.tbColumnaURL.TabIndex = 9;
            this.tbColumnaURL.Text = "6";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Columna URL";
            // 
            // lbProgreso
            // 
            this.lbProgreso.AutoSize = true;
            this.lbProgreso.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbProgreso.Location = new System.Drawing.Point(263, 138);
            this.lbProgreso.Name = "lbProgreso";
            this.lbProgreso.Size = new System.Drawing.Size(91, 25);
            this.lbProgreso.TabIndex = 11;
            this.lbProgreso.Text = "Progreso";
            this.lbProgreso.Visible = false;
            // 
            // btExportarDgv
            // 
            this.btExportarDgv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExportarDgv.Location = new System.Drawing.Point(476, 136);
            this.btExportarDgv.Name = "btExportarDgv";
            this.btExportarDgv.Size = new System.Drawing.Size(144, 35);
            this.btExportarDgv.TabIndex = 12;
            this.btExportarDgv.Text = "Exportar dgv";
            this.btExportarDgv.UseVisualStyleBackColor = true;
            this.btExportarDgv.Click += new System.EventHandler(this.btExportarDgv_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 377);
            this.Controls.Add(this.btExportarDgv);
            this.Controls.Add(this.lbProgreso);
            this.Controls.Add(this.tbColumnaURL);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbColumnaTipo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbColumnaID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btRutaDescarga);
            this.Controls.Add(this.tbRutaDescarga);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.btSeleccionarArchivo);
            this.Name = "MainForm";
            this.Text = "Descargar Documentos";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btSeleccionarArchivo;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.TextBox tbRutaDescarga;
        private System.Windows.Forms.Button btRutaDescarga;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbColumnaID;
        private System.Windows.Forms.TextBox tbColumnaTipo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbColumnaURL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbProgreso;
        private System.Windows.Forms.Button btExportarDgv;
    }
}

