
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
            this.lbLevelEducation = new System.Windows.Forms.Label();
            this.cbDay = new System.Windows.Forms.ComboBox();
            this.lbDay = new System.Windows.Forms.Label();
            this.tbHours = new System.Windows.Forms.TextBox();
            this.lbHours = new System.Windows.Forms.Label();
            this.btnBackReports = new System.Windows.Forms.Button();
            this.btnEnter = new System.Windows.Forms.Button();
            this.btnViewAll = new System.Windows.Forms.Button();
            this.btnAdditionalInfo = new System.Windows.Forms.Button();
            this.btnSaveAdditionalInfo = new System.Windows.Forms.Button();
            this.btnBackAdditionalInfo = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.lbGeneralIntel = new System.Windows.Forms.Label();
            this.lbAdditionalInfo = new System.Windows.Forms.Label();
            this.lbIntelHours = new System.Windows.Forms.Label();
            this.lbEnterIntel = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lbPassword = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.tbLevelEducation = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            this.dgvTable.Size = new System.Drawing.Size(976, 407);
            this.dgvTable.TabIndex = 0;
            // 
            // lbTeacher
            // 
            this.lbTeacher.AutoSize = true;
            this.lbTeacher.Location = new System.Drawing.Point(425, 72);
            this.lbTeacher.Name = "lbTeacher";
            this.lbTeacher.Size = new System.Drawing.Size(86, 13);
            this.lbTeacher.TabIndex = 1;
            this.lbTeacher.Text = "Преподаватель";
            this.lbTeacher.Visible = false;
            // 
            // lbDiscipline
            // 
            this.lbDiscipline.AutoSize = true;
            this.lbDiscipline.Location = new System.Drawing.Point(586, 4);
            this.lbDiscipline.Name = "lbDiscipline";
            this.lbDiscipline.Size = new System.Drawing.Size(70, 13);
            this.lbDiscipline.TabIndex = 2;
            this.lbDiscipline.Text = "Дисциплина";
            this.lbDiscipline.Visible = false;
            // 
            // lbStudiesGroup
            // 
            this.lbStudiesGroup.AutoSize = true;
            this.lbStudiesGroup.Location = new System.Drawing.Point(586, 43);
            this.lbStudiesGroup.Name = "lbStudiesGroup";
            this.lbStudiesGroup.Size = new System.Drawing.Size(87, 13);
            this.lbStudiesGroup.TabIndex = 3;
            this.lbStudiesGroup.Text = "Учебная группа";
            this.lbStudiesGroup.Visible = false;
            // 
            // lbTypeOfActivity
            // 
            this.lbTypeOfActivity.AutoSize = true;
            this.lbTypeOfActivity.Location = new System.Drawing.Point(731, 43);
            this.lbTypeOfActivity.Name = "lbTypeOfActivity";
            this.lbTypeOfActivity.Size = new System.Drawing.Size(70, 13);
            this.lbTypeOfActivity.TabIndex = 4;
            this.lbTypeOfActivity.Text = "Вид занятия";
            this.lbTypeOfActivity.Visible = false;
            // 
            // cbTeacher
            // 
            this.cbTeacher.FormattingEnabled = true;
            this.cbTeacher.Location = new System.Drawing.Point(428, 88);
            this.cbTeacher.Name = "cbTeacher";
            this.cbTeacher.Size = new System.Drawing.Size(155, 21);
            this.cbTeacher.TabIndex = 5;
            this.cbTeacher.Visible = false;
            this.cbTeacher.SelectedIndexChanged += new System.EventHandler(this.cbTeacher_SelectedIndexChanged);
            // 
            // cbDiscipline
            // 
            this.cbDiscipline.FormattingEnabled = true;
            this.cbDiscipline.Location = new System.Drawing.Point(589, 20);
            this.cbDiscipline.Name = "cbDiscipline";
            this.cbDiscipline.Size = new System.Drawing.Size(399, 21);
            this.cbDiscipline.TabIndex = 6;
            this.cbDiscipline.Visible = false;
            this.cbDiscipline.SelectedIndexChanged += new System.EventHandler(this.cbDiscipline_SelectedIndexChanged);
            // 
            // cbStudiesGroup
            // 
            this.cbStudiesGroup.FormattingEnabled = true;
            this.cbStudiesGroup.Location = new System.Drawing.Point(589, 59);
            this.cbStudiesGroup.Name = "cbStudiesGroup";
            this.cbStudiesGroup.Size = new System.Drawing.Size(139, 21);
            this.cbStudiesGroup.TabIndex = 7;
            this.cbStudiesGroup.Visible = false;
            this.cbStudiesGroup.SelectedIndexChanged += new System.EventHandler(this.cbStudiesGroup_SelectedIndexChanged);
            // 
            // cbTypeOfActivity
            // 
            this.cbTypeOfActivity.FormattingEnabled = true;
            this.cbTypeOfActivity.Location = new System.Drawing.Point(734, 59);
            this.cbTypeOfActivity.Name = "cbTypeOfActivity";
            this.cbTypeOfActivity.Size = new System.Drawing.Size(144, 21);
            this.cbTypeOfActivity.TabIndex = 8;
            this.cbTypeOfActivity.Visible = false;
            this.cbTypeOfActivity.SelectedIndexChanged += new System.EventHandler(this.cbTypeOfActivity_SelectedIndexChanged);
            // 
            // btnIntel
            // 
            this.btnIntel.Location = new System.Drawing.Point(171, 12);
            this.btnIntel.Name = "btnIntel";
            this.btnIntel.Size = new System.Drawing.Size(199, 23);
            this.btnIntel.TabIndex = 9;
            this.btnIntel.Text = "Сведения о проведенных занятиях";
            this.btnIntel.UseVisualStyleBackColor = true;
            this.btnIntel.Click += new System.EventHandler(this.btnIntel_Click);
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
            this.cbMonth.Location = new System.Drawing.Point(224, 88);
            this.cbMonth.Name = "cbMonth";
            this.cbMonth.Size = new System.Drawing.Size(72, 21);
            this.cbMonth.TabIndex = 10;
            this.cbMonth.Visible = false;
            this.cbMonth.SelectedIndexChanged += new System.EventHandler(this.cbMonth_SelectedIndexChanged);
            // 
            // lbMonth
            // 
            this.lbMonth.AutoSize = true;
            this.lbMonth.Location = new System.Drawing.Point(221, 72);
            this.lbMonth.Name = "lbMonth";
            this.lbMonth.Size = new System.Drawing.Size(40, 13);
            this.lbMonth.TabIndex = 11;
            this.lbMonth.Text = "Месяц";
            this.lbMonth.Visible = false;
            // 
            // lbYear
            // 
            this.lbYear.AutoSize = true;
            this.lbYear.Location = new System.Drawing.Point(168, 72);
            this.lbYear.Name = "lbYear";
            this.lbYear.Size = new System.Drawing.Size(25, 13);
            this.lbYear.TabIndex = 12;
            this.lbYear.Text = "Год";
            this.lbYear.Visible = false;
            // 
            // lbStadiesForm
            // 
            this.lbStadiesForm.AutoSize = true;
            this.lbStadiesForm.Location = new System.Drawing.Point(299, 72);
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
            this.cbYear.Location = new System.Drawing.Point(171, 88);
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
            this.cbStudiesForm.Location = new System.Drawing.Point(302, 88);
            this.cbStudiesForm.Name = "cbStudiesForm";
            this.cbStudiesForm.Size = new System.Drawing.Size(120, 21);
            this.cbStudiesForm.TabIndex = 15;
            this.cbStudiesForm.Visible = false;
            this.cbStudiesForm.SelectedIndexChanged += new System.EventHandler(this.cbStudiesForm_SelectedIndexChanged);
            // 
            // btnBackGeneral
            // 
            this.btnBackGeneral.Location = new System.Drawing.Point(171, 12);
            this.btnBackGeneral.Name = "btnBackGeneral";
            this.btnBackGeneral.Size = new System.Drawing.Size(199, 23);
            this.btnBackGeneral.TabIndex = 16;
            this.btnBackGeneral.Text = "Назад";
            this.btnBackGeneral.UseVisualStyleBackColor = true;
            this.btnBackGeneral.Visible = false;
            this.btnBackGeneral.Click += new System.EventHandler(this.btnBackGeneral_Click);
            // 
            // btnPaid
            // 
            this.btnPaid.Location = new System.Drawing.Point(884, 86);
            this.btnPaid.Name = "btnPaid";
            this.btnPaid.Size = new System.Drawing.Size(104, 23);
            this.btnPaid.TabIndex = 17;
            this.btnPaid.Text = "Оплатить";
            this.btnPaid.UseVisualStyleBackColor = true;
            this.btnPaid.Visible = false;
            this.btnPaid.Click += new System.EventHandler(this.btnPaid_Click);
            // 
            // btnInput
            // 
            this.btnInput.Location = new System.Drawing.Point(376, 12);
            this.btnInput.Name = "btnInput";
            this.btnInput.Size = new System.Drawing.Size(271, 23);
            this.btnInput.TabIndex = 18;
            this.btnInput.Text = "Ввод сведений о проведенных учебных занятиях";
            this.btnInput.UseVisualStyleBackColor = true;
            this.btnInput.Visible = false;
            this.btnInput.Click += new System.EventHandler(this.btnInput_Click);
            // 
            // lbLevelEducation
            // 
            this.lbLevelEducation.AutoSize = true;
            this.lbLevelEducation.Location = new System.Drawing.Point(359, 72);
            this.lbLevelEducation.Name = "lbLevelEducation";
            this.lbLevelEducation.Size = new System.Drawing.Size(51, 13);
            this.lbLevelEducation.TabIndex = 20;
            this.lbLevelEducation.Text = "Уровень";
            this.lbLevelEducation.Visible = false;
            // 
            // cbDay
            // 
            this.cbDay.FormattingEnabled = true;
            this.cbDay.Location = new System.Drawing.Point(302, 88);
            this.cbDay.Name = "cbDay";
            this.cbDay.Size = new System.Drawing.Size(54, 21);
            this.cbDay.TabIndex = 23;
            this.cbDay.Visible = false;
            // 
            // lbDay
            // 
            this.lbDay.AutoSize = true;
            this.lbDay.Location = new System.Drawing.Point(299, 72);
            this.lbDay.Name = "lbDay";
            this.lbDay.Size = new System.Drawing.Size(34, 13);
            this.lbDay.TabIndex = 24;
            this.lbDay.Text = "День";
            this.lbDay.Visible = false;
            // 
            // tbHours
            // 
            this.tbHours.Location = new System.Drawing.Point(884, 60);
            this.tbHours.Name = "tbHours";
            this.tbHours.Size = new System.Drawing.Size(104, 20);
            this.tbHours.TabIndex = 25;
            this.tbHours.Visible = false;
            // 
            // lbHours
            // 
            this.lbHours.AutoSize = true;
            this.lbHours.Location = new System.Drawing.Point(881, 44);
            this.lbHours.Name = "lbHours";
            this.lbHours.Size = new System.Drawing.Size(35, 13);
            this.lbHours.TabIndex = 26;
            this.lbHours.Text = "Часы";
            this.lbHours.Visible = false;
            // 
            // btnBackReports
            // 
            this.btnBackReports.Location = new System.Drawing.Point(171, 12);
            this.btnBackReports.Name = "btnBackReports";
            this.btnBackReports.Size = new System.Drawing.Size(412, 23);
            this.btnBackReports.TabIndex = 27;
            this.btnBackReports.Text = "Назад";
            this.btnBackReports.UseVisualStyleBackColor = true;
            this.btnBackReports.Visible = false;
            this.btnBackReports.Click += new System.EventHandler(this.btnBackReports_Click);
            // 
            // btnEnter
            // 
            this.btnEnter.Location = new System.Drawing.Point(589, 86);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(399, 23);
            this.btnEnter.TabIndex = 28;
            this.btnEnter.Text = "Внести";
            this.btnEnter.UseVisualStyleBackColor = true;
            this.btnEnter.Visible = false;
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // btnViewAll
            // 
            this.btnViewAll.Location = new System.Drawing.Point(589, 86);
            this.btnViewAll.Name = "btnViewAll";
            this.btnViewAll.Size = new System.Drawing.Size(139, 23);
            this.btnViewAll.TabIndex = 29;
            this.btnViewAll.Text = "Показать всех";
            this.btnViewAll.UseVisualStyleBackColor = true;
            this.btnViewAll.Visible = false;
            this.btnViewAll.Click += new System.EventHandler(this.btnViewAll_Click);
            // 
            // btnAdditionalInfo
            // 
            this.btnAdditionalInfo.Location = new System.Drawing.Point(376, 12);
            this.btnAdditionalInfo.Name = "btnAdditionalInfo";
            this.btnAdditionalInfo.Size = new System.Drawing.Size(271, 23);
            this.btnAdditionalInfo.TabIndex = 30;
            this.btnAdditionalInfo.Text = "Дополнительная информация о преподавателях";
            this.btnAdditionalInfo.UseVisualStyleBackColor = true;
            this.btnAdditionalInfo.Click += new System.EventHandler(this.btnAdditionalInfo_Click);
            // 
            // btnSaveAdditionalInfo
            // 
            this.btnSaveAdditionalInfo.Location = new System.Drawing.Point(376, 12);
            this.btnSaveAdditionalInfo.Name = "btnSaveAdditionalInfo";
            this.btnSaveAdditionalInfo.Size = new System.Drawing.Size(199, 23);
            this.btnSaveAdditionalInfo.TabIndex = 31;
            this.btnSaveAdditionalInfo.Text = "Сохранить";
            this.btnSaveAdditionalInfo.UseVisualStyleBackColor = true;
            this.btnSaveAdditionalInfo.Visible = false;
            this.btnSaveAdditionalInfo.Click += new System.EventHandler(this.btnSaveAdditionalInfo_Click);
            // 
            // btnBackAdditionalInfo
            // 
            this.btnBackAdditionalInfo.Location = new System.Drawing.Point(171, 12);
            this.btnBackAdditionalInfo.Name = "btnBackAdditionalInfo";
            this.btnBackAdditionalInfo.Size = new System.Drawing.Size(199, 23);
            this.btnBackAdditionalInfo.TabIndex = 32;
            this.btnBackAdditionalInfo.Text = "Назад";
            this.btnBackAdditionalInfo.UseVisualStyleBackColor = true;
            this.btnBackAdditionalInfo.Visible = false;
            this.btnBackAdditionalInfo.Click += new System.EventHandler(this.btnBackAdditionalInfo_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(734, 86);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(144, 23);
            this.btnPrint.TabIndex = 33;
            this.btnPrint.Text = "Печать";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Visible = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lbGeneralIntel
            // 
            this.lbGeneralIntel.AutoSize = true;
            this.lbGeneralIntel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbGeneralIntel.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.lbGeneralIntel.Location = new System.Drawing.Point(169, 72);
            this.lbGeneralIntel.Name = "lbGeneralIntel";
            this.lbGeneralIntel.Size = new System.Drawing.Size(483, 31);
            this.lbGeneralIntel.TabIndex = 35;
            this.lbGeneralIntel.Text = "Общие сведения по преподавателям";
            // 
            // lbAdditionalInfo
            // 
            this.lbAdditionalInfo.AutoSize = true;
            this.lbAdditionalInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbAdditionalInfo.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.lbAdditionalInfo.Location = new System.Drawing.Point(173, 72);
            this.lbAdditionalInfo.Name = "lbAdditionalInfo";
            this.lbAdditionalInfo.Size = new System.Drawing.Size(395, 31);
            this.lbAdditionalInfo.TabIndex = 36;
            this.lbAdditionalInfo.Text = "Дополнительная информация";
            this.lbAdditionalInfo.Visible = false;
            // 
            // lbIntelHours
            // 
            this.lbIntelHours.AutoSize = true;
            this.lbIntelHours.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbIntelHours.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.lbIntelHours.Location = new System.Drawing.Point(175, 39);
            this.lbIntelHours.Name = "lbIntelHours";
            this.lbIntelHours.Size = new System.Drawing.Size(319, 26);
            this.lbIntelHours.TabIndex = 37;
            this.lbIntelHours.Text = "Сведения по учтенным часам";
            this.lbIntelHours.Visible = false;
            // 
            // lbEnterIntel
            // 
            this.lbEnterIntel.AutoSize = true;
            this.lbEnterIntel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbEnterIntel.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.lbEnterIntel.Location = new System.Drawing.Point(163, 39);
            this.lbEnterIntel.Name = "lbEnterIntel";
            this.lbEnterIntel.Size = new System.Drawing.Size(424, 26);
            this.lbEnterIntel.TabIndex = 38;
            this.lbEnterIntel.Text = "Ввод сведений о проведенных занятиях";
            this.lbEnterIntel.Visible = false;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(884, 60);
            this.tbPassword.MaxLength = 20;
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '●';
            this.tbPassword.Size = new System.Drawing.Size(104, 20);
            this.tbPassword.TabIndex = 39;
            this.tbPassword.Visible = false;
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(881, 44);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(99, 13);
            this.lbPassword.TabIndex = 40;
            this.lbPassword.Text = "Требуется пароль";
            this.lbPassword.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(884, 86);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(104, 23);
            this.btnOK.TabIndex = 41;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Visible = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tbLevelEducation
            // 
            this.tbLevelEducation.Location = new System.Drawing.Point(362, 89);
            this.tbLevelEducation.Name = "tbLevelEducation";
            this.tbLevelEducation.Size = new System.Drawing.Size(60, 20);
            this.tbLevelEducation.TabIndex = 42;
            this.tbLevelEducation.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DBviewer.Properties.Resources.aso;
            this.pictureBox1.Location = new System.Drawing.Point(12, 5);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(144, 104);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 34;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 534);
            this.Controls.Add(this.tbLevelEducation);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lbPassword);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.lbEnterIntel);
            this.Controls.Add(this.lbIntelHours);
            this.Controls.Add(this.lbAdditionalInfo);
            this.Controls.Add(this.lbGeneralIntel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnBackAdditionalInfo);
            this.Controls.Add(this.btnSaveAdditionalInfo);
            this.Controls.Add(this.btnAdditionalInfo);
            this.Controls.Add(this.btnViewAll);
            this.Controls.Add(this.btnEnter);
            this.Controls.Add(this.btnBackReports);
            this.Controls.Add(this.lbHours);
            this.Controls.Add(this.tbHours);
            this.Controls.Add(this.lbDay);
            this.Controls.Add(this.cbDay);
            this.Controls.Add(this.lbLevelEducation);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1015, 250);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.Label lbLevelEducation;
        private System.Windows.Forms.ComboBox cbDay;
        private System.Windows.Forms.Label lbDay;
        private System.Windows.Forms.TextBox tbHours;
        private System.Windows.Forms.Label lbHours;
        private System.Windows.Forms.Button btnBackReports;
        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.Button btnViewAll;
        private System.Windows.Forms.Button btnAdditionalInfo;
        private System.Windows.Forms.Button btnSaveAdditionalInfo;
        private System.Windows.Forms.Button btnBackAdditionalInfo;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbGeneralIntel;
        private System.Windows.Forms.Label lbAdditionalInfo;
        private System.Windows.Forms.Label lbIntelHours;
        private System.Windows.Forms.Label lbEnterIntel;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox tbLevelEducation;
    }
}

