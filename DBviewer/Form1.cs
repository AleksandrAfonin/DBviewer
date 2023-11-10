using System;
using System.Linq;

//это комменатрий от меня
namespace DBviewer
{
    public partial class Form1 : Form
    {
        const string dataBase = "27231685c0687995.db";
        SQLiteConnection connection;
        SQLiteCommand CMD;
        SQLiteDataAdapter adapter;
        DataTable generalTable = new DataTable();
        DataTable reportsTable = new DataTable();

        bool isInput = false;

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateTableReports();
            GeneralTable();
            FillCBTeacher();

            //Console.WriteLine("8537: " + GetStringPayment(8530));
        }

        // Генерация общей таблицы
        private void GeneralTable()
        {
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
                {
                    //CMD = new SQLiteCommand("SELECT teach.Id AS '№ п_п', teach.FIO AS Преподаватель" +
                    //                        " FROM Teachers AS teach ORDER BY teach.Id", connection);
                    //CMD = new SQLiteCommand("SELECT teach.Id AS '№ п_п', teach.FIO AS Преподаватель" +
                    //                        " FROM Teachers AS teach WHERE teach.FIO LIKE \"Почасов%\" ORDER BY teach.Id", connection);
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT teach.Id AS '№ п_п', teach.FIO AS Преподаватель" +
                                            " FROM Teachers AS teach ORDER BY teach.Id", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    adapter.Fill(generalTable);
                    generalTable.Columns.Add(new DataColumn("Лек план", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("Лек вып", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("Практ план", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("Практ вып", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("Лаб план", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("Лаб вып", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("ТекКонс план", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("ТекКонс вып", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("КонсЭкз план", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("КонсЭкз вып", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("ОргСРС план", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("ОргСРС вып", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("Рейтинг план", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("Рейтинг вып", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("Экзамен план", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("Экзамен вып", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("Зачет план", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("Зачет вып", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("ИтогАттест план", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("ИтогАттест вып", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("Всего план", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("Всего вып", Type.GetType("System.Int32")));
                    generalTable.Columns.Add(new DataColumn("Объем нагр", Type.GetType("System.Int32")));
                    FillGeneralTable();
                    dgvTable.DataSource = generalTable;
                }
            }
            catch (SQLiteException es)
            {
                MessageBox.Show("Не удалось подключиться к БД" + es.Message);
                Application.Exit();
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
            foreach (DataRow row in generalTable.Rows)
            {
                CMD = new SQLiteCommand("SELECT SUM(LectureHours) FROM Disciplines WHERE Disciplines.IdLector = (SELECT Id FROM Teachers WHERE FIO = '" +
                                        row["Преподаватель"].ToString() + "')", connection);
                row["Лек план"] = CMD.ExecuteScalar();

                CMD = new SQLiteCommand("SELECT SUM(LectureHours) FROM Reports WHERE IaGakGek LIKE '%" + GetTeacherHours(row["Преподаватель"].ToString()) + "%'", connection);
                row["Лек вып"] = CMD.ExecuteScalar();

                CMD = new SQLiteCommand("SELECT SUM(PracticeHours) FROM Disciplines WHERE Disciplines.IdPractice = (SELECT Id FROM Teachers WHERE FIO = '" +
                                        row["Преподаватель"].ToString() + "')", connection);
                row["Практ план"] = CMD.ExecuteScalar();

                CMD = new SQLiteCommand("SELECT SUM(PracticeHours) FROM Reports WHERE IaGakGek LIKE '%" + GetTeacherHours(row["Преподаватель"].ToString()) + "%'", connection);
                row["Практ вып"] = CMD.ExecuteScalar();

                CMD = new SQLiteCommand("SELECT SUM(LabHours) FROM Disciplines WHERE Disciplines.IdLab1 = (SELECT Id FROM Teachers WHERE FIO = '" +
                                        row["Преподаватель"].ToString() + "')", connection);
                row["Лаб план"] = CMD.ExecuteScalar();

                CMD = new SQLiteCommand("SELECT SUM(LabHours) FROM Reports WHERE IaGakGek LIKE '%" + GetTeacherHours(row["Преподаватель"].ToString()) + "%'", connection);
                row["Лаб вып"] = CMD.ExecuteScalar();

                CMD = new SQLiteCommand("SELECT SUM(ExamHours) FROM Disciplines WHERE Disciplines.IdExam = (SELECT Id FROM Teachers WHERE FIO = '" +
                                        row["Преподаватель"].ToString() + "')", connection);
                row["Экзамен план"] = CMD.ExecuteScalar();

                CMD = new SQLiteCommand("SELECT SUM(ExamHours) FROM Reports WHERE IaGakGek LIKE '%" + GetTeacherHours(row["Преподаватель"].ToString()) + "%'", connection);
                row["Экзамен вып"] = CMD.ExecuteScalar();

                CMD = new SQLiteCommand("SELECT SUM(FinAttestHours) FROM Disciplines WHERE Disciplines.IdFinAttest = (SELECT Id FROM Teachers WHERE FIO = '" +
                                        row["Преподаватель"].ToString() + "')", connection);
                row["ИтогАттест план"] = CMD.ExecuteScalar();

                CMD = new SQLiteCommand("SELECT SUM(FinAttestHours) FROM Reports WHERE IaGakGek LIKE '%" + GetTeacherHours(row["Преподаватель"].ToString()) + "%'", connection);
                row["ИтогАттест вып"] = CMD.ExecuteScalar();

                row["Всего план"] = GetIntValue(row["Лек план"]) + GetIntValue(row["Практ план"]) + GetIntValue(row["Лаб план"]) +
                                    GetIntValue(row["Экзамен план"]) + GetIntValue(row["ИтогАттест план"]);
                row["Всего вып"] = GetIntValue(row["Лек вып"]) + GetIntValue(row["Практ вып"]) + GetIntValue(row["Лаб вып"]) +
                                    GetIntValue(row["Экзамен вып"]) + GetIntValue(row["ИтогАттест вып"]);
            }
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
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Reports (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                            " DisName TEXT(100), GroupName TEXT(20), LectureHours INTEGER, PracticeHours INTEGER," +
                            " LabHours INTEGER, CurConsultHours INTEGER, ExamConsultHours INTEGER, IWSHours INTEGER," +
                            " RatingHours INTEGER, ExamHours INTEGER, CreditHours INTEGER, FinAttestHours INTEGER," +
                            " TotalHours INTEGER, Date TEXT(10), IaGakGek TEXT(100), PaymentMark TEXT(20))", connection);
                    CMD.ExecuteNonQuery();
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось подключиться к БД");
                Application.Exit();
            }
        }

        // Загрузка таблицы Reports
        private void LoadTableReports()
        {
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT Id, DisName AS Дисциплина, GroupName AS Группа, LectureHours AS Лекции, PracticeHours AS Практика," +
                        " LabHours AS Лабораторные, CurConsultHours AS 'Текущие консутьтации', ExamConsultHours AS 'Экзаменационные консультации', IWSHours," +
                        " RatingHours AS Рейтинг, ExamHours AS Экзамен, CreditHours AS Зачеты, FinAttestHours AS 'Итоговая аттестация'," +
                        " TotalHours AS Всего, Date AS Дата, IaGakGek AS 'ИА ГАК ГЭК', PaymentMark AS Оплата FROM Reports", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    reportsTable.Clear();
                    adapter.Fill(reportsTable);
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Load Table Reports");
                //Application.Exit();
            }
        }

        // Загрузка таблицы Reports с учетом месяца, года, формы обучения, преподавателя
        private void LoadTableReportsWithArguments()
        {
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT Id, DisName AS Дисциплина, GroupName AS Группа, LectureHours AS Лекции, PracticeHours AS Практика," +
                        " LabHours AS Лабораторные, CurConsultHours AS 'Текущие консутьтации', ExamConsultHours AS 'Экзаменационные консультации', IWSHours," +
                        " RatingHours AS Рейтинг, ExamHours AS Экзамен, CreditHours AS Зачеты, FinAttestHours AS 'Итоговая аттестация'," +
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
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
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

        // Получить количество часов по плану для лекций
        private int GetLectureHours()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
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
                MessageBox.Show("Не удалось подключиться к БД");
                return 0;
                //Application.Exit();
            }
        }

        // Получить количесво часов по плану для практики
        private int GetPracticeHours()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
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
                MessageBox.Show("Не удалось подключиться к БД");
                return 0;
                //Application.Exit();
            }
        }

        // Получить количесво часов по плану для лабораторных
        private int GetLabHours()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT LabHours" +
                                                    " FROM Disciplines WHERE IdLab1 = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "') AND DisName = '" +
                                                cbDiscipline.Text + "' AND GroupName = '" +
                                                cbStudiesGroup.Text + "'", connection);
                    return GetIntValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось подключиться к БД");
                return 0;
                //Application.Exit();
            }
        }

        // Получить количесво часов по плану для аттестации
        private int GetFinAttestHours()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
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
                MessageBox.Show("Не удалось подключиться к БД");
                return 0;
                //Application.Exit();
            }
        }

        // Получить количесво часов по плану для экзамена
        private int GetExamHours()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
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
                MessageBox.Show("Не удалось подключиться к БД");
                return 0;
                //Application.Exit();
            }
        }

        // Получить количество учтенных часов для лекций
        private int GetLectureHoursComplete()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
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
                //Application.Exit();
            }
        }

        // Получить количество учтенных часов для практики
        private int GetPracticeHoursComplete()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
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
                //Application.Exit();
            }
        }

        // Получить количество учтенных часов для лабораторных
        private int GetLabHoursComplete()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
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
                //Application.Exit();
            }
        }

        // Получить количество учтенных часов для итоговой аттестации
        private int GetFinAttestHoursComplete()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
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
                //Application.Exit();
            }
        }

        // Получить количество учтенных часов для экзамена
        private int GetExamHoursComplete()
        {
            if (cbTeacher.SelectedIndex == -1 || cbDiscipline.SelectedIndex == -1 || cbStudiesGroup.SelectedIndex == -1) return 0;
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
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
                //Application.Exit();
            }
        }

