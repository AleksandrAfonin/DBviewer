using Spire.Xls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBviewer
{
    public partial class Form1 : Form
    {
        private static string dataBase = "27231685c0687995.db";
        private static string connectionString = "Data Source=" + dataBase + "; Version=3; FailIfMissing=True";
        private string templete = "template.xlsx";
        private string psw = "admin";

        SQLiteConnection connection;
        SQLiteCommand CMD;
        SQLiteDataAdapter adapter;
        DataTable disciplinesTable = new DataTable();
        DataTable teachersTable = new DataTable();
        DataTable generalTable = new DataTable();
        DataTable reportsTable = new DataTable();
        DataTable curConsultHoursTable = new DataTable();

        Size dgvTableSize;

        bool isInput = false;

        public Form1()
        {
            InitializeComponent();
            dgvTableSize = dgvTable.Size;
        }

        // При загрузке формы
        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            FileInfo[] files = directory.GetFiles("*" + ".db");
            if (files.Length != 0)
            {
                dataBase = files[0].FullName;
                connectionString = "Data Source=" + dataBase + "; Version=3; FailIfMissing=True";
            }
            else
            {
                MessageBox.Show("Требуется указать базу данных");
                if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                {
                    Environment.Exit(0);
                }

                string fileName = openFileDialog.FileName;
                dataBase = fileName;
                connectionString = "Data Source=" + dataBase + "; Version=3; FailIfMissing=True";
            }

            LoadDisciplinesTable();
            CreateNewColumnsIfNotExist();
            CreateTableReports();
            LoadTeachersTable();
            CreateTableCurConsultHours();
            GeneralTable();
            FillCBTeacher();
            Activate();
        }

        // При изменении размеров формы
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            dgvTableSize.Width = this.Width - 40;
            dgvTableSize.Height = this.Height - 165;
            dgvTable.Size = dgvTableSize;
        }

        // Загрузка таблицы Disciplines
        private void LoadDisciplinesTable()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT * FROM Disciplines", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    adapter.Fill(disciplinesTable);
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не то !");
                Environment.Exit(0);
            }
        }

        // Загрузка таблицы Teachers
        private void LoadTeachersTable()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT * FROM Teachers", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    adapter.Fill(teachersTable);
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Нет таблицы 'Teachers'");
                Environment.Exit(0);
            }
        }

        // Создание новых столбцов если не существуют
        private void CreateNewColumnsIfNotExist()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT FullFIO FROM Teachers", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                }
            }
            catch (SQLiteException)
            {
                try
                {
                    using (connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        CMD = new SQLiteCommand("ALTER TABLE Teachers ADD COLUMN FullFIO TEXT(100)", connection);
                        CMD.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException)
                {
                    MessageBox.Show("Не удалось добавить столбец FullFIO");
                }
            }

            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT HErate FROM Teachers", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                }
            }
            catch (SQLiteException)
            {
                try
                {
                    using (connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        CMD = new SQLiteCommand("ALTER TABLE Teachers ADD COLUMN HErate INTEGER", connection);
                        CMD.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException)
                {
                    MessageBox.Show("Не удалось добавить столбец HErate");
                }
            }

            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT SPErate FROM Teachers", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                }
            }
            catch (SQLiteException)
            {
                try
                {
                    using (connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        CMD = new SQLiteCommand("ALTER TABLE Teachers ADD COLUMN SPErate INTEGER", connection);
                        CMD.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException)
                {
                    MessageBox.Show("Не удалось добавить столбец SPErate");
                }
            }
        }

        // Проверка на текущую консультацию
        private bool CheckForCurConsult()
        {
            bool isCurConsult = false;
            string disName = cbDiscipline.Text;
            string groupName = cbStudiesGroup.Text;
            string teacherName = cbTeacher.Text;

            foreach(DataRow dataRow in curConsultHoursTable.Rows)
            {
                if (dataRow["DisName"].ToString() == disName && dataRow["GroupName"].ToString() == groupName &&
                     (dataRow["FIOLector"].ToString() == teacherName || dataRow["FIOPractice"].ToString() == teacherName ||
                      dataRow["FIOLab1"].ToString() == teacherName || dataRow["FIOLab2"].ToString() == teacherName))
                    isCurConsult = true;
            }
            return isCurConsult;
        }

        // Создание таблицы текущих консультаций для лектора, практика, лаборанта
        private void CreateTableCurConsultHours()
        {
            curConsultHoursTable.Columns.Add("DisName", typeof(String));
            curConsultHoursTable.Columns.Add("GroupName", typeof(String));
            curConsultHoursTable.Columns.Add("FIOLector", typeof(String));
            curConsultHoursTable.Columns.Add("LectorHours", typeof(Int32));
            curConsultHoursTable.Columns.Add("FIOPractice", typeof(String));
            curConsultHoursTable.Columns.Add("PracticeHours", typeof(Int32));
            curConsultHoursTable.Columns.Add("FIOLab1", typeof(String));
            curConsultHoursTable.Columns.Add("Lab1Hours", typeof(Int32));
            curConsultHoursTable.Columns.Add("FIOLab2", typeof(String));
            curConsultHoursTable.Columns.Add("Lab2Hours", typeof(Int32));

            foreach(DataRow row in disciplinesTable.Rows)
            {
                int curConsultHours = GetIntValue(row["CurConsultHours"]);

                if (curConsultHours == 0) continue;

                int idLector = GetIntValue(row["IdLector"]);
                int idPractice = GetIntValue(row["IdPractice"]);
                int idLab1 = GetIntValue(row["IdLab1"]);
                int idLab2 = GetIntValue(row["IdLab2"]);

                int letureHours = GetIntValue(row["LectureHours"]);
                int practiceHours = GetIntValue(row["PracticeHours"]);
                int labHours = GetIntValue(row["LabHours"]);

                int sumAud = letureHours + practiceHours + labHours;
                int curConLector = curConsultHours * letureHours / sumAud;
                int curConPractice = curConsultHours * practiceHours / sumAud;
                int curConLab1 = curConsultHours * labHours / sumAud;
                int curConLab2 = 0;

                if(idLab1 != 0 && idLab2 != 0)
                {
                    curConLab1 /= 2;
                    curConLab2 = curConLab1;
                }

                if(curConsultHours < curConLector + curConPractice + curConLab1 + curConLab2)
                {
                    if (curConLab2 > 0) curConLab2--;
                    if (curConsultHours < curConLector + curConPractice + curConLab1 + curConLab2)
                    {
                        if (curConLab1 > 0) curConLab1--;
                        if (curConsultHours < curConLector + curConPractice + curConLab1 + curConLab2)
                        {
                            if (curConPractice > 0) curConPractice--;
                            if (curConsultHours < curConLector + curConPractice + curConLab1 + curConLab2)
                            {
                                if (curConLector > 0) curConLector--;
                            }
                        }
                    }
                }
                else if (curConsultHours > curConLector + curConPractice + curConLab1 + curConLab2)
                {
                    if (idLector != 0) curConLector++;
                    if (curConsultHours > curConLector + curConPractice + curConLab1 + curConLab2)
                    {
                        if (idPractice != 0) curConPractice++;
                        if (curConsultHours > curConLector + curConPractice + curConLab1 + curConLab2)
                        {
                            if (idLab1 != 0) curConLab1++;
                            if (curConsultHours > curConLector + curConPractice + curConLab1 + curConLab2)
                            {
                                if (idLab2 != 0) curConLab2++;
                            }
                        }
                    }
                }

                string FIOLector = "", FIOPractice = "", FIOLab1 = "", FIOLab2 = "";
                // Получить лектора, практика, лаборантов
                try
                {
                    using (connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        CMD = new SQLiteCommand("SELECT FIO FROM Teachers WHERE Id = " + idLector, connection);
                        FIOLector = GetStringValue(CMD.ExecuteScalar());

                        CMD = new SQLiteCommand("SELECT FIO FROM Teachers WHERE Id = " + idPractice, connection);
                        FIOPractice = GetStringValue(CMD.ExecuteScalar());

                        CMD = new SQLiteCommand("SELECT FIO FROM Teachers WHERE Id = " + idLab1, connection);
                        FIOLab1 = GetStringValue(CMD.ExecuteScalar());

                        CMD = new SQLiteCommand("SELECT FIO FROM Teachers WHERE Id = " + idLab2, connection);
                        FIOLab2 = GetStringValue(CMD.ExecuteScalar());
                    }
                }
                catch (SQLiteException)
                {
                    MessageBox.Show("Не удалось получить данные");
                }

                DataRow tableRow = curConsultHoursTable.NewRow();

                tableRow["DisName"] = row["DisName"];
                tableRow["GroupName"] = row["GroupName"];
                tableRow["FIOLector"] = FIOLector;
                tableRow["LectorHours"] = curConLector;
                tableRow["FIOPractice"] = FIOPractice;
                tableRow["PracticeHours"] = curConPractice;
                tableRow["FIOLab1"] = FIOLab1;
                tableRow["Lab1Hours"] = curConLab1;
                tableRow["FIOLab2"] = FIOLab2;
                tableRow["Lab2Hours"] = curConLab2;

                curConsultHoursTable.Rows.Add(tableRow);
            }
        }
        
        // Генерация общей таблицы
        private void GeneralTable()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT teach.Id AS '№ п_п', teach.FIO AS Преподаватель" +
                                            " FROM Teachers AS teach ORDER BY teach.Id", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    adapter.Fill(generalTable);
                    generalTable.Columns.Add("Лекции план", typeof(Int32));
                    generalTable.Columns.Add("Лекции выполнено", typeof(Int32));
                    generalTable.Columns.Add("Практика план", typeof(Int32));
                    generalTable.Columns.Add("Практика выполнено", typeof(Int32));
                    generalTable.Columns.Add("Лабораторные план", typeof(Int32));
                    generalTable.Columns.Add("Лабораторные выполнено", typeof(Int32));
                    generalTable.Columns.Add("ТекКонсультации план", typeof(Int32));
                    generalTable.Columns.Add("ТекКонсультации выполнено", typeof(Int32));
                    generalTable.Columns.Add("Предэкзаменационные консультации план", typeof(Int32));
                    generalTable.Columns.Add("Предэкзаменационные консультации выполнено", typeof(Int32));
                    generalTable.Columns.Add("Экзамены план", typeof(Int32));
                    generalTable.Columns.Add("Экзамены выполнено", typeof(Int32));
                    generalTable.Columns.Add("Зачеты план", typeof(Int32));
                    generalTable.Columns.Add("Зачеты выполнено", typeof(Int32));
                    generalTable.Columns.Add("Итоговые аттестации план", typeof(Int32));
                    generalTable.Columns.Add("Итоговые аттестации выполнено", typeof(Int32));
                    generalTable.Columns.Add("Всего план", typeof(Int32));
                    generalTable.Columns.Add("Всего выполнено", typeof(Int32));
                    generalTable.Columns.Add("Объем нагрузки", typeof(Int32));
                }
                FillGeneralTable();
                dgvTable.DataSource = generalTable;
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось создать табличные данные");
            }
        }

        // Получить значение int из Object 
        private int GetIntValue(Object ob)
        {
            if (Convert.IsDBNull(ob)) return 0;
            return Convert.ToInt32(ob);
        }

        // Получить значение string из Object
        private string GetStringValue(Object ob)
        {
            if (Convert.IsDBNull(ob)) return "";
            return Convert.ToString(ob);
        }

        // Заполнение главной таблицы
        private void FillGeneralTable()
        {
            LoadTableReports();

            foreach(DataRow row in generalTable.Rows)
            {
                int sumLectureHours = 0, sumPracticeHours = 0, sumLabHours = 0, 
                    sumExamConsultHours = 0, sumCreditHours = 0, sumExamHours = 0, sumFinAttestHours = 0;
                string name = GetTeacherHours(GetStringValue(row["Преподаватель"]));

                foreach (DataRow disRow in disciplinesTable.Rows)
                {
                    if (GetIntValue(disRow["IdLector"]) == GetIntValue(row["№ п_п"])) sumLectureHours += GetIntValue(disRow["LectureHours"]);
                    if (GetIntValue(disRow["IdPractice"]) == GetIntValue(row["№ п_п"])) sumPracticeHours += GetIntValue(disRow["PracticeHours"]);
                    if (GetIntValue(disRow["IdLab1"]) == GetIntValue(row["№ п_п"]) && GetIntValue(disRow["IdLab2"]) == GetIntValue(row["№ п_п"])) sumLabHours += GetIntValue(disRow["LabHours"]);
                    if ((GetIntValue(disRow["IdLab1"]) == GetIntValue(row["№ п_п"]) && GetIntValue(disRow["IdLab2"]) != GetIntValue(row["№ п_п"])) ||
                        (GetIntValue(disRow["IdLab1"]) != GetIntValue(row["№ п_п"]) && GetIntValue(disRow["IdLab2"]) == GetIntValue(row["№ п_п"]))) sumLabHours += (GetIntValue(disRow["LabHours"]) / 2);
                    if (GetIntValue(disRow["IdExam"]) == GetIntValue(row["№ п_п"])) 
                    { 
                        sumExamConsultHours += GetIntValue(disRow["ExamConsultHours"]); 
                        sumExamHours += GetIntValue(disRow["ExamHours"]);
                        sumCreditHours += GetIntValue(disRow["CreditHours"]);
                    }
                    if (GetIntValue(disRow["IdFinAttest"]) == GetIntValue(row["№ п_п"])) sumFinAttestHours += GetIntValue(disRow["FinAttestHours"]);
                }

                if (sumLectureHours != 0) row["Лекции план"] = sumLectureHours;
                if (sumPracticeHours != 0) row["Практика план"] = sumPracticeHours;
                if (sumLabHours != 0) row["Лабораторные план"] = sumLabHours;
                int sum = GetSumCurConsultHour(GetStringValue(row["Преподаватель"]));
                if (sum != 0) row["ТекКонсультации план"] = sum;
                if (sumExamConsultHours != 0) row["Предэкзаменационные консультации план"] = sumExamConsultHours;
                if (sumExamHours != 0) row["Экзамены план"] = sumExamHours;
                if (sumCreditHours != 0) row["Зачеты план"] = sumCreditHours;
                if (sumFinAttestHours != 0) row["Итоговые аттестации план"] = sumFinAttestHours;

                sumLectureHours = 0; sumPracticeHours = 0; sumLabHours = 0; 
                sumExamConsultHours = 0; sumCreditHours = 0; sumExamHours = 0; sumFinAttestHours = 0;
                int sumCurConsultHours = 0;

                foreach (DataRow repRow in reportsTable.Rows)
                {
                    if(GetStringValue(repRow["ИА ГАК ГЭК"]).Contains(name))
                    {
                        sumLectureHours += GetIntValue(repRow["Лекции"]);
                        sumPracticeHours += GetIntValue(repRow["Практика"]);
                        sumLabHours += GetIntValue(repRow["Лабораторные"]);
                        sumCurConsultHours += GetIntValue(repRow["Текущие консультации"]);
                        sumExamConsultHours += GetIntValue(repRow["Предэкзаменационные консультации"]);
                        sumExamHours += GetIntValue(repRow["Экзамены"]);
                        sumCreditHours += GetIntValue(repRow["Зачеты"]);
                        sumFinAttestHours += GetIntValue(repRow["Итоговая аттестация"]);
                    }
                }

                if (sumLectureHours != 0) row["Лекции выполнено"] = sumLectureHours;
                if (sumPracticeHours != 0) row["Практика выполнено"] = sumPracticeHours;
                if (sumLabHours != 0) row["Лабораторные выполнено"] = sumLabHours;
                if (sumCurConsultHours != 0) row["ТекКонсультации выполнено"] = sumCurConsultHours;
                if (sumExamConsultHours != 0) row["Предэкзаменационные консультации выполнено"] = sumExamConsultHours;
                if (sumExamHours != 0) row["Экзамены выполнено"] = sumExamHours;
                if (sumCreditHours != 0) row["Зачеты выполнено"] = sumCreditHours;
                if (sumFinAttestHours != 0) row["Итоговые аттестации выполнено"] = sumFinAttestHours;


                sum = GetIntValue(row["Лекции план"]) + GetIntValue(row["Практика план"]) +
                                    GetIntValue(row["Лабораторные план"]) + GetIntValue(row["ТекКонсультации план"]) +
                                    GetIntValue(row["Предэкзаменационные консультации план"]) + GetIntValue(row["Экзамены план"]) +
                                    GetIntValue(row["Зачеты план"]) + GetIntValue(row["Итоговые аттестации план"]);
                if (sum != 0) row["Всего план"] = sum;

                sum = GetIntValue(row["Лекции выполнено"]) + GetIntValue(row["Практика выполнено"]) +
                                    GetIntValue(row["Лабораторные выполнено"]) + GetIntValue(row["ТекКонсультации выполнено"]) +
                                    GetIntValue(row["Предэкзаменационные консультации выполнено"]) + GetIntValue(row["Экзамены выполнено"]) +
                                    GetIntValue(row["Зачеты выполнено"]) + GetIntValue(row["Итоговые аттестации выполнено"]);
                if (sum != 0) row["Всего выполнено"] = sum;
            }

        }

        // Получить сумму часов текущих консультаций для преподавателя
        private int GetSumCurConsultHour(string fio)
        {
            int sum = 0;
            foreach (DataRow dataRow in curConsultHoursTable.Rows)
            {
                if (fio == GetStringValue(dataRow["FIOLector"])) sum += GetIntValue(dataRow["LectorHours"]);
                if (fio == GetStringValue(dataRow["FIOPractice"])) sum += GetIntValue(dataRow["PracticeHours"]);
                if (fio == GetStringValue(dataRow["FIOLab1"])) sum += GetIntValue(dataRow["Lab1Hours"]);
                if (fio == GetStringValue(dataRow["FIOLab2"])) sum += GetIntValue(dataRow["Lab2Hours"]);
            }
            return sum;
        }

        // Получить преподавателя почасовика
        private string GetTeacherHours(string name)
        {
            if (name.Contains("Почасов")) return name;
            else return "None";
        }

        // создание таблицы Reports если она не существует
        private void CreateTableReports()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Reports (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                            " DisName TEXT(100), GroupName TEXT(20), LectureHours INTEGER, PracticeHours INTEGER," +
                            " LabHours INTEGER, CurConsultHours INTEGER, ExamConsultHours INTEGER," +
                            " ExamHours INTEGER, CreditHours INTEGER, FinAttestHours INTEGER," +
                            " TotalHours INTEGER, Date TEXT(10), IaGakGek TEXT(100), PaymentMark TEXT(20))", connection);
                    CMD.ExecuteNonQuery();
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось создать таблицу отчетов");
            }
        }

        // Загрузка таблицы Reports
        private void LoadTableReports()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT Id, DisName AS Дисциплина, GroupName AS Группа, LectureHours AS Лекции, PracticeHours AS Практика," +
                        " LabHours AS Лабораторные, CurConsultHours AS 'Текущие консультации', ExamConsultHours AS 'Предэкзаменационные консультации'," +
                        " ExamHours AS Экзамены, CreditHours AS Зачеты, FinAttestHours AS 'Итоговая аттестация'," +
                        " TotalHours AS Всего, Date AS Дата, IaGakGek AS 'ИА ГАК ГЭК', PaymentMark AS Оплата FROM Reports", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    reportsTable.Clear();
                    adapter.Fill(reportsTable);
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось загрузить данные отчетов");
            }
        }

        // Загрузка таблицы Reports с учетом месяца, года, формы обучения, преподавателя
        private void LoadTableReportsWithArguments()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT Id, DisName AS Дисциплина, GroupName AS Группа, LectureHours AS Лекции, PracticeHours AS Практика," +
                        " LabHours AS Лабораторные, CurConsultHours AS 'Текущие консультации', ExamConsultHours AS 'Предэкзаменационные консультации'," +
                        " ExamHours AS Экзамены, CreditHours AS Зачеты, FinAttestHours AS 'Итоговая аттестация'," +
                        " TotalHours AS Всего, Date AS Дата, IaGakGek AS 'ИА ГАК ГЭК', PaymentMark AS Оплата FROM Reports WHERE" +
                        " IaGakGek LIKE '" + GetStringWith0(cbMonth.SelectedIndex + 1) + "%' AND IaGakGek LIKE '%" + cbYear.Text + "%' AND IaGakGek LIKE '%" +
                        cbTeacher.Text + "%' AND IaGakGek LIKE '%" + cbStudiesForm.Text + "%' AND PaymentMark IS NULL", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    reportsTable.Clear();
                    adapter.Fill(reportsTable);
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось загрузить данные отчетов");
            }
        }

        // Заполнение элементов Combo Box Teacher
        private void FillCBTeacher()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT DISTINCT FIO" +
                                            " FROM Teachers WHERE FIO LIKE 'Почасов%' ORDER BY Id", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    cbTeacher.DataSource = table;
                    cbTeacher.DisplayMember = "FIO";
                    cbTeacher.SelectedIndex = -1;
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить ФИО преподавателей");
            }
        }

        // Получение количества дней в месяце
        private void DaysInMonth()
        {
            if (cbMonth.SelectedIndex == -1 || cbYear.SelectedIndex == -1) return;
            int days = DateTime.DaysInMonth(Convert.ToInt32(cbYear.Text), Convert.ToInt32(cbMonth.SelectedIndex + 1));
            cbDay.SelectedIndex = -1;
            cbDay.Items.Clear();
            for (int i = 1; i <= days; i++)
            {
                cbDay.Items.Add(i.ToString());
            }
        }

        // Получить количество часов по плану для лекций *
        private int GetLectureHours()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT LectureHours" +
                                                    " FROM Disciplines WHERE IdLector = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "') AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить часы лекций");
                return 0;
            }
        }

        // Получить количесво часов по плану для практики *
        private int GetPracticeHours()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT PracticeHours" +
                                                    " FROM Disciplines WHERE IdPractice = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "') AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить часы практик");
                return 0;
            }
        }

        // Получить количесво часов по плану для лабораторных *
        private int GetLabHours()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            int hoursLab1, hoursLab2;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT LabHours" +
                                                    " FROM Disciplines WHERE IdLab1 = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "') AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    hoursLab1 = GetIntValue(CMD.ExecuteScalar());

                    CMD = new SQLiteCommand("SELECT LabHours" +
                                                    " FROM Disciplines WHERE IdLab2 = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "') AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    hoursLab2 = GetIntValue(CMD.ExecuteScalar());
                }

                if (hoursLab1 == hoursLab2) return hoursLab1;
                else return Math.Max(hoursLab1, hoursLab2) / 2;
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить часы лабораторных");
                return 0;
            }
        }

        // Получить количесво часов по плану для аттестации *
        private int GetFinAttestHours()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT FinAttestHours" +
                                                    " FROM Disciplines WHERE IdFinAttest = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "') AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить часы финальных аттестаций");
                return 0;
            }
        }

        // Получить количество часов по плану для текущей консультации *
        private int GetCurConsultHours()
        {
            int sumCurConsult = 0;
            string disName = cbDiscipline.Text;
            string groupName = cbStudiesGroup.Text;
            string teacherName = cbTeacher.Text;

            foreach (DataRow dataRow in curConsultHoursTable.Rows)
            {
                if (dataRow["DisName"].ToString() == disName && dataRow["GroupName"].ToString() == groupName)
                {
                    if (dataRow["FIOLector"].ToString() == teacherName) sumCurConsult += GetIntValue(dataRow["LectorHours"]);
                    if (dataRow["FIOPractice"].ToString() == teacherName) sumCurConsult += GetIntValue(dataRow["PracticeHours"]);
                    if (dataRow["FIOLab1"].ToString() == teacherName) sumCurConsult += GetIntValue(dataRow["Lab1Hours"]);
                    if (dataRow["FIOLab2"].ToString() == teacherName) sumCurConsult += GetIntValue(dataRow["Lab2Hours"]);
                }
            }
            return sumCurConsult;
        }

        // Получить количество часов по плану для зачетов *
        private int GetCreditHours()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT CreditHours" +
                                                    " FROM Disciplines WHERE IdExam = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "') AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить часы зачетов");
                return 0;
            }
        }

        // Получить количество часов по плану для предэкзаменационных консультаций *
        private int GetExamConsultHours()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT ExamConsultHours" +
                                                    " FROM Disciplines WHERE IdExam = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "') AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить часы предэкзаменационных консультаций");
                return 0;
            }
        }

        // Получить количесво часов по плану для экзамена *
        private int GetExamHours()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT ExamHours" +
                                                    " FROM Disciplines WHERE IdExam = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "') AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить экзаменационных часов");
                return 0;
            }
        }

        // Получить количество учтенных часов для лекций
        private int GetLectureHoursComplete()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT SUM(LectureHours)" +
                                                    " FROM Reports WHERE IaGakGek LIKE '%" + cbTeacher.Text + "%' AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить выполненные часы лекций");
                return 0;
            }
        }

        // Получить количество учтенных часов для практики
        private int GetPracticeHoursComplete()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT SUM(PracticeHours)" +
                                                    " FROM Reports WHERE IaGakGek LIKE '%" + cbTeacher.Text + "%' AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить выполненные часы практик");
                return 0;
            }
        }

        // Получить количество учтенных часов для лабораторных
        private int GetLabHoursComplete()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT SUM(LabHours)" +
                                                    " FROM Reports WHERE IaGakGek LIKE '%" + cbTeacher.Text + "%' AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить выполненные часы лабораторных");
                return 0;
            }
        }

        // Получить количество учтенных часов для текущих консультаций
        private int GetCurConsultHoursComplete()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT SUM(CurConsultHours)" +
                                                    " FROM Reports WHERE IaGakGek LIKE '%" + cbTeacher.Text + "%' AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить выполненные часы текущих консультаций");
                return 0;
            }
        }

        // Получить количество учтенных часов для предэкзаменационных консультаций
        private int GetExamConsultHoursComplete()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT SUM(ExamConsultHours)" +
                                                    " FROM Reports WHERE IaGakGek LIKE '%" + cbTeacher.Text + "%' AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить выполненные часы предэкзаменационных консультаций");
                return 0;
            }
        }

        // Получить количество учтенных часов для зачетов
        private int GetCreditHoursComplete()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT SUM(CreditHours)" +
                                                    " FROM Reports WHERE IaGakGek LIKE '%" + cbTeacher.Text + "%' AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить выполненные часы зачетов");
                return 0;
            }
        }

        // Получить количество учтенных часов для итоговой аттестации
        private int GetFinAttestHoursComplete()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT SUM(FinAttestHours)" +
                                                    " FROM Reports WHERE IaGakGek LIKE '%" + cbTeacher.Text + "%' AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить выполненные часы финальных аттестаций");
                return 0;
            }
        }

        // Получить количество учтенных часов для экзамена
        private int GetExamHoursComplete()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT SUM(ExamHours)" +
                                                    " FROM Reports WHERE IaGakGek LIKE '%" + cbTeacher.Text + "%' AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить выполненные часы экзаменов");
                return 0;
            }
        }

        // Получить количество часов по плану
        private int GetHoursPlan()
        {
            switch (cbTypeOfActivity.Text)
            {
                case "Лекции": return GetLectureHours();
                case "Практики": return GetPracticeHours();
                case "Лабораторные": return GetLabHours();
                case "Тек. консульт.": return GetCurConsultHours();
                case "Пред. экзам. консульт.": return GetExamConsultHours();
                case "Зачеты": return GetCreditHours();
                case "Экзамены": return GetExamHours();
                case "Итог. аттестации": return GetFinAttestHours();
                default: return 0;
            }
        }

        // Получить количество проведенных часов
        private int GetCompleteHours()
        {
            switch (cbTypeOfActivity.Text)
            {
                case "Лекции": return GetLectureHoursComplete();
                case "Практики": return GetPracticeHoursComplete();
                case "Лабораторные": return GetLabHoursComplete();
                case "Тек. консульт.": return GetCurConsultHoursComplete();
                case "Пред. экзам. консульт.": return GetExamConsultHoursComplete();
                case "Зачеты": return GetCreditHoursComplete();
                case "Экзамены": return GetExamHoursComplete();
                case "Итог. аттестации": return GetFinAttestHoursComplete();
                default: return 0;
            }
        }

        // Получить ставку
        private int GetRate()
        {
            string level;
            if (cbStudiesForm.Text == "ВО") level = "HErate";
            else level = "SPErate";
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT " + level + 
                                            " FROM Teachers WHERE FIO = '" + cbTeacher.Text + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить ставку");
                return 0;
            }
        }

        // Получить семестр (полугодие)
        private int GetSemestr()
        {
            return cbMonth.SelectedIndex < 8 && cbMonth.SelectedIndex > 0 ? 2 : 1;
        }

        // Получить сумму оплаты прописью
        private string GetStringPayment(int value)
        {
            return value.ToString() + " (" + RusNumber.Str(value) + " рублей 00 коп.)";
        }

        // Получить имя поля для часов
        private string GetFieldName()
        {
            switch (cbTypeOfActivity.Text)
            {
                case "Лекции": return "LectureHours";
                case "Практики": return "PracticeHours";
                case "Лабораторные": return "LabHours";
                case "Тек. консульт.": return "CurConsultHours";
                case "Пред. экзам. консульт.": return "ExamConsultHours";
                case "Зачеты": return "CreditHours";
                case "Экзамены": return "ExamHours";
                case "Итог. аттестации": return "FinAttestHours";
                default: return "";
            }
        }

        // Получение строкового значения с ведущим 0 *
        private string GetStringWith0(int value)
        {
            string str = value.ToString();
            if (str.Length < 2) str = "0" + str;
            return str;
        }

        // Получить полное ФИО преподавателя
        private string GetFullName()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT FullFIO FROM Teachers WHERE FIO = '" + cbTeacher.Text + "'", connection);
                    return GetStringValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить полное имя преподавателя");
                return "";
            }
        }

        // Получить должность преподавателя
        private string GetTeacherPosition()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT Position " +
                                            " FROM Teachers WHERE FIO = '" + GetShortTeacherName() + "'", connection);
                    return GetStringValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить должность преподавателя");
                return "";
            }
        }

        // Получить короткое имя преподавателя
        private string GetShortTeacherName()
        {
            string sTeacher = cbTeacher.Text;
            return sTeacher.Substring(sTeacher.IndexOf("-") + 1).Trim();
        }

        // Заполнение элементов Combo Box Discipline *
        private void SetDiscCBDiscipline()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT DISTINCT DisName" +
                                            " FROM Disciplines WHERE IdLector = (SELECT Id FROM Teachers WHERE FIO = '" +
                                        cbTeacher.Text + "') OR IdPractice = (SELECT Id FROM Teachers WHERE FIO = '" +
                                        cbTeacher.Text + "') OR IdLab1 = (SELECT Id FROM Teachers WHERE FIO = '" +
                                        cbTeacher.Text + "') OR IdLab2 = (SELECT Id FROM Teachers WHERE FIO = '" +
                                        cbTeacher.Text + "') OR IdFinAttest = (SELECT Id FROM Teachers WHERE FIO = '" +
                                        cbTeacher.Text + "') OR IdExam = (SELECT Id FROM Teachers WHERE FIO = '" +
                                        cbTeacher.Text + "')", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    cbDiscipline.DataSource = null;
                    cbDiscipline.DataSource = table;
                    cbDiscipline.DisplayMember = "DisName";
                    cbDiscipline.SelectedIndex = -1;
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить дисциплины");
            }
        }

        // Заполнение элементов Combo Box StudiesGroup *
        private void SetGroupCBStudiesGroup()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT DISTINCT GroupName" +
                                                    " FROM Disciplines WHERE (IdLector = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "') OR IdPractice = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "') OR IdLab1 = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "') OR IdLab2 = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "') OR IdFinAttest = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "') OR IdExam = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "')) AND DisName = '" + cbDiscipline.Text + "'", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    cbStudiesGroup.DataSource = null;
                    cbStudiesGroup.DataSource = table;
                    cbStudiesGroup.DisplayMember = "GroupName";
                    cbStudiesGroup.SelectedIndex = -1;
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить группы");
            }
        }

        // Заполнение элементов ComboBox TypeOfActivity *
        private void SetActivityCBTypeOfActivity()
        {
            bool lab1 = false;
            bool lab2 = false;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    cbTypeOfActivity.Items.Clear();
                    connection.Open();
                    // Проверка на лекции
                    CMD = new SQLiteCommand("SELECT DisName, GroupName" +
                                            " FROM Disciplines WHERE DisName = '" + cbDiscipline.Text + "' AND GroupName = '" +
                                            cbStudiesGroup.Text + "' AND IdLector = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "')", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count != 0) cbTypeOfActivity.Items.Add("Лекции");

                    // Проверка на практику
                    CMD = new SQLiteCommand("SELECT DisName, GroupName" +
                                            " FROM Disciplines WHERE DisName = '" + cbDiscipline.Text + "' AND GroupName = '" +
                                            cbStudiesGroup.Text + "' AND IdPractice = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "')", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count != 0) cbTypeOfActivity.Items.Add("Практики");

                    // Проверка на лабораторные
                    CMD = new SQLiteCommand("SELECT DisName, GroupName" +
                                            " FROM Disciplines WHERE DisName = '" + cbDiscipline.Text + "' AND GroupName = '" +
                                            cbStudiesGroup.Text + "' AND IdLab1 = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "')", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count != 0) lab1 = true;

                    CMD = new SQLiteCommand("SELECT DisName, GroupName" +
                                            " FROM Disciplines WHERE DisName = '" + cbDiscipline.Text + "' AND GroupName = '" +
                                            cbStudiesGroup.Text + "' AND IdLab2 = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "')", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count != 0) lab2 = true;
                    
                    if (lab1 || lab2) cbTypeOfActivity.Items.Add("Лабораторные");

                    // Проверка на текущую консультацию
                    if (CheckForCurConsult()) cbTypeOfActivity.Items.Add("Тек. консульт.");

                    // Проверка на предэкзамениционную консультацию, зачет, экзамен
                    CMD = new SQLiteCommand("SELECT DisName, GroupName" +
                                            " FROM Disciplines WHERE DisName = '" + cbDiscipline.Text + "' AND GroupName = '" +
                                            cbStudiesGroup.Text + "' AND IdExam = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "')", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count != 0)
                    {
                        cbTypeOfActivity.Items.Add("Пред. экзам. консульт.");
                        cbTypeOfActivity.Items.Add("Зачеты");
                        cbTypeOfActivity.Items.Add("Экзамены");
                    }

                    // Проверка на итоговую аттестацию
                    CMD = new SQLiteCommand("SELECT DisName, GroupName" +
                                            " FROM Disciplines WHERE DisName = '" + cbDiscipline.Text + "' AND GroupName = '" +
                                            cbStudiesGroup.Text + "' AND IdFinAttest = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "')", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count != 0) cbTypeOfActivity.Items.Add("Итог. аттестации");

                    cbTypeOfActivity.SelectedIndex = -1;
                    cbTypeOfActivity.Text = "";
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить виды занятий");
            }
        }

        // Заполнение элементов ComboBox LevelEducation *
        private void SetLevelCBLevelEducation()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT LevelEducation" +
                                            " FROM Disciplines WHERE DisName = '" + cbDiscipline.Text + "' AND GroupName = '" +
                                            cbStudiesGroup.Text + "' AND (IdLector = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "') OR IdPractice = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "') OR IdLab1 = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "') OR IdLab2 = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "') OR IdFinAttest = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "') OR IdExam = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "'))", connection);
                    tbLevelEducation.Text = GetStringValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить уровень образования");
            }
        }

        // Установка визуализации отдельных элементов
        private void SetVisiblePay()
        {
            btnPaid.Visible = true; lbPassword.Visible = false; tbPassword.Visible = false;
            btnOK.Visible = false;
        }

        // Подготовка отчета
        private bool PrepareOfReport()
        {
            int[] hours = new int[14];
            int totalSum = 0;
            string sForm = cbStudiesForm.Text;
            string teacherPositin = GetTeacherPosition();
            string str4 = teacherPositin + ", " + GetFullName() + " (" + sForm + ")";
            int rate = GetRate();

            List<string> date = new List<string>();
            List<string> typeOfActivity = new List<string>();
            List<string> groupName = new List<string>();
            List<int> quantity = new List<int>();

            int value;
            foreach (DataRow dataRow in reportsTable.Rows)
            {
                date.Add(dataRow["Дата"].ToString());
                groupName.Add(dataRow["Группа"].ToString());
                for(int i = 3; i <= 10; i++)
                {
                    value = GetIntValue(dataRow[i]);
                    if(value != 0)
                    {
                        switch (i)
                        {
                            case 3: typeOfActivity.Add("Лекции"); quantity.Add(value); hours[0] += value;
                                break;
                            case 4: typeOfActivity.Add("Практика"); quantity.Add(value); hours[2] += value;
                                break;
                            case 5: typeOfActivity.Add("Лаб."); quantity.Add(value); hours[1] += value;
                                break;
                            case 6: typeOfActivity.Add("Тек. конс."); quantity.Add(value); hours[3] += value;
                                break;
                            case 7: typeOfActivity.Add("Предэкзам. конс."); quantity.Add(value); hours[4] += value;
                                break;
                            case 8: typeOfActivity.Add("Экзамены"); quantity.Add(value); hours[5] += value;
                                break;
                            case 9: typeOfActivity.Add("Зачеты"); quantity.Add(value); hours[6] += value;
                                break;
                            case 10:
                                string disName = dataRow["Дисциплина"].ToString();
                                if (disName.Contains("ИА") && disName.Contains("сн") && disName.Contains("ру")) { typeOfActivity.Add(disName); quantity.Add(value); hours[7] += value; }
                                if (disName.Contains("ИА") && disName.Contains("еце")) { typeOfActivity.Add(disName); quantity.Add(value); hours[8] += value; }
                                if ((disName.Contains("ИА") && disName.Contains("аб")) || disName.Contains("КЭ") ||
                                    disName.Contains("ачет")) { typeOfActivity.Add(disName); quantity.Add(value); hours[9] += value; }
                                if (disName.Contains("ИА") && disName.Contains("орм")) { typeOfActivity.Add(disName); quantity.Add(value); hours[10] += value; }
                                if (disName.Contains("ИА") && disName.Contains("онс")) { typeOfActivity.Add(disName); quantity.Add(value); hours[11] += value; }
                                if (disName.Contains("ИА") && disName.Contains("ук") && disName.Contains("аф")) { typeOfActivity.Add(disName); quantity.Add(value); hours[12] += value; }
                                if (disName.Contains("ИА") && disName.Contains("ук") && disName.Contains("сп")) { typeOfActivity.Add(disName); quantity.Add(value); hours[13] += value; }
                                break;
                        }
                    }
                }
            }

            try
            {
                using (Workbook workbook = new Workbook())
                {
                    workbook.LoadFromFile(templete);
                    Worksheet worksheet1 = workbook.Worksheets[0];

                    worksheet1.Range[4, 1].Value = str4;

                    string str = GetStringValue(worksheet1.Range[7, 1].Text);

                    worksheet1.Range[7, 1].Text = "в " + GetSemestr() + str.Substring(3);
                    worksheet1.Range[8, 1].Text = "в период с «01» " + cbMonth.Text + " " + cbYear.Text +
                        "г. по «" + cbDay.Items.Count + "» " + cbMonth.Text + " " + cbYear.Text + "г.";

                    int count = 0;
                    for (int col = 1; col < 10; col += 8)
                {
                    for (int row = 11; row <= 47; row++)
                    {
                        if (count == date.Count)
                        {
                            worksheet1.Range[row, col].Value = "";
                            worksheet1.Range[row, col + 2].Value = "";
                            worksheet1.Range[row, col + 5].Value = "";
                            worksheet1.Range[row, col + 7].Value = "";
                        }
                        else
                        {
                            worksheet1.Range[row, col].Value = date[count];
                            worksheet1.Range[row, col + 2].Value = typeOfActivity[count];
                            worksheet1.Range[row, col + 5].Value = groupName[count];
                            worksheet1.Range[row, col + 7].Value2 = quantity[count];
                            count++;
                        }
                    }
                }

                    worksheet1.Range[49, 6].Value2 = quantity.Sum();
                    
                    worksheet1.Range[51, 4].Text = teacherPositin;
                    worksheet1.Range[51, 13].Text = GetShortTeacherName();
                        
                    worksheet1.Range[53, 2].Text = DateTime.Today.ToShortDateString();

                    for (int col = 3; col <= 16; col++)
                    {
                        worksheet1.Range[63, col].Value2 = hours[col - 3];
                        worksheet1.Range[64, col].Value2 = rate;
                        int sum = hours[col - 3] * rate;
                        worksheet1.Range[65, col].Value2 = sum;
                        totalSum += sum;
                    }

                    worksheet1.Range[67, 3].Value = GetStringPayment(totalSum);

                    workbook.Save();
                    workbook.SaveToFile((GetShortTeacherName() + "_" + cbMonth.Text + "_" + cbYear.Text).Replace(".", "_").Replace(" ", "_") + ".xlsx");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Закройте файл шаблона отчета и повторите попытку");
                return false;
            }

            return true;
        }

        // Пометить оплату
        private void MarkPay()
        {
            if (reportsTable.Rows.Count == 0) return;

            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    foreach (DataRow row in reportsTable.Rows)
                    {
                        CMD = new SQLiteCommand("UPDATE Reports SET PaymentMark = 'Оплачено' WHERE Reports.Id = " + row["Id"], connection);
                        CMD.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Оплата успешно отмечена.");
                LoadTableReportsWithArguments();
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось отметить оплату.");
                return;
            }
        }

        // Переход на форму отчетов
        private void btnIntel_Click(object sender, EventArgs e)
        {
            lbGeneralIntel.Visible = false; lbIntelHours.Visible = true;
            lbMonth.Visible = true; cbMonth.Visible = true;
            lbYear.Visible = true; cbYear.Visible = true;
            lbStadiesForm.Visible = true; cbStudiesForm.Visible = true;
            lbTeacher.Visible = true; cbTeacher.Visible = true;
            btnIntel.Visible = false; btnBackGeneral.Visible = true;
            btnInput.Visible = true; btnPaid.Visible = true;
            btnViewAll.Visible = true; btnAdditionalInfo.Visible = false;
            btnPrint.Visible = true; 

            cbMonth.SelectedIndex = DateTime.Today.Month - 1;
            cbYear.SelectedItem = DateTime.Today.Year.ToString();
            cbStudiesForm.SelectedIndex = 0;

            LoadTableReports();
            dgvTable.DataSource = reportsTable;
        }

        // Возврат на общую форму из формы отчетов
        private void btnBackGeneral_Click(object sender, EventArgs e)
        {
            SetVisiblePay();
            lbGeneralIntel.Visible = true; lbIntelHours.Visible = false;
            lbMonth.Visible = false; cbMonth.Visible = false;
            lbYear.Visible = false; cbYear.Visible = false;
            lbStadiesForm.Visible = false; cbStudiesForm.Visible = false;
            lbTeacher.Visible = false; cbTeacher.Visible = false;
            btnIntel.Visible = true; btnBackGeneral.Visible = false;
            btnInput.Visible = false; btnPaid.Visible = false;
            btnViewAll.Visible = false; btnAdditionalInfo.Visible = true;
            btnPrint.Visible = false;

            cbTeacher.SelectedIndex = -1;

            dgvTable.DataSource = generalTable;
        }

        // Показать весь Report
        private void btnViewAll_Click(object sender, EventArgs e)
        {
            SetVisiblePay();

            cbTeacher.SelectedIndex = -1;
            LoadTableReports();
        }

        // Пометка оплаты 
        private void btnPaid_Click(object sender, EventArgs e)
        {
            if (reportsTable.Rows.Count == 0)
            {
                MessageBox.Show("Нечего оплачивать.");
                return;
            }

            if (cbTeacher.SelectedIndex == -1)
            {
                MessageBox.Show("Преподаватель не выбран.");
                return;
            }

            tbPassword.Text = "";
            btnPaid.Visible = false; lbPassword.Visible = true; tbPassword.Visible = true;
            btnOK.Visible = true;
            tbPassword.Focus();
        }

        // Переход на форму ввода проведенных часов
        private void btnInput_Click(object sender, EventArgs e)
        {
            SetVisiblePay();
            btnBackReports.Visible = true; btnBackGeneral.Visible = false; btnInput.Visible = false;
            cbStudiesForm.Visible = false; lbStadiesForm.Visible = false; btnPaid.Visible = false;
            btnViewAll.Visible = false; btnEnter.Visible = true;
            cbDay.Visible = true; lbDay.Visible = true;
            cbDiscipline.Visible = true; lbDiscipline.Visible = true;
            cbStudiesGroup.Visible = true; lbStudiesGroup.Visible = true;
            cbTypeOfActivity.Visible = true; lbTypeOfActivity.Visible = true;
            tbHours.Visible = true; lbHours.Visible = true;
            tbLevelEducation.Visible = true; lbLevelEducation.Visible = true;
            lbIntelHours.Visible = false; lbEnterIntel.Visible = true;
            btnPrint.Visible = false;

            isInput = true;
            tbHours.Text = "";
            cbTeacher.SelectedIndex = -1;
            DaysInMonth();
        }

        // Возврат из формы ввода проведенных часов
        private void btnBackReports_Click(object sender, EventArgs e)
        {
            btnBackReports.Visible = false; btnBackGeneral.Visible = true; btnInput.Visible = true;
            cbStudiesForm.Visible = true; lbStadiesForm.Visible = true; btnPaid.Visible = true;
            btnViewAll.Visible = true; btnEnter.Visible = false;
            cbDay.Visible = false; lbDay.Visible = false;
            cbDiscipline.Visible = false; lbDiscipline.Visible = false;
            cbStudiesGroup.Visible = false; lbStudiesGroup.Visible = false;
            cbTypeOfActivity.Visible = false; lbTypeOfActivity.Visible = false;
            tbHours.Visible = false; lbHours.Visible = false;
            tbLevelEducation.Visible = false; lbLevelEducation.Visible = false;
            lbIntelHours.Visible = true; lbEnterIntel.Visible = false;
            btnPrint.Visible = true;

            cbTeacher.SelectedIndex = -1;

            isInput = false;
        }

        // Записать внесенные данные отработанных часов
        private void btnEnter_Click(object sender, EventArgs e)
        {
            int hoursAvailable = GetHoursPlan() - GetCompleteHours();
            int hours;
            try
            {
                hours = Int32.Parse(tbHours.Text);
                if (hours > hoursAvailable)
                {
                    MessageBox.Show("Количество часов превышает доступное !");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не корретный ввод отработанных часов !");
                return;
            }

            if (cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1 || cbTypeOfActivity.SelectedIndex == -1
                || cbDay.SelectedIndex == -1 || cbTeacher.SelectedIndex == -1)
            {
                MessageBox.Show("Не все данные заполнены !");
                return;
            }
            else
            {
                try
                {
                    using (connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        CMD = new SQLiteCommand("INSERT INTO Reports (DisName, GroupName, " + GetFieldName() +
                                    ", TotalHours, Date, IaGakGek) VALUES ('" + cbDiscipline.Text + "', '" +
                                    cbStudiesGroup.Text + "', " + hours + ", " + hours +
                                    ", '" + GetStringWith0(cbDay.SelectedIndex + 1) + "." + GetStringWith0(cbMonth.SelectedIndex + 1) + "." + cbYear.Text +
                                    "', '" + GetStringWith0(cbMonth.SelectedIndex + 1) + cbYear.Text + cbTeacher.Text + tbLevelEducation.Text +
                                    "')", connection);
                        CMD.ExecuteNonQuery();
                        LoadTableReports();
                        dgvTable.DataSource = reportsTable;
                    }
                    MessageBox.Show("Данные успешно внесены.");
                    
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("Вылет в INSERTe: " + ex);
                }

                cbTypeOfActivity.SelectedIndex = -1;
                lbHours.Text = "Часы";
            }
        }

        // Ввод дополнительной информации о преподавателях
        private void btnAdditionalInfo_Click(object sender, EventArgs e)
        {
            lbGeneralIntel.Visible = false; lbAdditionalInfo.Visible = true;
            btnSaveAdditionalInfo.Visible = true; btnBackAdditionalInfo.Visible = true;
            btnAdditionalInfo.Visible = false; btnIntel.Visible = false;

            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT Id, FIO AS Преподаватель, FullFIO AS 'Полное ФИО', HErate AS 'Ставка ВО', SPErate AS 'Ставка СПО'" +
                        " FROM Teachers WHERE FIO LIKE '%очасов%'", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dgvTable.DataSource = table;
                }

                dgvTable.ReadOnly = false;
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить ФИО преподавателей");
            }
        }

        // Возврат из ввода дополнительной информации без сохранения
        private void btnBackAdditionalInfo_Click(object sender, EventArgs e)
        {
            btnSaveAdditionalInfo.Visible = false; btnBackAdditionalInfo.Visible = false;
            btnAdditionalInfo.Visible = true; btnIntel.Visible = true;
            lbAdditionalInfo.Visible = false; lbGeneralIntel.Visible = true;

            dgvTable.DataSource = generalTable; dgvTable.ReadOnly = true;
        }

        // Сохранение дополнительной информации и возврат
        private void btnSaveAdditionalInfo_Click(object sender, EventArgs e)
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    foreach (DataGridViewRow row in dgvTable.Rows)
                    {
                        CMD = new SQLiteCommand("UPDATE Teachers SET FullFIO = '" + row.Cells["Полное ФИО"].Value +
                                "', HErate = " + GetIntValue(row.Cells["Ставка ВО"].Value) +
                                ", SPErate = " + GetIntValue(row.Cells["Ставка СПО"].Value) +
                                " WHERE Id = " + row.Cells["Id"].Value, connection);
                        CMD.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Данные успешно сохранены.");
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось внести данные.");
            }

            btnSaveAdditionalInfo.Visible = false; btnBackAdditionalInfo.Visible = false;
            btnAdditionalInfo.Visible = true; btnIntel.Visible = true;
            lbAdditionalInfo.Visible = false; lbGeneralIntel.Visible = true;

            dgvTable.DataSource = generalTable; dgvTable.ReadOnly = true;
        }

        // Обработка изменения месяца
        private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            DaysInMonth();
            if (!isInput)
            {
                LoadTableReportsWithArguments();
            }
        }

        // Обработка изменения года
        private void cbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            DaysInMonth();
            if (!isInput)
            {
                LoadTableReportsWithArguments();
            }
        }

        // Обработка изменения преподавателя
        private void cbTeacher_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInput)
            {
                cbDay.SelectedIndex = -1;
                SetDiscCBDiscipline();
            }
            else
            {
                LoadTableReportsWithArguments();
            }

        }

        // Обработка изменения дисциплины
        private void cbDiscipline_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGroupCBStudiesGroup();
        }

        // Обработка изменения учебной группы
        private void cbStudiesGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbHours.Text = "";
            lbHours.Text = "Часы";
            SetActivityCBTypeOfActivity();
            SetLevelCBLevelEducation();
        }

        // Обработка изменения вида занятий
        private void cbTypeOfActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbHours.Text = "";Console.WriteLine("По плану: " + GetHoursPlan() + "  Выполнено: " + GetCompleteHours());
            lbHours.Text = "Часы (доступно: " + (GetHoursPlan() - GetCompleteHours()) + ")";
        }

        // Обработка изменения формы обучения
        private void cbStudiesForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isInput)
            {
                LoadTableReportsWithArguments();
            }
        }

        // Обработка печати
        private void btnPrint_Click(object sender, EventArgs e)
        {
            SetVisiblePay();

            if (cbTeacher.SelectedIndex == -1)
            {
                MessageBox.Show("Нет преподавателя !");
                return;
            }

            if (reportsTable.Rows.Count == 0)
            {
                MessageBox.Show("Нечего печатать !");
                return;
            }

            if(!PrepareOfReport()) return;

            using (Workbook workbook = new Workbook())
            {
                workbook.LoadFromFile(templete);
                workbook.PrintDocument.Print();
            }

            PrinterSettings printerSetttings = new PrinterSettings();
            string printer = printerSetttings.PrinterName;

            MessageBox.Show("Документ отправлен на печать.\nПринтер: " + printer);
        }

        // Обработка оплаты
        private void btnOK_Click(object sender, EventArgs e)
        {
            SetVisiblePay();

            if (tbPassword.Text == psw)
            {
                MarkPay();
            }
            else
            {
                MessageBox.Show("Не угадал )");
            }
        }
    }
}
