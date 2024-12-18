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
    public partial class Ad_record_form : Form
    {
        private string connectionString;
        private bool userEditing = false;
        public Ad_record_form()
        {
            InitializeComponent();
            connectionString = "Data Source=DESKTOP-6MQUQFM\\SQLEXPRESS;Initial Catalog=Radiostation;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True";

            // Привязка событий к кнопкам
            ReFresh();
        }


        private void ReFresh()
        {
            LoadData();
        }

        private void LoadData(List<AdRecord> records = null)
        {
            if (records == null)
            {
                records = new List<AdRecord>();
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT КодРолика, НазваниеРолика, Продолжительность FROM Ad_record";
                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var record = new AdRecord
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("кодРолика")),
                                Title = reader.GetString(reader.GetOrdinal("НазваниеРолика")),
                                Duration = (TimeSpan)reader["Продолжительность"]
                            };
                            records.Add(record);
                        }
                    }
                }
            }

            dataGridView1.DataSource = records;
            if (dataGridView1.Columns["Id"] != null)
            {
                dataGridView1.Columns["Id"].Visible = false;
            }

            dataGridView1.Columns["Title"].HeaderText = "Название ролика";
            dataGridView1.Columns["Duration"].HeaderText = "Продолжительность";
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            string title = textBoxTitle.Text.Trim();
            int hours = (int)Hours_numericUpDown.Value;
            int minutes = (int)Minut_numericUpDown.Value;
            int seconds = (int)Second_numericUpDown.Value;

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Введите название ролика.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TimeSpan duration = new TimeSpan(hours, minutes, seconds);
            if (duration <= TimeSpan.Zero)
            {
                MessageBox.Show("Длительность должна быть больше 0.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Ad_record (КодРолика, НазваниеРолика, Продолжительность) VALUES ((select Max(КодРолика) from Ad_record) + 1, @title, @duration)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@title", title);
                    command.Parameters.AddWithValue("@duration", duration);
                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Ролик успешно добавлен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ReFresh();
        }

        private void search_button_Click(object sender, EventArgs e)
        {
            string searchTitle = textBoxTitle.Text.Trim();
            var records = new List<AdRecord>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT кодРолика, НазваниеРолика, Продолжительность FROM Ad_record";
                if (!string.IsNullOrEmpty(searchTitle))
                {
                    query += " WHERE НазваниеРолика LIKE @search";
                }

                using (var command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(searchTitle))
                        command.Parameters.AddWithValue("@search", "%" + searchTitle + "%");

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            records.Add(new AdRecord
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("кодРолика")),
                                Title = reader.GetString(reader.GetOrdinal("НазваниеРолика")),
                                Duration = (TimeSpan)reader["Продолжительность"]
                            });
                        }
                    }
                }
            }

            LoadData(records);
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = (int)dataGridView1.SelectedRows[0].Cells["Id"].Value;
                var result = MessageBox.Show("Вы уверены, что хотите удалить выбранный ролик?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Ad_record WHERE кодРолика = @id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("Не удалось найти указанный ролик для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                MessageBox.Show("Ролик успешно удалён.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReFresh();
            }
            else
            {
                MessageBox.Show("Выберите ролик для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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



        private void label1_Click(object sender, EventArgs e)
        {
            // Можно игнорировать или убрать
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            // Если пользователь не редактировал - выходим
            if (!userEditing) return;
            userEditing = false;
            // Если строка валидна и привязана к объекту
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                if (row.DataBoundItem is AdRecord editedRecord)
                {
                    // Валидация данных при сохранении
                    if (string.IsNullOrWhiteSpace(editedRecord.Title))
                    {
                        MessageBox.Show("Название ролика не может быть пустым.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Можно вернуть старое значение или не обновлять
                        return;
                    }

                    if (editedRecord.Duration <= TimeSpan.Zero)
                    {
                        MessageBox.Show("Длительность должна быть больше 0.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Обновляем данные в базе
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "UPDATE Ad_record SET НазваниеРолика = @title, Продолжительность = @duration WHERE кодРолика = @id";
                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@title", editedRecord.Title);
                            command.Parameters.AddWithValue("@duration", editedRecord.Duration);
                            command.Parameters.AddWithValue("@id", editedRecord.Id);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                MessageBox.Show("Не удалось обновить запись. Возможно, она была удалена.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }

            
        }
    }

    public class AdRecord
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
    }

}