        // Получить количество часов по плану
        private int GetHoursPlan()
        {
            switch (cbTypeOfActivity.Text)
            {
                case "Лекции": return GetLectureHours();
                case "Практика": return GetPracticeHours();
                case "Лабораторные": return GetLabHours();
                case "Итог аттестация": return GetFinAttestHours();
                case "Экзамен": return GetExamHours();
                default: return 0;
            }
        }

        // Получить количество проведенных часов
        private int GetCompleteHours()
        {
            switch (cbTypeOfActivity.Text)
            {
                case "Лекции": return GetLectureHoursComplete();
                case "Практика": return GetPracticeHoursComplete();
                case "Лабораторные": return GetLabHoursComplete();
                case "Итог аттестация": return GetFinAttestHoursComplete();
                case "Экзамен": return GetExamHoursComplete();
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
                case "Практика": return "PracticeHours";
                case "Лабораторные": return "LabHours";
                case "Итог аттестация": return "FinAttestHours";
                case "Экзамен": return "ExamHours";
                default: return "";
            }
        }

        // Получение строкового значения с ведущим 0
        private string GetStringWith0(int value)
        {
            string str = value.ToString();
            if (str.Length < 2) str = "0" + str;
            return str;
        }

        // Заполнение элементов Combo Box Discipline
        private void SetDiscCBDiscipline()
        {
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT DISTINCT DisName" +
                                            " FROM Disciplines WHERE IdLector = (SELECT Id FROM Teachers WHERE FIO = '" +
                                        cbTeacher.Text + "') OR IdPractice = (SELECT Id FROM Teachers WHERE FIO = '" +
                                        cbTeacher.Text + "') OR IdLab1 = (SELECT Id FROM Teachers WHERE FIO = '" +
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
                MessageBox.Show("Не удалось подключиться к БД");
                Application.Exit();
            }
        }

        // Заполнение элементов Combo Box StudiesGroup
        private void SetGroupCBStudiesGroup()
        {
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT DISTINCT GroupName" +
                                                    " FROM Disciplines WHERE (IdLector = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "') OR IdPractice = (SELECT Id FROM Teachers WHERE FIO = '" +
                                                cbTeacher.Text + "') OR IdLab1 = (SELECT Id FROM Teachers WHERE FIO = '" +
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
                MessageBox.Show("Не удалось подключиться к БД");
                Application.Exit();
            }
        }

        // Заполнение элементов ComboBox TypeOfActivity
        private void SetActivityCBTypeOfActivity()
        {
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
                {
                    cbTypeOfActivity.Items.Clear();
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT DisName, GroupName" +
                                            " FROM Disciplines WHERE DisName = '" + cbDiscipline.Text + "' AND GroupName = '" +
                                            cbStudiesGroup.Text + "' AND IdLector = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "')", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count != 0) cbTypeOfActivity.Items.Add("Лекции");

                    CMD = new SQLiteCommand("SELECT DisName, GroupName" +
                                            " FROM Disciplines WHERE DisName = '" + cbDiscipline.Text + "' AND GroupName = '" +
                                            cbStudiesGroup.Text + "' AND IdPractice = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "')", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count != 0) cbTypeOfActivity.Items.Add("Практика");

                    CMD = new SQLiteCommand("SELECT DisName, GroupName" +
                                            " FROM Disciplines WHERE DisName = '" + cbDiscipline.Text + "' AND GroupName = '" +
                                            cbStudiesGroup.Text + "' AND IdLab1 = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "')", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count != 0) cbTypeOfActivity.Items.Add("Лабораторные");

                    CMD = new SQLiteCommand("SELECT DisName, GroupName" +
                                            " FROM Disciplines WHERE DisName = '" + cbDiscipline.Text + "' AND GroupName = '" +
                                            cbStudiesGroup.Text + "' AND IdFinAttest = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "')", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count != 0) cbTypeOfActivity.Items.Add("Итог аттестация");

                    CMD = new SQLiteCommand("SELECT DisName, GroupName" +
                                            " FROM Disciplines WHERE DisName = '" + cbDiscipline.Text + "' AND GroupName = '" +
                                            cbStudiesGroup.Text + "' AND IdExam = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "')", connection);
                    adapter = new SQLiteDataAdapter(CMD);
                    table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count != 0) cbTypeOfActivity.Items.Add("Экзамен");

                    cbTypeOfActivity.SelectedIndex = -1;
                    cbTypeOfActivity.Text = "";
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось подключиться к БД");
                Application.Exit();
            }
        }

        // Заполнение элементов ComboBox LevelEducation
        private void SetLevelCBLevelEducation()
        {
            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
                {
                    connection.Open();
                    CMD = new SQLiteCommand("SELECT LevelEducation" +
                                            " FROM Disciplines WHERE DisName = '" + cbDiscipline.Text + "' AND GroupName = '" +
                                            cbStudiesGroup.Text + "' AND (IdLector = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "') OR IdPractice = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "') OR IdLab1 = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "') OR IdFinAttest = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "') OR IdExam = (SELECT Id FROM Teachers WHERE FIO = '" +
                                            cbTeacher.Text + "'))", connection);
                    cbLevelEducation.Text = GetStringValue(CMD.ExecuteScalar());
                }
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Не удалось подключиться к БД");
                Application.Exit();
            }
        }

        // Переход на форму отчетов
        private void btnFillForm_Click(object sender, EventArgs e)
        {
            lbMonth.Visible = true; cbMonth.Visible = true;
            lbYear.Visible = true; cbYear.Visible = true;
            lbStadiesForm.Visible = true; cbStudiesForm.Visible = true;
            lbTeacher.Visible = true; cbTeacher.Visible = true;
            btnIntel.Visible = false; btnBackGeneral.Visible = true;
            btnInput.Visible = true; btnPaid.Visible = true;

            cbMonth.SelectedIndex = DateTime.Today.Month - 1;
            cbYear.SelectedItem = DateTime.Today.Year.ToString();
            cbStudiesForm.SelectedIndex = 0;

            LoadTableReports();
            dgvTable.DataSource = reportsTable;
        }

        // Возврат на общую форму из формы отчетов
        private void btnBackGeneral_Click(object sender, EventArgs e)
        {
            //isInput = false;

            lbMonth.Visible = false; cbMonth.Visible = false;
            lbYear.Visible = false; cbYear.Visible = false;
            lbStadiesForm.Visible = false; cbStudiesForm.Visible = false;
            lbTeacher.Visible = false; cbTeacher.Visible = false;
            btnIntel.Visible = true; btnBackGeneral.Visible = false;
            btnInput.Visible = false; btnPaid.Visible = false;

            cbTeacher.SelectedIndex = -1;

            dgvTable.DataSource = generalTable;
        }

        // Пометка оплаты 
        private void btnPaid_Click(object sender, EventArgs e)
        {
            if (reportsTable.Rows.Count == 0)
            {
                MessageBox.Show("Нечего оплачивать.");
                return;
            }

            try
            {
                using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
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
                //Application.Exit();
            }
        }

        // Переход на форму ввода проведенных часов
        private void btnInput_Click(object sender, EventArgs e)
        {
            btnBackReports.Visible = true; btnBackGeneral.Visible = false; btnInput.Visible = false;
            cbStudiesForm.Visible = false; lbStadiesForm.Visible = false; btnPaid.Visible = false;
            btnEnter.Visible = true;
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
            btnEnter.Visible = false;
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
                    using (connection = new SQLiteConnection("Data Source=" + dataBase + "; Version=3; FailIfMissing=True"))
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
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("Вылет в INSERTe: " + ex.ToString());
                    //Application.Exit();
                }
            }
        }

        // Обработка изменения месяца
        private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            DaysInMonth();
            cbHalfYear.SelectedIndex = cbMonth.SelectedIndex > 6 && cbMonth.SelectedIndex < 12 ? 0 : 1;
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
            SetActivityCBTypeOfActivity();
            SetLevelCBLevelEducation();
        }

        private void cbTypeOfActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbHours.Text = "";
            //tbHours.Text = GetLectureHours().ToString();
            //tbHours.Text = GetPracticeHours().ToString();
            //tbHours.Text = GetLabHours().ToString();
            //tbHours.Text = GetFinAttestHours().ToString();
            //tbHours.Text = GetExamHours().ToString();
            Console.WriteLine("По плану: " + GetHoursPlan() + "  Выполнено: " + GetCompleteHours());
            lbHours.Text = "Часы(доступно: " + (GetHoursPlan() - GetCompleteHours()) + ")";
        }

        private void cbStudiesForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isInput)
            {
                LoadTableReportsWithArguments();
            }
        }
    }
}
