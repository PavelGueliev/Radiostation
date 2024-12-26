using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Radiostation
{
    public partial class Track_form : Form
    {
        private ServiceTrack serviceTrack;
        private string connectionString;
        private bool userEditing = false;
        public Track_form()
        {
            connectionString = "Data Source=DESKTOP-6MQUQFM\\SQLEXPRESS;Initial Catalog=Radiostation;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True";
            serviceTrack = new ServiceTrack(connectionString);
            InitializeComponent();
            ReFresh();
        }

        private void ReFresh()
        {
            LoadData();
            LoadGenres();
            LoadAuthors();
        }



        private void LoadData(List<Track> tracks = null)
        {
            if (tracks == null)
                tracks = serviceTrack.GetAllTracks();

            dataGridView1.DataSource = tracks;

            // Скрываем столбец с Id, если нужно
            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.Columns["Author"].HeaderText = "Автор";
            dataGridView1.Columns["Title"].HeaderText = "Название";
            dataGridView1.Columns["Genre"].HeaderText = "Жанр";
            dataGridView1.Columns["Duration"].HeaderText = "Длительность";
        }

        private void LoadGenres()
        {
            // Предположим, что жанры можно получить из базы или прописать статически
            // Например, жанры:
            string query = "SELECT id, title FROM Genre";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Создаем список для хранения данных
                    var genres = new List<KeyValuePair<int, string>>();
                    genres.Add(new KeyValuePair<int, string>(
                            -1, "" // Отображаемый текст (DisplayMember)
                        ));
                    while (reader.Read())
                    {
                        // Добавляем каждую строку в список
                        genres.Add(new KeyValuePair<int, string>(
                            reader.GetInt32(reader.GetOrdinal("id")),   // Значение (ValueMember)
                            reader.GetString(reader.GetOrdinal("title")) // Отображаемый текст (DisplayMember)
                        ));
                    }

                    // Привязываем список к ComboBox
                    Genre_comboBox.DataSource = genres;
                    Genre_comboBox.DisplayMember = "Value"; // Указываем свойство для отображения
                    Genre_comboBox.ValueMember = "Key";    // Указываем свойство для хранения значения
                    Genre_comboBox.SelectedIndex = -1;
                }

            }
        }

        private void LoadAuthors()
        {
            // Предположим, что авторов тоже можем получить из базы или справочника
            // Предположим, что жанры можно получить из базы или прописать статически
            // Например, жанры:
            string query = "SELECT DISTINCT author FROM Track";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Создаем список для хранения данных
                    List<string> authors = new List<string> { "" };

                    while (reader.Read())
                    {
                        // Добавляем каждую строку в список
                        authors.Add( // Значение (ValueMember)
                            reader.GetString(reader.GetOrdinal("author")) // Отображаемый текст (DisplayMember)
                        );
                    }

                    // Привязываем список к ComboBox
                    textComboBoxAuthor.DataSource = authors;
                    textComboBoxAuthor.DisplayMember = "Value"; // Указываем свойство для отображения
                    textComboBoxAuthor.SelectedIndex = -1;
                }

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Track_form_Load(object sender, EventArgs e)
        {

        }

        private void search_button_Click_1(object sender, EventArgs e)
        {
            // Выполняем поиск по введённым параметрам
            string title = string.IsNullOrWhiteSpace(textBoxTitle.Text) ? null : textBoxTitle.Text;
            string author = textComboBoxAuthor.SelectedIndex >= 0 ? textComboBoxAuthor.SelectedItem.ToString() : null;
            int? genre = Genre_comboBox.SelectedIndex >= 0 ? (int?)Genre_comboBox.SelectedIndex : null;

            // Если длительность тоже хотите учитывать в поиске, можно добавить, но здесь опустим.
            // Предполагаем, что search может быть только по названию, автору, жанру.


                var results = serviceTrack.SearchTracks(title, author, genre);
                LoadData(results);

        }

        private void delete_button_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Получаем id трека
                var selectedRow = dataGridView1.SelectedRows[0];
                if (selectedRow.Cells["Id"].Value is int id)
                {
                    try
                    {
                        serviceTrack.DeleteTrack(id);
                        MessageBox.Show("Трек успешно удалён!");
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при удалении трека: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите трек для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void exit_button_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void add_button_Click_1(object sender, EventArgs e)
        {
            // Проверим корректность данных
            if (string.IsNullOrWhiteSpace(textBoxTitle.Text))
            {
                MessageBox.Show("Введите название трека.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxTitle.Text))
            {
                MessageBox.Show("Выберите автора.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Genre_comboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Выберите жанр.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int hours = (int)Hours_numericUpDown.Value;
            int minutes = (int)Minut_numericUpDown.Value;
            int seconds = (int)Second_numericUpDown.Value;
            TimeSpan totalSeconds = new TimeSpan(hours, minutes, seconds);

            if (totalSeconds <= TimeSpan.Zero)
            {
                MessageBox.Show("Укажите длительность трека больше 0.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string query = "select id from Genre where title=@genre";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Выполняем первую команду


                // Выполняем вторую команду для чтения данных
                using (SqlCommand command = new SqlCommand(query, connection)) // queryForReader - это запрос для чтения данных
                {
                    command.Parameters.AddWithValue("@genre", Genre_comboBox.Text);
                    command.ExecuteNonQuery();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                    }
                }
            }

            var track = new Track
            {
                Title = textBoxTitle.Text,    // textBoxGenre фактически служит для названия
                Author = textComboBoxAuthor.Text,
                Genre = Genre_comboBox.Text,
                Duration = totalSeconds
            };

            try
            {
                serviceTrack.AddTrack(track);
                MessageBox.Show("Трек успешно добавлен!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении трека: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            userEditing = true;
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (userEditing)
            {
                userEditing = false; // Сбрасываем флаг
                var updatedRow = dataGridView1.Rows[e.RowIndex];
                var track = new Track
                {
                    Id = (int)updatedRow.Cells["id"].Value,
                    Title = (string)updatedRow.Cells["Title"].Value,    // textBoxGenre фактически служит для названия
                    Author = (string)updatedRow.Cells["Author"].Value,
                    Genre = (string)updatedRow.Cells["Genre"].Value,
                    Duration = (TimeSpan)updatedRow.Cells["Duration"].Value
                };
                MessageBox.Show("Успешное изменение трека: " + (string)updatedRow.Cells["Title"].Value, "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                serviceTrack.UpdateTrack(track);
            }
        }
    }
}
