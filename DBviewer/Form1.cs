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
        DataTable detailInfoTable = new DataTable();

        Form2 form2;

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

            this.Text = "DBviewer - " + dataBase;

            LoadDisciplinesTable();
            CreateNewColumnsIfNotExist();
            CreateDetailInfoTable();
            CreateTableReports();
            LoadTeachersTable();
            CreateTableCurConsultHours();
            CreateDirectorColumn();
            cbPeriod.SelectedIndex = 0;
            cbLevel.SelectedIndex = 0;
            GeneralTable();
            FillCBTeacher();
            Logo();
            SetYearAndMonth();
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

        // Получить ФИО руководителя кафедры
        private string GetDirectorName(int semester)
        {
            string name;
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT FIO" +
                              " FROM Teachers WHERE Id = (SELECT IdFinAttest FROM Disciplines WHERE DisName LIKE '%ук%' " +
                              "AND DisName LIKE '%каф%' AND Semester = " + semester + ")", connection);
                    name = GetStringValue(CMD.ExecuteScalar());
                }

                int index1 = name.IndexOf(" ");
                return name.Substring(index1 + 1).Trim() + " " + name.Substring(0, index1).Trim();
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить руководителя кафедры");
                return "";
            }
        }

        // Создание столбца DirectorName в таблице Info если не существует
        private void CreateDirectorColumn()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT DirectorName FROM Info", connection);
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
                        CMD = new SQLiteCommand("ALTER TABLE Info ADD COLUMN DirectorName TEXT(100)", connection);
                        CMD.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException)
                {
                    MessageBox.Show("Не удалось добавить столбец DirectorName в таблицу Info");
                }
            }

            if (GetDepartmentDirector() == "")
            {
                // Получить руководителя
                int month = DateTime.Today.Month;
                int semester = month < 9 && month > 1 ? 2 : 1;
                string directorName = GetDirectorName(semester);
                try
                {
                    using (connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        CMD = new SQLiteCommand("UPDATE Info SET DirectorName = '" + directorName +
                            "' WHERE Id = 1", connection);
                        CMD.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException)
                {
                    MessageBox.Show("Не удалось внести DirectorName в таблицу Info");
                }
            }
        }

        // Получить название кафедры
        private string GetDepartmentName()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT DepartmentName" +
                              " FROM Info WHERE Id = 1", connection);
                    return GetStringValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить название кафедры");
                return "";
            }
        }

        // Получит руководителя кафедры из таблицы Info
        private string GetDepartmentDirector()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT DirectorName" +
                              " FROM Info WHERE Id = 1", connection);
                    return GetStringValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить руководителя кафедры");
                return "";
            }
        }

        // Установка логотипа
        private void Logo()
        {
            string departmentName = GetDepartmentName();
            switch (departmentName)
            {
                case "АСОИУ": pictureBox.Image = Properties.Resources.asoiu;
                    break;
                case "ЭПП":
                    pictureBox.Image = Properties.Resources.espp;
                    break;
                case "ТМ":
                    pictureBox.Image = Properties.Resources.tm;
                    break;
                case "ТТП":
                    pictureBox.Image = Properties.Resources.ttp;
                    break;
                case "end":
                    pictureBox.Image = Properties.Resources.end;
                    break;
                case "ЭиГН":
                    pictureBox.Image = Properties.Resources.egn;
                    break;
                case "iygn":
                    pictureBox.Image = Properties.Resources.iygn;
                    break;
                case "tti":
                    pictureBox.Image = Properties.Resources.tti;
                    break;
                default:
                    pictureBox.Image = Properties.Resources.UnKnown;
                    break;
            }
        }

        // Создание новых столбцов если не существуют в таблице Teachers
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
            curConsultHoursTable.Columns.Add("Semester", typeof(Int32));
            curConsultHoursTable.Columns.Add("LevelEducation", typeof(String));
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
                tableRow["Semester"] = GetSemesterFromTable(row);
                tableRow["LevelEducation"] = row["LevelEducation"];
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
        
        // Создание таблицы DetailInfoTable
        private void CreateDetailInfoTable()
        {
            detailInfoTable.Columns.Add("Дисциплина", typeof(string));
            detailInfoTable.Columns.Add("Группа", typeof(string));
            detailInfoTable.Columns.Add("Курс", typeof(Int32));
            detailInfoTable.Columns.Add("Семестр", typeof(Int32));
            detailInfoTable.Columns.Add("Продолжительность", typeof(Int32));
            detailInfoTable.Columns.Add("Студентов", typeof(Int32));
            detailInfoTable.Columns.Add("Лекции", typeof(Int32));
            detailInfoTable.Columns.Add("Практика", typeof(Int32));
            detailInfoTable.Columns.Add("Лабораторные работы", typeof(Int32));
            detailInfoTable.Columns.Add("Текущие консультации", typeof(Int32));
            detailInfoTable.Columns.Add("Предэкзаменационные консультации", typeof(Int32));
            detailInfoTable.Columns.Add("Экзамен", typeof(Int32));
            detailInfoTable.Columns.Add("Зачет", typeof(Int32));
            detailInfoTable.Columns.Add("Прочие", typeof(Int32));
            detailInfoTable.Columns.Add("Всего", typeof(Int32));
        }

        // Подготовка таблицы детальной информации по преподавателю
        private void PrepareDetailInfoTable(string fio)
        {
            int teacherId = GetTeacherId(fio);
            detailInfoTable.Clear();
            DataRow dataRow;

            int totalLecture = 0, totalPractice = 0, totalLab = 0, totalCurConsult = 0, totalExamConsult = 0, totalExam = 0, totalCredit = 0, totalFinAttest = 0, totalAll = 0;

            for (int semester = 1; semester < 3; semester++)
            {
                int lecture = 0, practice = 0, lab = 0, curConsult = 0, examConsult = 0, exam = 0, credit = 0, finAttest = 0, all = 0;
                dataRow = detailInfoTable.NewRow();
                if (semester == 1) dataRow["Дисциплина"] = "ПЕРВОЕ ПОЛУГОДИЕ:"; else dataRow["Дисциплина"] = "ВТОРОЕ ПОЛУГОДИЕ:";
                detailInfoTable.Rows.Add(dataRow);

                foreach (DataRow disRow in disciplinesTable.Rows)
                {
                    if (GetIntValue(disRow["IdLector"]) == teacherId || GetIntValue(disRow["IdPractice"]) == teacherId ||
                        GetIntValue(disRow["IdLab1"]) == teacherId || GetIntValue(disRow["IdLab2"]) == teacherId ||
                        GetIntValue(disRow["IdFinAttest"]) == teacherId || GetIntValue(disRow["IdExam"]) == teacherId)
                    {
                        if (GetSemesterFromTable(disRow) == semester)
                        {
                            dataRow = detailInfoTable.NewRow();
                            dataRow["Дисциплина"] = GetStringValue(disRow["DisName"]);
                            if (GetStringValue(disRow["GroupName"]) != "") dataRow["Группа"] = GetStringValue(disRow["GroupName"]);
                            if (GetIntValue(disRow["Semester"]) != 0) dataRow["Курс"] = (GetIntValue(disRow["Semester"]) + 1) / 2;
                            if (GetIntValue(disRow["Semester"]) != 0) dataRow["Семестр"] = GetIntValue(disRow["Semester"]);
                            if (GetIntValue(disRow["CountWeeks"]) != 0) dataRow["Продолжительность"] = GetIntValue(disRow["CountWeeks"]);
                            if (GetIntValue(disRow["CountStudents"]) != 0) dataRow["Студентов"] = GetIntValue(disRow["CountStudents"]);
                            if (GetIntValue(disRow["IdLector"]) == teacherId && GetIntValue(disRow["LectureHours"]) != 0) { lecture += GetIntValue(disRow["LectureHours"]); dataRow["Лекции"] = GetIntValue(disRow["LectureHours"]); }
                            if (GetIntValue(disRow["IdPractice"]) == teacherId && GetIntValue(disRow["PracticeHours"]) != 0) { practice += GetIntValue(disRow["PracticeHours"]); dataRow["Практика"] = GetIntValue(disRow["PracticeHours"]); }
                            if (GetIntValue(disRow["IdLab1"]) == teacherId && GetIntValue(disRow["IdLab2"]) == teacherId && GetIntValue(disRow["LabHours"]) != 0) { lab += GetIntValue(disRow["LabHours"]); dataRow["Лабораторные работы"] = GetIntValue(disRow["LabHours"]); }
                            if (((GetIntValue(disRow["IdLab1"]) == teacherId && GetIntValue(disRow["IdLab2"]) != teacherId) ||
                                (GetIntValue(disRow["IdLab1"]) != teacherId && GetIntValue(disRow["IdLab2"]) == teacherId)) && GetIntValue(disRow["LabHours"]) != 0) { lab += GetIntValue(disRow["LabHours"]) / 2; dataRow["Лабораторные работы"] = GetIntValue(disRow["LabHours"]) / 2; }
                            int curConHours = GetCurConsultHours(fio, GetStringValue(disRow["DisName"]), GetStringValue(disRow["GroupName"]), semester);
                            if (curConHours != 0) { curConsult += curConHours; dataRow["Текущие консультации"] = curConHours; }
                            if (GetIntValue(disRow["IdExam"]) == teacherId)
                            {
                                if (GetIntValue(disRow["ExamConsultHours"]) != 0) { examConsult += GetIntValue(disRow["ExamConsultHours"]); dataRow["Предэкзаменационные консультации"] = GetIntValue(disRow["ExamConsultHours"]); }
                                if (GetIntValue(disRow["ExamHours"]) != 0) { exam += GetIntValue(disRow["ExamHours"]); dataRow["Экзамен"] = GetIntValue(disRow["ExamHours"]); }
                                if (GetIntValue(disRow["CreditHours"]) != 0) { credit += GetIntValue(disRow["CreditHours"]); dataRow["Зачет"] = GetIntValue(disRow["CreditHours"]); }
                            }
                            if (GetIntValue(disRow["IdFinAttest"]) == teacherId && GetIntValue(disRow["FinAttestHours"]) != 0) { finAttest += GetIntValue(disRow["FinAttestHours"]); dataRow["Прочие"] = GetIntValue(disRow["FinAttestHours"]); }
                            int count = GetIntValue(dataRow["Лекции"]) + GetIntValue(dataRow["Практика"]) + GetIntValue(dataRow["Лабораторные работы"]) + GetIntValue(dataRow["Текущие консультации"]) +
                                GetIntValue(dataRow["Предэкзаменационные консультации"]) + GetIntValue(dataRow["Экзамен"]) + GetIntValue(dataRow["Зачет"]) + GetIntValue(dataRow["Прочие"]);
                            if (count != 0) dataRow["Всего"] = count;
                            all += count;
                            detailInfoTable.Rows.Add(dataRow);
                        }
                    }
                }
                dataRow = detailInfoTable.NewRow();
                if (semester == 1) dataRow["Дисциплина"] = "ИТОГО ЗА ПЕРВОЕ ПОЛУГОДИЕ"; else dataRow["Дисциплина"] = "ИТОГО ЗА ВТОРОЕ ПОЛУГОДИЕ";
                if (lecture != 0) dataRow["Лекции"] = lecture;
                if (practice != 0) dataRow["Практика"] = practice;
                if (lab != 0) dataRow["Лабораторные работы"] = lab;
                if (curConsult != 0) dataRow["Текущие консультации"] = curConsult;
                if (examConsult != 0) dataRow["Предэкзаменационные консультации"] = examConsult;
                if (exam != 0) dataRow["Экзамен"] = exam;
                if (credit != 0) dataRow["Зачет"] = credit;
                if (finAttest != 0) dataRow["Прочие"] = finAttest;
                if (all != 0) dataRow["Всего"] = all;
                totalLecture += lecture; totalPractice += practice; totalLab += lab; totalCurConsult += curConsult;
                totalExamConsult += examConsult; totalExam += exam; totalCredit += credit; totalFinAttest += finAttest; totalAll += all;
                detailInfoTable.Rows.Add(dataRow);
            }
            dataRow = detailInfoTable.NewRow();
            dataRow["Дисциплина"] = "ИТОГО ЗА УЧЕБНЫЙ ГОД";
            if (totalLecture != 0) dataRow["Лекции"] = totalLecture;
            if (totalPractice != 0) dataRow["Практика"] = totalPractice;
            if (totalLab != 0) dataRow["Лабораторные работы"] = totalLab;
            if (totalCurConsult != 0) dataRow["Текущие консультации"] = totalCurConsult;
            if (totalExamConsult != 0) dataRow["Предэкзаменационные консультации"] = totalExamConsult;
            if (totalExam != 0) dataRow["Экзамен"] = totalExam;
            if (totalCredit != 0) dataRow["Зачет"] = totalCredit;
            if (totalFinAttest != 0) dataRow["Прочие"] = totalFinAttest;
            if (totalAll != 0) dataRow["Всего"] = totalAll;
            detailInfoTable.Rows.Add(dataRow);
        }
        
        // Генерация общей таблицы
        private void GeneralTable()
        {
            generalTable.Columns.Add("№ п_п");
            generalTable.Columns.Add("Преподаватель");

            DataTable tempTable = new DataTable();

            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT Id AS '№ п_п', FIO AS Преподаватель" +
                                            " FROM Teachers ORDER BY FIO", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    adapter.Fill(tempTable);

                    foreach(DataRow row in tempTable.Rows)
                    {
                        if (row["Преподаватель"].ToString().Contains("очасов"))
                        {
                            DataRow dataRow = generalTable.NewRow();
                            dataRow[0] = row[0]; dataRow[1] = row[1];
                            generalTable.Rows.Add(dataRow);
                        }
                    }

                    foreach (DataRow row in tempTable.Rows)
                    {
                        if (!row["Преподаватель"].ToString().Contains("очасов"))
                        {
                            DataRow dataRow = generalTable.NewRow();
                            dataRow[0] = row[0]; dataRow[1] = row[1];
                            generalTable.Rows.Add(dataRow);
                        }
                    }

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
                PlacedGeneralTable();
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось создать табличные данные generalTable");
            }
        }

        // Размещение GeneralTable в dgvTable
        private void PlacedGeneralTable() 
        {
            dgvTable.DataSource = generalTable;
            dgvTable.Columns["№ п_п"].Visible = false;
            dgvTable.Columns["Объем нагрузки"].Visible = false;
            dgvTable.Columns["Преподаватель"].Width = 165;
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

        // Получить семестр из таблицы disciplinesTable
        private int GetSemesterFromTable(DataRow disRow)
        {
            return GetIntValue(disRow["Semester"]) % 2 == 0 ? 2 : 1;
        }

        // Заполнение главной таблицы
        private void FillGeneralTable()
        {
            LoadTableReports();

            foreach(DataRow row in generalTable.Rows)
            {
                int sumLectureHours = 0, sumPracticeHours = 0, sumLabHours = 0, 
                    sumExamConsultHours = 0, sumCreditHours = 0, sumExamHours = 0, sumFinAttestHours = 0;
                
                foreach (DataRow disRow in disciplinesTable.Rows)
                {
                    bool isSemesterGeneral = IsSemesterGeneral(disRow);
                    bool isLevelGeneral = IsLevelGeneral(disRow);

                    if (GetIntValue(disRow["IdLector"]) == GetIntValue(row["№ п_п"]) && isSemesterGeneral && isLevelGeneral) sumLectureHours += GetIntValue(disRow["LectureHours"]);
                    if (GetIntValue(disRow["IdPractice"]) == GetIntValue(row["№ п_п"]) && isSemesterGeneral && isLevelGeneral) sumPracticeHours += GetIntValue(disRow["PracticeHours"]);
                    if (GetIntValue(disRow["IdLab1"]) == GetIntValue(row["№ п_п"]) && GetIntValue(disRow["IdLab2"]) == GetIntValue(row["№ п_п"]) && isSemesterGeneral && isLevelGeneral) sumLabHours += GetIntValue(disRow["LabHours"]);
                    if (((GetIntValue(disRow["IdLab1"]) == GetIntValue(row["№ п_п"]) && GetIntValue(disRow["IdLab2"]) != GetIntValue(row["№ п_п"])) ||
                        (GetIntValue(disRow["IdLab1"]) != GetIntValue(row["№ п_п"]) && GetIntValue(disRow["IdLab2"]) == GetIntValue(row["№ п_п"]))) && isSemesterGeneral && isLevelGeneral) sumLabHours += (GetIntValue(disRow["LabHours"]) / 2);
                    if (GetIntValue(disRow["IdExam"]) == GetIntValue(row["№ п_п"]) && isSemesterGeneral && isLevelGeneral)
                    {
                        sumExamConsultHours += GetIntValue(disRow["ExamConsultHours"]);
                        sumExamHours += GetIntValue(disRow["ExamHours"]);
                        sumCreditHours += GetIntValue(disRow["CreditHours"]);
                    }
                    if (GetIntValue(disRow["IdFinAttest"]) == GetIntValue(row["№ п_п"]) && isSemesterGeneral && isLevelGeneral) sumFinAttestHours += GetIntValue(disRow["FinAttestHours"]);
                }

                if (sumLectureHours == 0) row["Лекции план"] = DBNull.Value; else row["Лекции план"] = sumLectureHours;
                if (sumPracticeHours == 0) row["Практика план"] = DBNull.Value; else row["Практика план"] = sumPracticeHours;
                if (sumLabHours == 0) row["Лабораторные план"] = DBNull.Value; else row["Лабораторные план"] = sumLabHours;
                int sum = GetSumCurConsultHour(GetStringValue(row["Преподаватель"]), cbPeriod.SelectedIndex, cbLevel.Text);
                if (sum == 0) row["ТекКонсультации план"] = DBNull.Value; else row["ТекКонсультации план"] = sum;
                if (sumExamConsultHours == 0) row["Предэкзаменационные консультации план"] = DBNull.Value; else row["Предэкзаменационные консультации план"] = sumExamConsultHours;
                if (sumExamHours == 0) row["Экзамены план"] = DBNull.Value; else row["Экзамены план"] = sumExamHours;
                if (sumCreditHours == 0) row["Зачеты план"] = DBNull.Value; else row["Зачеты план"] = sumCreditHours;
                if (sumFinAttestHours == 0) row["Итоговые аттестации план"] = DBNull.Value; else row["Итоговые аттестации план"] = sumFinAttestHours;

                sumLectureHours = 0; sumPracticeHours = 0; sumLabHours = 0; 
                sumExamConsultHours = 0; sumCreditHours = 0; sumExamHours = 0; sumFinAttestHours = 0;
                int sumCurConsultHours = 0;
                string name = GetTeacherHours(GetStringValue(row["Преподаватель"]));

                foreach (DataRow repRow in reportsTable.Rows)
                {
                    if (GetStringValue(repRow["ИА ГАК ГЭК"]).Contains(name) && IsSemesterReports(repRow) && IsLevelReports(repRow))
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

                if (sumLectureHours == 0) row["Лекции выполнено"] = DBNull.Value; else row["Лекции выполнено"] = sumLectureHours;
                if (sumPracticeHours == 0) row["Практика выполнено"] = DBNull.Value; else row["Практика выполнено"] = sumPracticeHours;
                if (sumLabHours == 0) row["Лабораторные выполнено"] = DBNull.Value; else row["Лабораторные выполнено"] = sumLabHours;
                if (sumCurConsultHours == 0) row["ТекКонсультации выполнено"] = DBNull.Value; else row["ТекКонсультации выполнено"] = sumCurConsultHours;
                if (sumExamConsultHours == 0) row["Предэкзаменационные консультации выполнено"] = DBNull.Value; else row["Предэкзаменационные консультации выполнено"] = sumExamConsultHours;
                if (sumExamHours == 0) row["Экзамены выполнено"] = DBNull.Value; else row["Экзамены выполнено"] = sumExamHours;
                if (sumCreditHours == 0) row["Зачеты выполнено"] = DBNull.Value; else row["Зачеты выполнено"] = sumCreditHours;
                if (sumFinAttestHours == 0) row["Итоговые аттестации выполнено"] = DBNull.Value; else row["Итоговые аттестации выполнено"] = sumFinAttestHours;


                sum = GetIntValue(row["Лекции план"]) + GetIntValue(row["Практика план"]) +
                                    GetIntValue(row["Лабораторные план"]) + GetIntValue(row["ТекКонсультации план"]) +
                                    GetIntValue(row["Предэкзаменационные консультации план"]) + GetIntValue(row["Экзамены план"]) +
                                    GetIntValue(row["Зачеты план"]) + GetIntValue(row["Итоговые аттестации план"]);
                if (sum == 0) row["Всего план"] = DBNull.Value; else row["Всего план"] = sum;

                sum = GetIntValue(row["Лекции выполнено"]) + GetIntValue(row["Практика выполнено"]) +
                                    GetIntValue(row["Лабораторные выполнено"]) + GetIntValue(row["ТекКонсультации выполнено"]) +
                                    GetIntValue(row["Предэкзаменационные консультации выполнено"]) + GetIntValue(row["Экзамены выполнено"]) +
                                    GetIntValue(row["Зачеты выполнено"]) + GetIntValue(row["Итоговые аттестации выполнено"]);
                if (sum == 0) row["Всего выполнено"] = DBNull.Value; else row["Всего выполнено"] = sum;

                // Получить соответствие выбранного полугодия из таблицы General
                bool IsSemesterGeneral(DataRow disRow)
                {
                    if (cbPeriod.SelectedIndex == 0) return true;
                    int semester = GetIntValue(disRow["Semester"]) % 2 == 0 ? 2 : 1;
                    if (cbPeriod.SelectedIndex == semester) return true;
                    return false;
                }

                // Получить соответствие выбранного уровня образования из таблицы General
                bool IsLevelGeneral(DataRow disRow)
                {
                    if (cbLevel.SelectedIndex == 0) return true;
                    if (GetStringValue(disRow["LevelEducation"]) == cbLevel.Text) return true;
                    return false;
                }

                // Получить соответствие выбранного полугодия из таблицы Reports
                bool IsSemesterReports(DataRow repRow)
                {
                    if (cbPeriod.SelectedIndex == 0) return true;
                    //string str = GetStringValue(repRow["ИА ГАК ГЭК"]);
                    //str = str.Substring(0, 2);
                    //int month = Convert.ToInt32(str);
                    //int semester = month < 9 && month > 1 ? 2 : 1;
                    if (cbPeriod.SelectedIndex == GetIntValue(repRow["Полугодие"])) return true;
                    return false;
                }

                // Получить соответствие выбранного уровня образования из таблицы Reports
                bool IsLevelReports(DataRow repRow)
                {
                    if (cbLevel.SelectedIndex == 0) return true;
                    string str = GetStringValue(repRow["ИА ГАК ГЭК"]);
                    str = str.Substring(str.Length - 2, 2);
                    if (cbLevel.Text.Contains(str)) return true;
                    return false;
                }
            }
        }

        // Получить сумму часов текущих консультаций для преподавателя
        private int GetSumCurConsultHour(string fio, int semester, string level)
        {
            int sum = 0;
            foreach (DataRow dataRow in curConsultHoursTable.Rows)
            {
                    if (fio == GetStringValue(dataRow["FIOLector"]) && IsSemester(dataRow) && IsLevel(dataRow)) sum += GetIntValue(dataRow["LectorHours"]);
                    if (fio == GetStringValue(dataRow["FIOPractice"]) && IsSemester(dataRow) && IsLevel(dataRow)) sum += GetIntValue(dataRow["PracticeHours"]);
                    if (fio == GetStringValue(dataRow["FIOLab1"]) && IsSemester(dataRow) && IsLevel(dataRow)) sum += GetIntValue(dataRow["Lab1Hours"]);
                    if (fio == GetStringValue(dataRow["FIOLab2"]) && IsSemester(dataRow) && IsLevel(dataRow)) sum += GetIntValue(dataRow["Lab2Hours"]);
            }
            return sum;

            bool IsSemester(DataRow row)
            {
                if (semester == 0) return true;
                if (semester == GetIntValue(row["Semester"])) return true;
                return false;
            }

            bool IsLevel(DataRow row)
            {
                if (level == "ВО+СПО") return true;
                if (level == GetStringValue(row["LevelEducation"])) return true;
                return false;
            }
        }

        // Получить часы текущих консультаций для преподавателя, дисциплины, группы, семестра
        private int GetCurConsultHours(string fio, string discipline, string group, int semester)
        {
            int sum = 0;
            foreach (DataRow dataRow in curConsultHoursTable.Rows)
            {
                if (discipline == GetStringValue(dataRow["DisName"]) && group == GetStringValue(dataRow["GroupName"]) && semester == GetIntValue(dataRow["Semester"])) 
                {
                    if (fio == GetStringValue(dataRow["FIOLector"])) sum += GetIntValue(dataRow["LectorHours"]);
                    if (fio == GetStringValue(dataRow["FIOPractice"])) sum += GetIntValue(dataRow["PracticeHours"]);
                    if (fio == GetStringValue(dataRow["FIOLab1"])) sum += GetIntValue(dataRow["Lab1Hours"]);
                    if (fio == GetStringValue(dataRow["FIOLab2"])) sum += GetIntValue(dataRow["Lab2Hours"]);
                }
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
                            " DisName TEXT(100), GroupName TEXT(20), HalfYear INTEGER, LectureHours INTEGER, PracticeHours INTEGER," +
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
                    CMD = new SQLiteCommand("SELECT Id, DisName AS Дисциплина, GroupName AS Группа, HalfYear AS Полугодие, LectureHours AS Лекции, PracticeHours AS Практика," +
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
                    CMD = new SQLiteCommand("SELECT Id, DisName AS Дисциплина, GroupName AS Группа, HalfYear AS Полугодие, LectureHours AS Лекции, PracticeHours AS Практика," +
                        " LabHours AS Лабораторные, CurConsultHours AS 'Текущие консультации', ExamConsultHours AS 'Предэкзаменационные консультации'," +
                        " ExamHours AS Экзамены, CreditHours AS Зачеты, FinAttestHours AS 'Итоговая аттестация'," +
                        " TotalHours AS Всего, Date AS Дата, IaGakGek AS 'ИА ГАК ГЭК', PaymentMark AS Оплата FROM Reports WHERE" +
                        " IaGakGek LIKE '" + GetStringWith0(cbMonth.SelectedIndex + 1) + "%' AND IaGakGek LIKE '%" + cbYear.Text + "%' AND IaGakGek LIKE '%" +
                        cbTeacher.Text + "%' AND IaGakGek LIKE '%" + cbStudiesForm.Text + "%' AND PaymentMark IS NULL AND HalfYear = " + (cbHalfYear2.SelectedIndex + 1), connection);
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
                                            " FROM Teachers WHERE FIO LIKE '%очасов%' ORDER BY FIO", connection);
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

        // Удаление записей из таблицы отчетов
        private void DeleteFromReports()
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    foreach (DataGridViewRow dataRow in dgvTable.SelectedRows)
                    {
                        Console.WriteLine("Строка: " + dataRow.Cells["Id"].Value);

                        CMD = new SQLiteCommand("DELETE FROM Reports WHERE Id = " + dataRow.Cells["Id"].Value + " AND PaymentMark IS NULL", connection);
                        CMD.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Операция прошла успешно.\nСтроки с пометкой об оплате не удаляются.");
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось удалить строку/строки");
            }
        }

        // Получит Id преподавателя
        private int GetTeacherId(string fio)
        {
            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT Id FROM Teachers WHERE FIO = '" + fio + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось получить Id преподавателя");
                return 0;
            }
        }
        
        // Получить количество часов по плану для лекций из таблицы disciplinesTable
        private int GetLectureHoursFromTable()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            string discipline = cbDiscipline.Text, group = cbStudiesGroup.Text, level = tbLevelEducation.Text;
            int teacherId = GetTeacherId(cbTeacher.Text);
            int semesterCur = GetCurSemestr();
            int sum = 0;

            foreach(DataRow dataRow in disciplinesTable.Rows)
            {
                if (GetStringValue(dataRow["DisName"]) == discipline && GetStringValue(dataRow["GroupName"]) == group &&
                    GetStringValue(dataRow["LevelEducation"]) == level && GetIntValue(dataRow["IdLector"]) == teacherId &&
                    GetSemesterFromTable(dataRow) == semesterCur) sum += GetIntValue(dataRow["LectureHours"]);
            }

            return sum;
        }

        // Получить количество часов по плану для практики из таблицы disciplinesTable
        private int GetPracticeHoursFromTable()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            string discipline = cbDiscipline.Text, group = cbStudiesGroup.Text, level = tbLevelEducation.Text;
            int teacherId = GetTeacherId(cbTeacher.Text);
            int semesterCur = GetCurSemestr();
            int sum = 0;

            foreach (DataRow dataRow in disciplinesTable.Rows)
            {
                if (GetStringValue(dataRow["DisName"]) == discipline && GetStringValue(dataRow["GroupName"]) == group &&
                    GetStringValue(dataRow["LevelEducation"]) == level && GetIntValue(dataRow["IdPractice"]) == teacherId &&
                    GetSemesterFromTable(dataRow) == semesterCur) sum += GetIntValue(dataRow["PracticeHours"]);
            }

            return sum;
        }

        // Получить количество часов по плану для лабораторных из таблицы disciplinesTable
        private int GetLabHoursFromTable()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            string discipline = cbDiscipline.Text, group = cbStudiesGroup.Text, level = tbLevelEducation.Text;
            int teacherId = GetTeacherId(cbTeacher.Text);
            int semesterCur = GetCurSemestr();
            int sum = 0;

            foreach (DataRow dataRow in disciplinesTable.Rows)
            {
                if (GetStringValue(dataRow["DisName"]) == discipline && GetStringValue(dataRow["GroupName"]) == group &&
                    GetStringValue(dataRow["LevelEducation"]) == level && GetSemesterFromTable(dataRow) == semesterCur) 
                {
                    if (GetIntValue(dataRow["IdLab1"]) == teacherId && GetIntValue(dataRow["IdLab2"]) == teacherId) sum += GetIntValue(dataRow["LabHours"]);
                    if ((GetIntValue(dataRow["IdLab1"]) == teacherId && GetIntValue(dataRow["IdLab2"]) != teacherId) ||
                        (GetIntValue(dataRow["IdLab1"]) != teacherId && GetIntValue(dataRow["IdLab2"]) == teacherId)) sum += GetIntValue(dataRow["LabHours"]) / 2;
                }
            }

            return sum;
        }

        // Получить количество часов по плану для аттестации из таблицы disciplinesTable
        private int GetFinAttestHoursFromTable()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            string discipline = cbDiscipline.Text, group = cbStudiesGroup.Text, level = tbLevelEducation.Text;
            int teacherId = GetTeacherId(cbTeacher.Text);
            int semesterCur = GetCurSemestr();
            int sum = 0;

            foreach (DataRow dataRow in disciplinesTable.Rows)
            {
                if (GetStringValue(dataRow["DisName"]) == discipline && GetStringValue(dataRow["GroupName"]) == group &&
                    GetStringValue(dataRow["LevelEducation"]) == level && GetIntValue(dataRow["IdFinAttest"]) == teacherId &&
                    GetSemesterFromTable(dataRow) == semesterCur) sum += GetIntValue(dataRow["FinAttestHours"]);
            }

            return sum;
        }

        // Получить количество часов по плану для текущей консультации
        private int GetCurConsultHoursPlan()
        {
            string teacherName = cbTeacher.Text, discipline = cbDiscipline.Text, group = cbStudiesGroup.Text, level = tbLevelEducation.Text;
            int semesterCur = GetCurSemestr();
            int sum = 0;

            foreach (DataRow dataRow in curConsultHoursTable.Rows)
            {
                if (GetStringValue(dataRow["DisName"]) == discipline && GetStringValue(dataRow["GroupName"]) == group &&
                    GetStringValue(dataRow["LevelEducation"]) == level && GetIntValue(dataRow["Semester"]) == semesterCur)
                {
                    if (GetStringValue(dataRow["FIOLector"]) == teacherName) sum += GetIntValue(dataRow["LectorHours"]);
                    if (GetStringValue(dataRow["FIOPractice"]) == teacherName) sum += GetIntValue(dataRow["PracticeHours"]);
                    if (GetStringValue(dataRow["FIOLab1"]) == teacherName) sum += GetIntValue(dataRow["Lab1Hours"]);
                    if (GetStringValue(dataRow["FIOLab2"]) == teacherName) sum += GetIntValue(dataRow["Lab2Hours"]);
                }
            }
            return sum;
        }

        // Получить количество часов по плану для зачетов из таблицы disciplinesTable
        private int GetCreditHoursFromTable()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            string discipline = cbDiscipline.Text, group = cbStudiesGroup.Text, level = tbLevelEducation.Text;
            int teacherId = GetTeacherId(cbTeacher.Text);
            int semesterCur = GetCurSemestr();
            int sum = 0;

            foreach (DataRow dataRow in disciplinesTable.Rows)
            {
                if (GetStringValue(dataRow["DisName"]) == discipline && GetStringValue(dataRow["GroupName"]) == group &&
                    GetStringValue(dataRow["LevelEducation"]) == level && GetIntValue(dataRow["IdExam"]) == teacherId &&
                    GetSemesterFromTable(dataRow) == semesterCur) sum += GetIntValue(dataRow["CreditHours"]);
            }

            return sum;
        }

        // Получить количество часов по плану для предэкзаменационных консультаций из таблицы disciplinesTable
        private int GetExamConsultHoursFromTable()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            string discipline = cbDiscipline.Text, group = cbStudiesGroup.Text, level = tbLevelEducation.Text;
            int teacherId = GetTeacherId(cbTeacher.Text);
            int semesterCur = GetCurSemestr();
            int sum = 0;

            foreach (DataRow dataRow in disciplinesTable.Rows)
            {
                if (GetStringValue(dataRow["DisName"]) == discipline && GetStringValue(dataRow["GroupName"]) == group &&
                    GetStringValue(dataRow["LevelEducation"]) == level && GetIntValue(dataRow["IdExam"]) == teacherId &&
                    GetSemesterFromTable(dataRow) == semesterCur) sum += GetIntValue(dataRow["ExamConsultHours"]);
            }

            return sum;
        }

        // Получить количество часов по плану для экзамена из таблицы disciplinesTable
        private int GetExamHoursFromTable()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            string discipline = cbDiscipline.Text, group = cbStudiesGroup.Text, level = tbLevelEducation.Text;
            int teacherId = GetTeacherId(cbTeacher.Text);
            int semesterCur = GetCurSemestr();
            int sum = 0;

            foreach (DataRow dataRow in disciplinesTable.Rows)
            {
                if (GetStringValue(dataRow["DisName"]) == discipline && GetStringValue(dataRow["GroupName"]) == group &&
                    GetStringValue(dataRow["LevelEducation"]) == level && GetIntValue(dataRow["IdExam"]) == teacherId &&
                    GetSemesterFromTable(dataRow) == semesterCur) sum += GetIntValue(dataRow["ExamHours"]);
            }

            return sum;
        }

        // Получить количество учтенных часов для лекций из таблицы reportsTable
        private int GetHoursCompleteFromTable(string field)
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            string discipline = cbDiscipline.Text, group = cbStudiesGroup.Text, 
                   level = tbLevelEducation.Text, teacher = cbTeacher.Text;
            int semesterCur = GetCurSemestr();
            int sum = 0;

            foreach (DataRow repRow in reportsTable.Rows)
            {
                if (GetStringValue(repRow["ИА ГАК ГЭК"]).Contains(teacher) && 
                    GetStringValue(repRow["Дисциплина"]) == discipline && GetStringValue(repRow["Группа"]) == group &&
                    GetIntValue(repRow["Полугодие"]) == semesterCur && IsLevelReports(repRow, level)) sum += GetIntValue(repRow[field]);
            }

            return sum;
        }

        // Получить соответствие уровня из таблицы reportsTable
        private bool IsLevelReports(DataRow repRow, string level)
        {
            string str = GetStringValue(repRow["ИА ГАК ГЭК"]);
            str = str.Substring(str.Length - 2, 2);
            if (level.Contains(str)) return true;
            return false;
        }

        // Получить количество часов по плану
        private int GetHoursPlan()
        {
            switch (cbTypeOfActivity.Text)
            {
                case "Лекции": return GetLectureHoursFromTable();
                case "Практики": return GetPracticeHoursFromTable();
                case "Лабораторные": return GetLabHoursFromTable();
                case "Тек. консульт.": return GetCurConsultHoursPlan();
                case "Пред. экзам. консульт.": return GetExamConsultHoursFromTable();
                case "Зачеты": return GetCreditHoursFromTable();
                case "Экзамены": return GetExamHoursFromTable();
                case "Итог. аттестации": return GetFinAttestHoursFromTable();
                default: return 0;
            }
        }

        // Получить количество проведенных часов
        private int GetCompleteHours()
        {
            switch (cbTypeOfActivity.Text)
            {
                case "Лекции": return GetHoursCompleteFromTable("Лекции");// GetLectureHoursComplete();
                case "Практики": return GetHoursCompleteFromTable("Практика");
                case "Лабораторные": return GetHoursCompleteFromTable("Лабораторные");
                case "Тек. консульт.": return GetHoursCompleteFromTable("Текущие консультации");
                case "Пред. экзам. консульт.": return GetHoursCompleteFromTable("Предэкзаменационные консультации");
                case "Зачеты": return GetHoursCompleteFromTable("Зачеты");
                case "Экзамены": return GetHoursCompleteFromTable("Экзамены");
                case "Итог. аттестации": return GetHoursCompleteFromTable("Итоговая аттестация");
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
        private int GetCurSemestr()
        {
            return cbHalfYear.SelectedIndex + 1;
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

        // Установка CBYear и CBMonth
        private void SetYearAndMonth()
        {
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;
            cbYear.Items.Clear();
            if (month >= 9 && month <= 12)
            {
                cbYear.Items.Add(year.ToString());
                cbYear.Items.Add((year + 1).ToString());
            }
            else
            {
                cbYear.Items.Add((year - 1).ToString());
                cbYear.Items.Add(year.ToString());
                if (month == 7 || month == 8) month = 6;
            }

            //indexMonth = month - 1;
            cbMonth.SelectedIndex = month - 1;
            cbYear.SelectedItem = year.ToString();
            //indexYear = cbYear.SelectedIndex;
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

        // Проверка выставленной даты
        private bool CheckOfDate()
        {
            int indY = cbYear.SelectedIndex;
            int indM = cbMonth.SelectedIndex;
            if ((indY == 0 && indM < 8) || (indY == 1 && indM > 5)) return false;
            return true;
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
                for(int i = 4; i <= 11; i++)
                {
                    value = GetIntValue(dataRow[i]);
                    if(value != 0)
                    {
                        switch (i)
                        {
                            case 4: typeOfActivity.Add("Лекции"); quantity.Add(value); hours[0] += value;
                                break;
                            case 5: typeOfActivity.Add("Практика"); quantity.Add(value); hours[2] += value;
                                break;
                            case 6: typeOfActivity.Add("Лаб."); quantity.Add(value); hours[1] += value;
                                break;
                            case 7: typeOfActivity.Add("Тек. конс."); quantity.Add(value); hours[3] += value;
                                break;
                            case 8: typeOfActivity.Add("Предэкзам. конс."); quantity.Add(value); hours[4] += value;
                                break;
                            case 9: typeOfActivity.Add("Экзамены"); quantity.Add(value); hours[5] += value;
                                break;
                            case 10: typeOfActivity.Add("Зачеты"); quantity.Add(value); hours[6] += value;
                                break;
                            case 11:
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

                    worksheet1.Range[7, 1].Text = "в " + (cbHalfYear2.SelectedIndex + 1) + " семестре " + cbYear.Items[0].ToString() +
                           "/" + cbYear.Items[1].ToString() + " учебного года";
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

                    worksheet1.Range[56, 5].Text = GetDepartmentName();
                    worksheet1.Range[57, 5].Text = GetDepartmentDirector();

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
                    workbook.SaveToFile((GetShortTeacherName() + "_" + cbStudiesForm.Text + "_" + cbMonth.Text + "_" + cbYear.Text).Replace(".", "_").Replace(" ", "_") + ".xlsx");
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
            cbPeriod.Visible = false; lbPeriod.Visible = false;
            cbLevel.Visible = false; lbLevel.Visible = false;
            cbHalfYear2.Visible = true; lbHalfYear2.Visible = true;

            cbStudiesForm.SelectedIndex = 0;
            cbHalfYear2.SelectedIndex = 0;

            LoadTableReports();
            dgvTable.DataSource = reportsTable;
            dgvTable.Columns["Id"].Visible = false;
            dgvTable.Columns["Дисциплина"].Width = 150;
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
            cbPeriod.Visible = true; lbPeriod.Visible = true;
            cbLevel.Visible = true; lbLevel.Visible = true;
            cbHalfYear2.Visible = false; lbHalfYear2.Visible = false;

            cbTeacher.SelectedIndex = -1;

            PlacedGeneralTable();
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
            btnPrint.Visible = false; btnDelete.Visible = true;
            cbHalfYear2.Visible = false; lbHalfYear2.Visible = false;
            cbHalfYear.Visible = true; lbHalfYear.Visible = true;

            isInput = true;
            tbHours.Text = "";
            cbTeacher.SelectedIndex = -1;
            cbHalfYear.SelectedIndex = 0;
            SetYearAndMonth();
            DaysInMonth();
            LoadTableReports();
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
            btnPrint.Visible = true; btnDelete.Visible = false;
            cbHalfYear2.Visible = true; lbHalfYear2.Visible = true;
            cbHalfYear.Visible = false; lbHalfYear.Visible = false;

            cbTeacher.SelectedIndex = -1;

            isInput = false;
        }

        // Записать внесенные данные отработанных часов
        private void btnEnter_Click(object sender, EventArgs e)
        {
            //if (!CheckOfDate()) return;
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
                
                if (hours < 1)
                {
                    MessageBox.Show("Не корретный ввод отработанных часов !");
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
                        CMD = new SQLiteCommand("INSERT INTO Reports (DisName, GroupName, HalfYear, " + GetFieldName() +
                                    ", TotalHours, Date, IaGakGek) VALUES ('" + cbDiscipline.Text + "', '" +
                                    cbStudiesGroup.Text + "', " + (cbHalfYear.SelectedIndex + 1) + ", " + hours + ", " + hours +
                                    ", '" + GetStringWith0(cbDay.SelectedIndex + 1) + "." + GetStringWith0(cbMonth.SelectedIndex + 1) + "." + cbYear.Text +
                                    "', '" + GetStringWith0(cbMonth.SelectedIndex + 1) + cbYear.Text + cbTeacher.Text + tbLevelEducation.Text +
                                    "')", connection);
                        CMD.ExecuteNonQuery();
                        LoadTableReports();
                    }

                    FillGeneralTable();
                    MessageBox.Show("Данные успешно внесены.");
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("Вылет в INSERTe: " + ex);
                }

                lbHours.Text = "Часы (доступно: " + (GetHoursPlan() - GetCompleteHours()) + ")";
            }
        }

        // Ввод дополнительной информации о преподавателях
        private void btnAdditionalInfo_Click(object sender, EventArgs e)
        {
            lbGeneralIntel.Visible = false; lbAdditionalInfo.Visible = true;
            btnSaveAdditionalInfo.Visible = true; btnBackAdditionalInfo.Visible = true;
            btnAdditionalInfo.Visible = false; btnIntel.Visible = false;
            cbPeriod.Visible = false; lbPeriod.Visible = false;
            cbLevel.Visible = false; lbLevel.Visible = false;

            try
            {
                using (connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT Id, FIO AS Преподаватель, FullFIO AS 'Полное ФИО', HErate AS 'Ставка ВО', SPErate AS 'Ставка СПО'" +
                        " FROM Teachers WHERE FIO LIKE '%очасов%' ORDER BY Преподаватель", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dgvTable.DataSource = table;
                }

                dgvTable.ReadOnly = false;
                dgvTable.Columns["Id"].Visible = false;
                dgvTable.Columns["Преподаватель"].Width = 300;
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
            cbPeriod.Visible = true; lbPeriod.Visible = true;
            cbLevel.Visible = true; lbLevel.Visible = true;

            PlacedGeneralTable();
            dgvTable.ReadOnly = true;
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

            PlacedGeneralTable();
            dgvTable.ReadOnly = true;
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

            if (!PrepareOfReport()) return;

            if (printDialog.ShowDialog() != DialogResult.OK) return;

            using (Workbook workbook = new Workbook())
            {
                workbook.LoadFromFile(templete);
                workbook.PrintDocument.Print();
            }

            MessageBox.Show("Документ отправлен на печать");
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
                MessageBox.Show("Не верный пароль !");
            }
        }

        // Обработка изменения месяца
        private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            DaysInMonth();
            if (!isInput)
            {
                LoadTableReportsWithArguments();
            }
            else
            {
                lbHours.Text = "Часы (доступно: " + (GetHoursPlan() - GetCompleteHours()) + ")";
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

        // Обрабатка события изменения дня
        private void cbDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDay.SelectedIndex == -1) return;
            if ((DateTime.Parse("01/" + (cbMonth.SelectedIndex + 1) + "/" + cbYear.Text) > DateTime.Today) || !CheckOfDate())
            {
                MessageBox.Show("Не верная дата !");
                cbDay.SelectedIndex = -1;
            }
            return;
        }
        
        // Обработка нажатия кнопки "удалить выбранные строки"
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTable.SelectedRows.Count == 0) return;
            DeleteFromReports();
            LoadTableReportsWithArguments();
        }

        private void cbPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillGeneralTable();
        }

        private void cbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillGeneralTable();
        }

        private void dgvTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!lbGeneralIntel.Visible || e.ColumnIndex != 1) return;
            string fio = dgvTable[1, e.RowIndex].Value.ToString();
            PrepareDetailInfoTable(fio);
            if (!(form2 is null)) form2.Close();
            form2 = new Form2(fio, detailInfoTable);
            form2.Show();
        }

        private void cbHalfYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbHours.Text = "";
            lbHours.Text = "Часы (доступно: " + (GetHoursPlan() - GetCompleteHours()) + ")";
        }

        private void cbHalfYear2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTableReportsWithArguments();
        }
    }
}
