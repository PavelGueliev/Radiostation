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

namespace Radiostation
{
    public partial class Genre_form : Form
    {
        private string connectionString;
        private bool userEditing = false;
        public Genre_form()
        {
            connectionString = "Data Source=DESKTOP-6MQUQFM\\SQLEXPRESS;Initial Catalog=Radiostation;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True";
            InitializeComponent();
            ReFresh();
        }

        private void ReFresh()
        {
            LoadData();
            LoadGenres();
        }

        private void LoadData(List<Genre> genres = null)
        {
            if (genres == null)
            {
                genres = new List<Genre>();
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT Id, title FROM Genre";
                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            genres.Add(new Genre
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("title"))
                            });
                        }
                    }
                }
            }

            dataGridView1.DataSource = genres;
            if (dataGridView1.Columns["Id"] != null)
            {
                dataGridView1.Columns["Id"].Visible = false; // Скрываем столбец ID
            }
        }

        private void LoadGenres()
        {
            // Загрузка жанров в ComboBox
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id, title FROM Genre";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var genreList = new List<KeyValuePair<int, string>>();
                    genreList.Add(new KeyValuePair<int, string>(-1, "")); // Пустое значение для сброса
                    while (reader.Read())
                    {
                        genreList.Add(new KeyValuePair<int, string>(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetString(reader.GetOrdinal("title"))
                        ));
                    }

                    Genre_comboBox.DataSource = genreList;
                    Genre_comboBox.DisplayMember = "Value";
                    Genre_comboBox.ValueMember = "Key";
                    Genre_comboBox.SelectedIndex = -1;
                }
            }
        }

        private void search_button_Click(object sender, EventArgs e)
        {
            string searchText = Genre_comboBox.Text.Trim();
            var foundGenres = new List<Genre>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, title FROM Genre WHERE title LIKE @search";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@search", "%" + searchText + "%");
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            foundGenres.Add(new Genre
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("title"))
                            });
                        }
                    }
                }
            }

            LoadData(foundGenres);
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            string newTitle = Genre_comboBox.Text.Trim();
            if (string.IsNullOrEmpty(newTitle))
            {
                MessageBox.Show("Введите название жанра для добавления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверим, нет ли уже такого жанра
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Проверка существования жанра
                string checkQuery = "SELECT COUNT(*) FROM Genre WHERE title = @title";
                using (var checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@title", newTitle);
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Жанр с таким названием уже существует.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Добавление нового жанра
                string insertQuery = "INSERT INTO Genre (id, title) VALUES ((select MAX(id) from Genre) + 1, @title)";
                using (var insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@title", newTitle);
                    insertCommand.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Жанр успешно добавлен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ReFresh();
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = (int)dataGridView1.SelectedRows[0].Cells["Id"].Value;

                // Подтверждение удаления
                var result = MessageBox.Show("Вы уверены, что хотите удалить выбранный жанр?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM Genre WHERE Id = @id";
                    using (var command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("Не удалось найти жанр для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                MessageBox.Show("Жанр успешно удалён.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReFresh();
            }
            else
            {
                MessageBox.Show("Выберите жанр для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            userEditing = true;
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (!userEditing) return;

            // При редактировании в DataGridView можно реализовать обновление:
            // Допустим, мы разрешили редактировать title жанра.
            // После выхода из редактирования, сохраняем изменения.
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is Genre editedGenre)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE Genre SET title = @title WHERE Id = @id";
                    using (var command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@title", editedGenre.Title);
                        command.Parameters.AddWithValue("@id", editedGenre.Id);
                        command.ExecuteNonQuery();
                    }
                }
            }

            userEditing = false;
        }
    }

    public class Genre
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
