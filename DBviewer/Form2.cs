using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBviewer
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        DataTable detailInfoTable = new DataTable();
        string fio;
        Size dgvForm2Size;

        public Form2(string fio, DataTable detailInfoTable)
        {
            InitializeComponent();
            this.fio = fio;
            this.detailInfoTable = detailInfoTable;
        }

        private void Form2_SizeChanged(object sender, EventArgs e)
        {
            dgvForm2Size.Width = this.Width - 40;
            dgvForm2Size.Height = this.Height - 109;
            dgvForm2.Size = dgvForm2Size;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            lbFIO.Text = fio;
            dgvForm2.DataSource = detailInfoTable;
            dgvForm2.Columns["Дисциплина"].Width = 200;
            dgvForm2.Columns["Группа"].Width = 80;
        }
    }
}
