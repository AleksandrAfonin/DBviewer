
namespace DBviewer
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvTable = new System.Windows.Forms.DataGridView();
            this.lbTeacher = new System.Windows.Forms.Label();
            this.lbDiscipline = new System.Windows.Forms.Label();
            this.lbStudiesGroup = new System.Windows.Forms.Label();
            this.lbTypeOfActivity = new System.Windows.Forms.Label();
            this.cbTeacher = new System.Windows.Forms.ComboBox();
            this.cbDiscipline = new System.Windows.Forms.ComboBox();
            this.cbStudiesGroup = new System.Windows.Forms.ComboBox();
            this.cbTypeOfActivity = new System.Windows.Forms.ComboBox();
            this.btnIntel = new System.Windows.Forms.Button();
            this.cbMonth = new System.Windows.Forms.ComboBox();
            this.lbMonth = new System.Windows.Forms.Label();
            this.lbYear = new System.Windows.Forms.Label();
            this.lbStadiesForm = new System.Windows.Forms.Label();
            this.cbYear = new System.Windows.Forms.ComboBox();
            this.cbStudiesForm = new System.Windows.Forms.ComboBox();
            this.btnBackGeneral = new System.Windows.Forms.Button();
            this.btnPaid = new System.Windows.Forms.Button();
            this.btnInput = new System.Windows.Forms.Button();
            this.cbLevelEducation = new System.Windows.Forms.ComboBox();
            this.lbLevelEducation = new System.Windows.Forms.Label();
            this.cbHalfYear = new System.Windows.Forms.ComboBox();
            this.lbHalfYear = new System.Windows.Forms.Label();
            this.cbDay = new System.Windows.Forms.ComboBox();
            this.lbDay = new System.Windows.Forms.Label();
            this.tbHours = new System.Windows.Forms.TextBox();
            this.lbHours = new System.Windows.Forms.Label();
            this.btnBackReports = new System.Windows.Forms.Button();
            this.btnEnter = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTable)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTable
            // 
            this.dgvTable.AllowUserToAddRows = false;
            this.dgvTable.AllowUserToDeleteRows = false;
            this.dgvTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTable.Location = new System.Drawing.Point(12, 115);
            this.dgvTable.Name = "dgvTable";
            this.dgvTable.ReadOnly = true;
            this.dgvTable.Size = new System.Drawing.Size(992, 407);
            this.dgvTable.TabIndex = 0;
            // 
            // lbTeacher
            // 
            this.lbTeacher.AutoSize = true;
            this.lbTeacher.Location = new System.Drawing.Point(236, 71);
            this.lbTeacher.Name = "lbTeacher";
            this.lbTeacher.Size = new System.Drawing.Size(86, 13);
            this.lbTeacher.TabIndex = 1;
            this.lbTeacher.Text = "Преподаватель";
            this.lbTeacher.Visible = false;
            // 
            // lbDiscipline
            // 
            this.lbDiscipline.AutoSize = true;
            this.lbDiscipline.Location = new System.Drawing.Point(614, 9);
            this.lbDiscipline.Name = "lbDiscipline";
            this.lbDiscipline.Size = new System.Drawing.Size(70, 13);
            this.lbDiscipline.TabIndex = 2;
            this.lbDiscipline.Text = "Дисциплина";
            this.lbDiscipline.Visible = false;
            // 
            // lbStudiesGroup
            // 
            this.lbStudiesGroup.AutoSize = true;
            this.lbStudiesGroup.Location = new System.Drawing.Point(677, 71);
            this.lbStudiesGroup.Name = "lbStudiesGroup";
            this.lbStudiesGroup.Size = new System.Drawing.Size(87, 13);
            this.lbStudiesGroup.TabIndex = 3;
            this.lbStudiesGroup.Text = "Учебная группа";
            this.lbStudiesGroup.Visible = false;
            // 
            // lbTypeOfActivity
            // 
            this.lbTypeOfActivity.AutoSize = true;
            this.lbTypeOfActivity.Location = new System.Drawing.Point(822, 71);
            this.lbTypeOfActivity.Name = "lbTypeOfActivity";
            this.lbTypeOfActivity.Size = new System.Drawing.Size(70, 13);
            this.lbTypeOfActivity.TabIndex = 4;
            this.lbTypeOfActivity.Text = "Вид занятия";
            this.lbTypeOfActivity.Visible = false;
            // 
            // cbTeacher
            // 
            this.cbTeacher.FormattingEnabled = true;
            this.cbTeacher.Location = new System.Drawing.Point(239, 87);
            this.cbTeacher.Name = "cbTeacher";
            this.cbTeacher.Size = new System.Drawing.Size(181, 21);
            this.cbTeacher.TabIndex = 5;
            this.cbTeacher.Visible = false;
            this.cbTeacher.SelectedIndexChanged += new System.EventHandler(this.cbTeacher_SelectedIndexChanged);
            // 
            // cbDiscipline
            // 
            this.cbDiscipline.FormattingEnabled = true;
            this.cbDiscipline.Location = new System.Drawing.Point(617, 25);
            this.cbDiscipline.Name = "cbDiscipline";
            this.cbDiscipline.Size = new System.Drawing.Size(387, 21);
            this.cbDiscipline.TabIndex = 6;
            this.cbDiscipline.Visible = false;
            this.cbDiscipline.SelectedIndexChanged += new System.EventHandler(this.cbDiscipline_SelectedIndexChanged);
            // 
            // cbStudiesGroup
            // 
            this.cbStudiesGroup.FormattingEnabled = true;
            this.cbStudiesGroup.Location = new System.Drawing.Point(680, 87);
            this.cbStudiesGroup.Name = "cbStudiesGroup";
            this.cbStudiesGroup.Size = new System.Drawing.Size(139, 21);
            this.cbStudiesGroup.TabIndex = 7;
            this.cbStudiesGroup.Visible = false;
            this.cbStudiesGroup.SelectedIndexChanged += new System.EventHandler(this.cbStudiesGroup_SelectedIndexChanged);
            // 
            // cbTypeOfActivity
            // 
            this.cbTypeOfActivity.FormattingEnabled = true;
            this.cbTypeOfActivity.Location = new System.Drawing.Point(825, 87);
            this.cbTypeOfActivity.Name = "cbTypeOfActivity";
            this.cbTypeOfActivity.Size = new System.Drawing.Size(179, 21);
            this.cbTypeOfActivity.TabIndex = 8;
            this.cbTypeOfActivity.Visible = false;
            this.cbTypeOfActivity.SelectedIndexChanged += new System.EventHandler(this.cbTypeOfActivity_SelectedIndexChanged);
            // 
            // btnIntel
            // 
            this.btnIntel.Location = new System.Drawing.Point(12, 12);
            this.btnIntel.Name = "btnIntel";
            this.btnIntel.Size = new System.Drawing.Size(205, 23);
            this.btnIntel.TabIndex = 9;
            this.btnIntel.Text = "Сведения о проведенных занятиях";
            this.btnIntel.UseVisualStyleBackColor = true;
            this.btnIntel.Click += new System.EventHandler(this.btnFillForm_Click);
            // 
            // cbMonth
            // 
            this.cbMonth.FormattingEnabled = true;
            this.cbMonth.Items.AddRange(new object[] {
            "январь",
            "февраль",
            "март",
            "апрель",
            "май",
            "июнь",
            "июль",
            "август",
            "сентябрь",
            "октябрь",
            "ноябрь",
            "декабрь"});
            this.cbMonth.Location = new System.Drawing.Point(12, 87);
            this.cbMonth.Name = "cbMonth";
            this.cbMonth.Size = new System.Drawing.Size(72, 21);
            this.cbMonth.TabIndex = 10;
            this.cbMonth.Visible = false;
            this.cbMonth.SelectedIndexChanged += new System.EventHandler(this.cbMonth_SelectedIndexChanged);
            // 
            // lbMonth
            // 
            this.lbMonth.AutoSize = true;
            this.lbMonth.Location = new System.Drawing.Point(9, 70);
            this.lbMonth.Name = "lbMonth";
            this.lbMonth.Size = new System.Drawing.Size(40, 13);
            this.lbMonth.TabIndex = 11;
            this.lbMonth.Text = "Месяц";
            this.lbMonth.Visible = false;
            // 
            // lbYear
            // 
            this.lbYear.AutoSize = true;
            this.lbYear.Location = new System.Drawing.Point(87, 70);
            this.lbYear.Name = "lbYear";
            this.lbYear.Size = new System.Drawing.Size(25, 13);
            this.lbYear.TabIndex = 12;
            this.lbYear.Text = "Год";
            this.lbYear.Visible = false;
            // 
            // lbStadiesForm
            // 
            this.lbStadiesForm.AutoSize = true;
            this.lbStadiesForm.Location = new System.Drawing.Point(140, 70);
            this.lbStadiesForm.Name = "lbStadiesForm";
            this.lbStadiesForm.Size = new System.Drawing.Size(93, 13);
            this.lbStadiesForm.TabIndex = 13;
            this.lbStadiesForm.Text = "Форма обучения";
            this.lbStadiesForm.Visible = false;
            // 
            // cbYear
            // 
            this.cbYear.FormattingEnabled = true;
            this.cbYear.Items.AddRange(new object[] {
            "2023",
            "2024",
            "2025",
            "2026",
            "2027"});
            this.cbYear.Location = new System.Drawing.Point(90, 87);
            this.cbYear.Name = "cbYear";
            this.cbYear.Size = new System.Drawing.Size(47, 21);
            this.cbYear.TabIndex = 14;
            this.cbYear.Visible = false;
            this.cbYear.SelectedIndexChanged += new System.EventHandler(this.cbYear_SelectedIndexChanged);
            // 
            // cbStudiesForm
            // 
            this.cbStudiesForm.FormattingEnabled = true;
            this.cbStudiesForm.Items.AddRange(new object[] {
            "ВО",
            "СПО"});
            this.cbStudiesForm.Location = new System.Drawing.Point(143, 87);
            this.cbStudiesForm.Name = "cbStudiesForm";
            this.cbStudiesForm.Size = new System.Drawing.Size(90, 21);
            this.cbStudiesForm.TabIndex = 15;
            this.cbStudiesForm.Visible = false;
            this.cbStudiesForm.SelectedIndexChanged += new System.EventHandler(this.cbStudiesForm_SelectedIndexChanged);
            // 
            // btnBackGeneral
            // 
            this.btnBackGeneral.Location = new System.Drawing.Point(12, 36);
            this.btnBackGeneral.Name = "btnBackGeneral";
            this.btnBackGeneral.Size = new System.Drawing.Size(205, 23);
            this.btnBackGeneral.TabIndex = 16;
            this.btnBackGeneral.Text = "Назад";
            this.btnBackGeneral.UseVisualStyleBackColor = true;
            this.btnBackGeneral.Visible = false;
            this.btnBackGeneral.Click += new System.EventHandler(this.btnBackGeneral_Click);
            // 
            // btnPaid
            // 
            this.btnPaid.Location = new System.Drawing.Point(426, 85);
            this.btnPaid.Name = "btnPaid";
            this.btnPaid.Size = new System.Drawing.Size(75, 23);
            this.btnPaid.TabIndex = 17;
            this.btnPaid.Text = "Оплатить";
            this.btnPaid.UseVisualStyleBackColor = true;
            this.btnPaid.Visible = false;
            this.btnPaid.Click += new System.EventHandler(this.btnPaid_Click);
            // 
            // btnInput
            // 
            this.btnInput.Location = new System.Drawing.Point(223, 12);
            this.btnInput.Name = "btnInput";
            this.btnInput.Size = new System.Drawing.Size(271, 23);
            this.btnInput.TabIndex = 18;
            this.btnInput.Text = "Ввод сведений о проведенных учебных занятиях";
            this.btnInput.UseVisualStyleBackColor = true;
            this.btnInput.Visible = false;
            this.btnInput.Click += new System.EventHandler(this.btnInput_Click);
            // 
            // cbLevelEducation
            // 
            this.cbLevelEducation.FormattingEnabled = true;
            this.cbLevelEducation.Location = new System.Drawing.Point(360, 58);
            this.cbLevelEducation.Name = "cbLevelEducation";
            this.cbLevelEducation.Size = new System.Drawing.Size(60, 21);
            this.cbLevelEducation.TabIndex = 19;
            this.cbLevelEducation.Visible = false;
            // 
            // lbLevelEducation
            // 
            this.lbLevelEducation.AutoSize = true;
            this.lbLevelEducation.Location = new System.Drawing.Point(357, 42);
            this.lbLevelEducation.Name = "lbLevelEducation";
            this.lbLevelEducation.Size = new System.Drawing.Size(51, 13);
            this.lbLevelEducation.TabIndex = 20;
            this.lbLevelEducation.Text = "Уровень";
            this.lbLevelEducation.Visible = false;
            // 
            // cbHalfYear
            // 
            this.cbHalfYear.FormattingEnabled = true;
            this.cbHalfYear.Items.AddRange(new object[] {
            "первое",
            "второе"});
            this.cbHalfYear.Location = new System.Drawing.Point(426, 58);
            this.cbHalfYear.Name = "cbHalfYear";
            this.cbHalfYear.Size = new System.Drawing.Size(121, 21);
            this.cbHalfYear.TabIndex = 21;
            this.cbHalfYear.Visible = false;
            // 
            // lbHalfYear
            // 
            this.lbHalfYear.AutoSize = true;
            this.lbHalfYear.Location = new System.Drawing.Point(423, 41);
            this.lbHalfYear.Name = "lbHalfYear";
            this.lbHalfYear.Size = new System.Drawing.Size(61, 13);
            this.lbHalfYear.TabIndex = 22;
            this.lbHalfYear.Text = "Полугодие";
            this.lbHalfYear.Visible = false;
            // 
            // cbDay
            // 
            this.cbDay.FormattingEnabled = true;
            this.cbDay.Location = new System.Drawing.Point(553, 58);
            this.cbDay.Name = "cbDay";
            this.cbDay.Size = new System.Drawing.Size(54, 21);
            this.cbDay.TabIndex = 23;
            this.cbDay.Visible = false;
            // 
            // lbDay
            // 
            this.lbDay.AutoSize = true;
            this.lbDay.Location = new System.Drawing.Point(550, 42);
            this.lbDay.Name = "lbDay";
            this.lbDay.Size = new System.Drawing.Size(34, 13);
            this.lbDay.TabIndex = 24;
            this.lbDay.Text = "День";
            this.lbDay.Visible = false;
            // 
            // tbHours
            // 
            this.tbHours.Location = new System.Drawing.Point(895, 58);
            this.tbHours.Name = "tbHours";
            this.tbHours.Size = new System.Drawing.Size(47, 20);
            this.tbHours.TabIndex = 25;
            this.tbHours.Visible = false;
            // 
            // lbHours
            // 
            this.lbHours.AutoSize = true;
            this.lbHours.Location = new System.Drawing.Point(892, 41);
            this.lbHours.Name = "lbHours";
            this.lbHours.Size = new System.Drawing.Size(35, 13);
            this.lbHours.TabIndex = 26;
            this.lbHours.Text = "Часы";
            this.lbHours.Visible = false;
            // 
            // btnBackReports
            // 
            this.btnBackReports.Location = new System.Drawing.Point(223, 36);
            this.btnBackReports.Name = "btnBackReports";
            this.btnBackReports.Size = new System.Drawing.Size(128, 23);
            this.btnBackReports.TabIndex = 27;
            this.btnBackReports.Text = "Назад на Reports";
            this.btnBackReports.UseVisualStyleBackColor = true;
            this.btnBackReports.Visible = false;
            this.btnBackReports.Click += new System.EventHandler(this.btnBackReports_Click);
            // 
            // btnEnter
            // 
            this.btnEnter.Location = new System.Drawing.Point(521, 86);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(75, 23);
            this.btnEnter.TabIndex = 28;
            this.btnEnter.Text = "Внести";
            this.btnEnter.UseVisualStyleBackColor = true;
            this.btnEnter.Visible = false;
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 534);
            this.Controls.Add(this.btnEnter);
            this.Controls.Add(this.btnBackReports);
            this.Controls.Add(this.lbHours);
            this.Controls.Add(this.tbHours);
            this.Controls.Add(this.lbDay);
            this.Controls.Add(this.cbDay);
            this.Controls.Add(this.lbHalfYear);
            this.Controls.Add(this.cbHalfYear);
            this.Controls.Add(this.lbLevelEducation);
            this.Controls.Add(this.cbLevelEducation);
            this.Controls.Add(this.btnInput);
            this.Controls.Add(this.btnPaid);
            this.Controls.Add(this.btnBackGeneral);
            this.Controls.Add(this.cbStudiesForm);
            this.Controls.Add(this.cbYear);
            this.Controls.Add(this.lbStadiesForm);
            this.Controls.Add(this.lbYear);
            this.Controls.Add(this.lbMonth);
            this.Controls.Add(this.cbMonth);
            this.Controls.Add(this.btnIntel);
            this.Controls.Add(this.cbTypeOfActivity);
            this.Controls.Add(this.cbStudiesGroup);
            this.Controls.Add(this.cbDiscipline);
            this.Controls.Add(this.cbTeacher);
            this.Controls.Add(this.lbTypeOfActivity);
            this.Controls.Add(this.lbStudiesGroup);
            this.Controls.Add(this.lbDiscipline);
            this.Controls.Add(this.lbTeacher);
            this.Controls.Add(this.dgvTable);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTable;
        private System.Windows.Forms.Label lbTeacher;
        private System.Windows.Forms.Label lbDiscipline;
        private System.Windows.Forms.Label lbStudiesGroup;
        private System.Windows.Forms.Label lbTypeOfActivity;
        private System.Windows.Forms.ComboBox cbTeacher;
        private System.Windows.Forms.ComboBox cbDiscipline;
        private System.Windows.Forms.ComboBox cbStudiesGroup;
        private System.Windows.Forms.ComboBox cbTypeOfActivity;
        private System.Windows.Forms.Button btnIntel;
        private System.Windows.Forms.ComboBox cbMonth;
        private System.Windows.Forms.Label lbMonth;
        private System.Windows.Forms.Label lbYear;
        private System.Windows.Forms.Label lbStadiesForm;
        private System.Windows.Forms.ComboBox cbYear;
        private System.Windows.Forms.ComboBox cbStudiesForm;
        private System.Windows.Forms.Button btnBackGeneral;
        private System.Windows.Forms.Button btnPaid;
        private System.Windows.Forms.Button btnInput;
        private System.Windows.Forms.ComboBox cbLevelEducation;
        private System.Windows.Forms.Label lbLevelEducation;
        private System.Windows.Forms.ComboBox cbHalfYear;
        private System.Windows.Forms.Label lbHalfYear;
        private System.Windows.Forms.ComboBox cbDay;
        private System.Windows.Forms.Label lbDay;
        private System.Windows.Forms.TextBox tbHours;
        private System.Windows.Forms.Label lbHours;
        private System.Windows.Forms.Button btnBackReports;
        private System.Windows.Forms.Button btnEnter;
    }
}

