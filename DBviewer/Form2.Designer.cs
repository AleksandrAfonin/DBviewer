
namespace DBviewer
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.dgvForm2 = new System.Windows.Forms.DataGridView();
            this.lbFIO = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvForm2)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvForm2
            // 
            this.dgvForm2.AllowUserToAddRows = false;
            this.dgvForm2.AllowUserToDeleteRows = false;
            this.dgvForm2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvForm2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvForm2.Location = new System.Drawing.Point(12, 59);
            this.dgvForm2.Name = "dgvForm2";
            this.dgvForm2.ReadOnly = true;
            this.dgvForm2.Size = new System.Drawing.Size(978, 463);
            this.dgvForm2.TabIndex = 0;
            // 
            // lbFIO
            // 
            this.lbFIO.AutoSize = true;
            this.lbFIO.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbFIO.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.lbFIO.Location = new System.Drawing.Point(12, 9);
            this.lbFIO.Name = "lbFIO";
            this.lbFIO.Size = new System.Drawing.Size(0, 31);
            this.lbFIO.TabIndex = 1;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 534);
            this.Controls.Add(this.lbFIO);
            this.Controls.Add(this.dgvForm2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1015, 250);
            this.Name = "Form2";
            this.Text = "Детальная плановая нагрузка преподавателя";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.SizeChanged += new System.EventHandler(this.Form2_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dgvForm2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvForm2;
        private System.Windows.Forms.Label lbFIO;
    }
}