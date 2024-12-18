using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void exit_button_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public FormManager()
        {
            InitializeComponent();
            string connectionString = "Data Source=DESKTOP-6MQUQFM\\SQLEXPRESS;Initial Catalog=Radiostation;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True"; // Укажите строку подключения
            presenterRepository = new PresenterRepository(connectionString);
            RefreshDataGridView();
        }

        private void add_button_Click(object sender, EventArgs e)
        {
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

            var presenter = new Presenter
            {
                ФИО = NameCombobox.Text,
                НомерТелефона = Number.Text,
                ДатаРождения = dateTimePicker.Value
            };

            try
            {
                presenterRepository.AddPresenter(presenter);
                MessageBox.Show("Запись успешно добавлена.");
                RefreshDataGridView(); // Обновляем DataGridView
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
                int id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

                try
                {
                    presenterRepository.DeletePresenter(id);
                    MessageBox.Show("Запись успешно удалена.");
                    RefreshDataGridView(); // Обновляем DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении записи: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.");
            }
        }

        private void RefreshDataGridView()
        {
            var presenters = presenterRepository.SearchPresenters(null, null, null);
            dataGridView1.DataSource = presenters;
            dataGridView1.Columns["КодВедущего"].Visible = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FormManager_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowValidated_1(object sender, DataGridViewCellEventArgs e)
        {
            // Получение строки
            if (userEditing)
            {
                userEditing = false; // Сбрасываем флаг
                var updatedRow = dataGridView1.Rows[e.RowIndex];
                int id = (int)updatedRow.Cells["КодВедущего"].Value;
                string name = (string)updatedRow.Cells["ФИО"].Value;
                string number = (string)updatedRow.Cells["НомерТелефона"].Value;
                DateTime? birthDate = (DateTime)updatedRow.Cells["ДатаРождения"].Value;
                presenterRepository.UpdatePresenter(id, name, number, birthDate);
            }

        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            userEditing = true;
        }

        private void FormManager_Load(object sender, EventArgs e)
        {

        }
    }
}
