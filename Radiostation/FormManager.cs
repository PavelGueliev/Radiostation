using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Radiostation
{
    public partial class FormManager : Form
    {
        private readonly PresenterRepository presenterRepository;
        private bool userEditing = false;

        public FormManager()
        {
            InitializeComponent();
            string connectionString = "Data Source=DESKTOP-6MQUQFM\\SQLEXPRESS;Initial Catalog=Radiostation;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True";
            presenterRepository = new PresenterRepository(connectionString);
        }

        private void FormManager_Load(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void RefreshDataGridView()
        {
            var presenters = presenterRepository.SearchPresenters(null, null, null);
            dataGridView1.DataSource = presenters;

            // Скрываем первичный ключ
            dataGridView1.Columns["КодВедущего"].Visible = false;

            // Переименовываем столбцы
            dataGridView1.Columns["ФИО"].HeaderText = "ФИО";
            dataGridView1.Columns["НомерТелефона"].HeaderText = "Номер телефона";
            dataGridView1.Columns["ДатаРождения"].HeaderText = "Дата рождения";
            dataGridView1.Columns["Пароль"].HeaderText = "Пароль";

            // Разрешаем редактировать все поля, чтобы пользователь мог менять пароль
            dataGridView1.ReadOnly = false;
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            // Валидация
            if (string.IsNullOrEmpty(NameCombobox.Text))
            {
                MessageBox.Show("Заполните поле ФИО", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(Number.Text))
            {
                MessageBox.Show("Заполните поле Номер телефона", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!dateTimePicker.Checked)
            {
                MessageBox.Show("Заполните поле Дата рождения", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Получаем пароль из textBox
            string password = passwordTextBox.Text;

            var presenter = new Presenter
            {
                ФИО = NameCombobox.Text,
                НомерТелефона = Number.Text,
                ДатаРождения = dateTimePicker.Value,
                Пароль = password
            };

            try
            {
                presenterRepository.AddPresenter(presenter);
                MessageBox.Show("Запись успешно добавлена.");
                RefreshDataGridView();
            }
            catch (SqlException ex)
            {
                string userMessage;

                switch (ex.Number)
                {
                    case 547:
                        // Ошибка нарушения целостности при связи (FK) или ограничения CHECK
                        // "The INSERT statement conflicted with the FOREIGN KEY constraint" и т.п.
                        userMessage = "Ведущий должен быть старше 18";
                        break;

                    case 2627:
                        // Нарушение UNIQUE или PRIMARY KEY
                        // "Violation of PRIMARY KEY constraint" / "Violation of UNIQUE KEY constraint"
                        userMessage = "Невозможно сохранить запись. Такую запись уже добавляли.";
                        break;

                    case 2601:
                        // Аналогично 2627, нарушение уникального индекса
                        userMessage = "Дубликат. Запись с такими уникальными полями уже существует.";
                        break;


                    case 50000:
                        // Пользовательская ошибка, сгенерированная через RAISERROR(...) с number=50000
                        userMessage = "Ошибка : " + ex.Message;
                        break;

                    default:
                        // Все остальные ошибки. Можно вывести ex.Number и ex.Message полностью.
                        userMessage = $"Ошибка  (код {ex.Number}): {ex.Message}";
                        break;
                }

                // Выводим конечное сообщение пользователю
                MessageBox.Show(userMessage,
                                "Ошибка SQL",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении записи: " + ex.Message);
            }
        }

        private void search_button_Click(object sender, EventArgs e)
        {
            string name = NameCombobox.Text;
            string number = Number.Text;
            DateTime? birthDate = dateTimePicker.Checked ? dateTimePicker.Value : (DateTime?)null;

            var presenters = presenterRepository.SearchPresenters(name, number, birthDate);
            dataGridView1.DataSource = presenters;
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = (int)dataGridView1.SelectedRows[0].Cells["КодВедущего"].Value;
                try
                {
                    presenterRepository.DeletePresenter(id);
                    MessageBox.Show("Запись успешно удалена.");
                    RefreshDataGridView();
                }
                catch (SqlException ex)
                {
                    string userMessage;

                    switch (ex.Number)
                    {
                        case 547:
                            // Ошибка нарушения целостности при связи (FK) или ограничения CHECK
                            // "The INSERT statement conflicted with the FOREIGN KEY constraint" и т.п.
                            userMessage = "Ошибка нарушения целостности при связи или ограничения.";
                            break;

                        case 2627:
                            // Нарушение UNIQUE или PRIMARY KEY
                            // "Violation of PRIMARY KEY constraint" / "Violation of UNIQUE KEY constraint"
                            userMessage = "Невозможно сохранить запись. Такую запись уже добавляли.";
                            break;

                        case 2601:
                            // Аналогично 2627, нарушение уникального индекса
                            userMessage = "Дубликат. Запись с такими уникальными полями уже существует.";
                            break;


                        case 50000:
                            // Пользовательская ошибка, сгенерированная через RAISERROR(...) с number=50000
                            userMessage = "Ошибка базы данных: " + ex.Message;
                            break;

                        default:
                            // Все остальные ошибки. Можно вывести ex.Number и ex.Message полностью.
                            userMessage = $"Ошибка SQL (код {ex.Number}): {ex.Message}";
                            break;
                    }

                    // Выводим конечное сообщение пользователю
                    MessageBox.Show(userMessage,
                                    "Ошибка",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.");
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            userEditing = true;
        }

        private void dataGridView1_RowValidated_1(object sender, DataGridViewCellEventArgs e)
        {
            if (!userEditing) return;
            userEditing = false;

            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                var updatedRow = dataGridView1.Rows[e.RowIndex];

                int id = (int)updatedRow.Cells["КодВедущего"].Value;
                string name = (string)updatedRow.Cells["ФИО"].Value;
                string number = (string)updatedRow.Cells["НомерТелефона"].Value;
                DateTime birthDate = (DateTime)updatedRow.Cells["ДатаРождения"].Value;
                string password = (string)updatedRow.Cells["Пароль"].Value;

                try
                {
                    presenterRepository.UpdatePresenter(id, name, number, birthDate, password);
                }
                catch (SqlException ex)
                {
                    string userMessage;

                    switch (ex.Number)
                    {
                        case 547:
                            // Ошибка нарушения целостности при связи (FK) или ограничения CHECK
                            // "The INSERT statement conflicted with the FOREIGN KEY constraint" и т.п.
                            userMessage = "Ведущий должен быть старше 18";
                            break;

                        case 2627:
                            // Нарушение UNIQUE или PRIMARY KEY
                            // "Violation of PRIMARY KEY constraint" / "Violation of UNIQUE KEY constraint"
                            userMessage = "Невозможно сохранить запись. Такую запись уже добавляли.";
                            break;

                        case 2601:
                            // Аналогично 2627, нарушение уникального индекса
                            userMessage = "Дубликат. Запись с такими уникальными полями уже существует.";
                            break;


                        case 50000:
                            // Пользовательская ошибка, сгенерированная через RAISERROR(...) с number=50000
                            userMessage = "Ошибка базы данных: " + ex.Message;
                            break;

                        default:
                            // Все остальные ошибки. Можно вывести ex.Number и ex.Message полностью.
                            userMessage = $"Ошибка SQL (код {ex.Number}): {ex.Message}";
                            break;
                    }

                    // Выводим конечное сообщение пользователю
                    MessageBox.Show(userMessage,
                                    "Ошибка SQL",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении: " + ex.Message);
                }
            }
        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Можно удалить если не нужно
        }

        private void FormManager_TextChanged(object sender, EventArgs e)
        {
            // Можно удалить если не нужно
        }
    }
}
