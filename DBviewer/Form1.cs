using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBviewer
{
    public partial class Form1 : Form
    {
        const string dataBase = "27231685c0687995.db";
        const string connectionString = "Data Source=" + dataBase + "; Version=3; FailIfMissing=True";

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
            LoadDisciplinesTable();
            CreateNewColumnsIfNotExist();
            CreateTableReports();
            LoadTeachersTable();
            CreateTableCurConsultHours();
            GeneralTable();
            FillCBTeacher();

            

            //dgvTable.DataSource = curConsultHoursTable;
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
                MessageBox.Show("Не удалось SELECT * FROM Disciplines LoadDisciplinesTable");
                Application.Exit();
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
                MessageBox.Show("Не удалось SELECT * FROM Disciplines");
                //Application.Exit();
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
            catch (SQLiteException ex)
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
                catch (SQLiteException e)
                {
                    MessageBox.Show("Не удалось добавить столбец FullFIO" + e);
                }

                MessageBox.Show("Не удалось SELECT FullFIO FROM Teachers " + ex);
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
            catch (SQLiteException ex)
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
                catch (SQLiteException e)
                {
                    MessageBox.Show("Не удалось добавить столбец HErate" + e);
                }

                MessageBox.Show("Не удалось SELECT HErate FROM Teachers " + ex);
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
            catch (SQLiteException ex)
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
                catch (SQLiteException e)
                {
                    MessageBox.Show("Не удалось добавить столбец SPErate" + e);
                }

                MessageBox.Show("Не удалось SELECT SPErate FROM Teachers " + ex);
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
                    MessageBox.Show("Не удалось FIO Teachers");
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
                MessageBox.Show("Не удалось GeneralTable");
                //Application.Exit();
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
                MessageBox.Show("Не удалось CreateTableReports");
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
                MessageBox.Show("Не удалось Load Table Reports");
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
                MessageBox.Show("Load Table Reports With Arguments");
                Application.Exit();
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
                MessageBox.Show("Не удалось подключиться к БД");
                Application.Exit();
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
                MessageBox.Show("Не удалось GetLectureHours");
                return 0;
                //Application.Exit();
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
                MessageBox.Show("Не удалось GetPracticeHours");
                return 0;
                //Application.Exit();
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
                MessageBox.Show("Не удалось GetLabHours");
                return 0;
                //Application.Exit();
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
                MessageBox.Show("Не удалось GetFinAttestHours");
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
                MessageBox.Show("Не удалось GetCreditHours");
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
                MessageBox.Show("Не удалось GetExamConsultHours");
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
                MessageBox.Show("Не удалось GetExamHours");
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
                MessageBox.Show("Не удалось получить LectureHoursComplete");
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
                MessageBox.Show("Не удалось получить PracticeHoursComplete");
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
                MessageBox.Show("Не удалось получить LabHoursComplete");
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
                MessageBox.Show("Не удалось получить GetCurConsultHoursComplete");
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
                MessageBox.Show("Не удалось получить GetExamConsultHoursComplete");
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
                MessageBox.Show("Не удалось получить GetCreditHoursComplete");
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
                MessageBox.Show("Не удалось получить FinAttestHoursComplete");
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
                MessageBox.Show("Не удалось получить ExamHoursComplete");
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

        // Получить сумму оплаты прописью
        private string GetStringPayment(int value)
        {
            return "(" + RusNumber.Str(value) + " рублей 00 коп.)";
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
                MessageBox.Show("Не удалось SetDiscCBDiscipline");
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
                MessageBox.Show("Не удалось SetGroupCBStudiesGroup");
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
                MessageBox.Show("Не удалось подключиться к БД");
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
                    cbLevelEducation.Text = GetStringValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось подключиться к БД");
            }
        }

        // Переход на форму отчетов
        private void btnIntel_Click(object sender, EventArgs e)
        {
            lbMonth.Visible = true; cbMonth.Visible = true;
            lbYear.Visible = true; cbYear.Visible = true;
            lbStadiesForm.Visible = true; cbStudiesForm.Visible = true;
            lbTeacher.Visible = true; cbTeacher.Visible = true;
            btnIntel.Visible = false; btnBackGeneral.Visible = true;
            btnInput.Visible = true; btnPaid.Visible = true;
            btnViewAll.Visible = true; btnAdditionalInfo.Visible = false;

            cbMonth.SelectedIndex = DateTime.Today.Month - 1;
            cbYear.SelectedItem = DateTime.Today.Year.ToString();
            cbStudiesForm.SelectedIndex = 0;

            LoadTableReports();
            dgvTable.DataSource = reportsTable;
        }

        // Возврат на общую форму из формы отчетов
        private void btnBackGeneral_Click(object sender, EventArgs e)
        {
            lbMonth.Visible = false; cbMonth.Visible = false;
            lbYear.Visible = false; cbYear.Visible = false;
            lbStadiesForm.Visible = false; cbStudiesForm.Visible = false;
            lbTeacher.Visible = false; cbTeacher.Visible = false;
            btnIntel.Visible = true; btnBackGeneral.Visible = false;
            btnInput.Visible = false; btnPaid.Visible = false;
            btnViewAll.Visible = false; btnAdditionalInfo.Visible = true;

            cbTeacher.SelectedIndex = -1;

            dgvTable.DataSource = generalTable;
        }

        // Показать весь Report
        private void btnViewAll_Click(object sender, EventArgs e)
        {
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

        // Переход на форму ввода проведенных часов
        private void btnInput_Click(object sender, EventArgs e)
        {
            btnBackReports.Visible = true; btnBackGeneral.Visible = false; btnInput.Visible = false;
            cbStudiesForm.Visible = false; lbStadiesForm.Visible = false; btnPaid.Visible = false;
            btnViewAll.Visible = false; btnEnter.Visible = true;
            cbDay.Visible = true; lbDay.Visible = true;
            cbDiscipline.Visible = true; lbDiscipline.Visible = true;
            cbStudiesGroup.Visible = true; lbStudiesGroup.Visible = true;
            cbTypeOfActivity.Visible = true; lbTypeOfActivity.Visible = true;
            tbHours.Visible = true; lbHours.Visible = true;
            cbLevelEducation.Visible = true; lbLevelEducation.Visible = true;
            cbHalfYear.Visible = true; lbHalfYear.Visible = true;

            isInput = true;
            tbHours.Text = "";
            cbTeacher.SelectedIndex = -1;
            cbHalfYear.SelectedIndex = cbMonth.SelectedIndex > 6 && cbMonth.SelectedIndex < 12 ? 0 : 1;
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
            cbLevelEducation.Visible = false; lbLevelEducation.Visible = false;
            cbHalfYear.Visible = false; lbHalfYear.Visible = false;

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
                                    ", '" + GetStringWith0(cbDay.SelectedIndex + 1) + "." + GetStringWith0(cbMonth.SelectedIndex + 1) + "." + cbYear.Text.Remove(0, 2) +
                                    "', '" + GetStringWith0(cbMonth.SelectedIndex + 1) + cbYear.Text + cbTeacher.Text + cbLevelEducation.Text +
                                    "')", connection);
                        CMD.ExecuteNonQuery();
                        LoadTableReports();
                        dgvTable.DataSource = reportsTable;
                    }
                    MessageBox.Show("Данные успешно внесены.");
                    cbTypeOfActivity.SelectedIndex = -1;
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("Вылет в INSERTe: " + ex);
                }
            }
        }

        // Ввод дополнительной информации о преподавателях
        private void btnAdditionalInfo_Click(object sender, EventArgs e)
        {
            btnSaveAdditionalInfo.Visible = true; btnBackAdditionalInfo.Visible = true;
            btnAdditionalInfo.Visible = false; btnIntel.Visible = false;

            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT Id, FIO AS Преподаватель, FullFIO AS 'Полное ФИО', HErate AS 'Ставка ВО', SPErate AS 'Ставка СПО'" +
                        " FROM Teachers WHERE FIO LIKE '%Почасов%'", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dgvTable.DataSource = table;
                }

                dgvTable.ReadOnly = false;
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось btnAdditionalInfo_Click");
            }
        }

        // Возврат из ввода дополнительной информации без сохранения
        private void btnBackAdditionalInfo_Click(object sender, EventArgs e)
        {
            btnSaveAdditionalInfo.Visible = false; btnBackAdditionalInfo.Visible = false;
            btnAdditionalInfo.Visible = true; btnIntel.Visible = true;

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
                MessageBox.Show("Данные успешно внесены.");
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось внести данные.");
            }

            btnSaveAdditionalInfo.Visible = false; btnBackAdditionalInfo.Visible = false;
            btnAdditionalInfo.Visible = true; btnIntel.Visible = true;

            dgvTable.DataSource = generalTable; dgvTable.ReadOnly = true;
        }

        // Обработка изменения месяца
        private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            DaysInMonth();
            cbHalfYear.SelectedIndex = cbMonth.SelectedIndex < 8 && cbMonth.SelectedIndex > 0 ? 1 : 0;
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
            tbHours.Text = "";
            Console.WriteLine("По плану: " + GetHoursPlan() + "  Выполнено: " + GetCompleteHours());
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

    }
}
