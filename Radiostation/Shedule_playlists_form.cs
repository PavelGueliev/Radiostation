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
    public partial class Shedule_playlists_form : Form
    {
        private string connectionString;
        private bool userEditingSchedule = false;

        public Shedule_playlists_form()
        {
            InitializeComponent();
            connectionString = "Data Source=DESKTOP-6MQUQFM\\SQLEXPRESS;Initial Catalog=Radiostation;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True";

            this.Load += Shedule_playlists_form_Load;

        }

        private void Shedule_playlists_form_Load(object sender, EventArgs e)
        {
            LoadPresenters();
            LoadPlaylists();
            LoadSchedule();
        }

        private void LoadPresenters()
        {
            var presenters = new List<KeyValuePair<int, string>>();
            presenters.Add(new KeyValuePair<int, string>(-1, ""));

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT КодВедущего, ФИО FROM Presenter ORDER BY ФИО";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        presenters.Add(new KeyValuePair<int, string>(
                            reader.GetInt32(reader.GetOrdinal("КодВедущего")),
                            reader.GetString(reader.GetOrdinal("ФИО"))
                        ));
                    }
                }
            }
            
            presenterComboBox.DataSource = presenters;
            presenterComboBox.DisplayMember = "Value";
            presenterComboBox.ValueMember = "Key";
            presenterComboBox.SelectedIndex = presenterComboBox.FindStringExact(Program.CurrentUsername);
        }

        private void LoadPlaylists()
        {
            var playlists = new List<KeyValuePair<int, string>>();
            playlists.Add(new KeyValuePair<int, string>(-1, ""));

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT КодПлейлиста, Название FROM Playlist ORDER BY Название";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        playlists.Add(new KeyValuePair<int, string>(
                            reader.GetInt32(reader.GetOrdinal("КодПлейлиста")),
                            reader.GetString(reader.GetOrdinal("Название"))
                        ));
                    }
                }
            }

            playlistComboBox.DataSource = playlists;
            playlistComboBox.DisplayMember = "Value";
            playlistComboBox.ValueMember = "Key";
            playlistComboBox.SelectedIndex = -1;
        }

        private void LoadSchedule()
        {
            var scheduleList = new List<PlaylistSchedule>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT p.Продолжительность, s.КодЗаписи, s.КодВедущего, s.КодПлейлиста, s.ДатаВремя, v.ФИО, p.Название AS НазваниеПлейлиста " +
                               "FROM Shedule_playlists s " +
                               "JOIN Presenter v ON s.КодВедущего = v.КодВедущего " +
                               "JOIN Playlist p ON s.КодПлейлиста = p.КодПлейлиста " +
                               "WHERE 1=1";

                if (presenterComboBox.SelectedValue is int selectedPresenterId && selectedPresenterId > 0)
                {
                    query += " AND s.КодВедущего = @presenterId";
                }

                if (playlistComboBox.SelectedValue is int selectedPlaylistId && selectedPlaylistId > 0)
                {
                    query += " AND s.КодПлейлиста = @playlistId";
                }

                if (dateTimePicker.Checked)
                {
                    query += " AND CONVERT(date, s.ДатаВремя) = @date";
                }

                using (var command = new SqlCommand(query, connection))
                {
                    if (presenterComboBox.SelectedValue is int pId && pId > 0)
                        command.Parameters.AddWithValue("@presenterId", pId);

                    if (playlistComboBox.SelectedValue is int plId && plId > 0)
                        command.Parameters.AddWithValue("@playlistId", plId);

                    if (dateTimePicker.Checked)
                        command.Parameters.AddWithValue("@date", dateTimePicker.Value.Date);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            scheduleList.Add(new PlaylistSchedule
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("КодЗаписи")),
                                PresenterId = reader.GetInt32(reader.GetOrdinal("КодВедущего")),
                                PlaylistId = reader.GetInt32(reader.GetOrdinal("КодПлейлиста")),
                                DateTime = reader.GetDateTime(reader.GetOrdinal("ДатаВремя")),
                                DateTimeStop = reader.GetDateTime(reader.GetOrdinal("ДатаВремя"))
                      .Add(reader.GetTimeSpan(reader.GetOrdinal("Продолжительность"))),
                                PresenterName = reader.GetString(reader.GetOrdinal("ФИО")),
                                PlaylistName = reader.GetString(reader.GetOrdinal("НазваниеПлейлиста")),
                                Duration = (TimeSpan)reader["Продолжительность"]
                            });
                        }
                    }
                }
            }

            dataGridViewShedulePlaylist.DataSource = scheduleList;

            if (dataGridViewShedulePlaylist.Columns["Id"] != null)
                dataGridViewShedulePlaylist.Columns["Id"].Visible = false;
            if (dataGridViewShedulePlaylist.Columns["PresenterId"] != null)
                dataGridViewShedulePlaylist.Columns["PresenterId"].Visible = false;
            if (dataGridViewShedulePlaylist.Columns["PlaylistId"] != null)
                dataGridViewShedulePlaylist.Columns["PlaylistId"].Visible = false;

            dataGridViewShedulePlaylist.Columns["PresenterName"].HeaderText = "Ведущий";
            dataGridViewShedulePlaylist.Columns["PlaylistName"].HeaderText = "Плейлист";
            dataGridViewShedulePlaylist.Columns["DateTime"].HeaderText = "Дата и время начала";
            dataGridViewShedulePlaylist.Columns["DateTimeStop"].HeaderText = "Дата и время окончания";
            dataGridViewShedulePlaylist.Columns["Duration"].HeaderText = "Длительность";

            // Делаем некоторые поля только для чтения
            if (dataGridViewShedulePlaylist.Columns["PresenterName"] != null)
                dataGridViewShedulePlaylist.Columns["PresenterName"].ReadOnly = true;
            if (dataGridViewShedulePlaylist.Columns["DateTimeStop"] != null)
                dataGridViewShedulePlaylist.Columns["DateTimeStop"].ReadOnly = true;
            if (dataGridViewShedulePlaylist.Columns["PlaylistName"] != null)
                dataGridViewShedulePlaylist.Columns["PlaylistName"].ReadOnly = true;
            if (dataGridViewShedulePlaylist.Columns["Duration"] != null)
                dataGridViewShedulePlaylist.Columns["Duration"].ReadOnly = true;

            LoadPlaylistComposition();
        }

        private void LoadPlaylistComposition()
        {
            if (dataGridViewShedulePlaylist.CurrentRow == null || dataGridViewShedulePlaylist.CurrentRow.DataBoundItem == null)
            {
                dataGridViewComposition.DataSource = null;
                return;
            }

            var selectedSchedule = (PlaylistSchedule)dataGridViewShedulePlaylist.CurrentRow.DataBoundItem;

            var composition = new List<PlaylistComposition>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                SELECT cp.КодПлейлиста, cp.КодТрека, t.title, t.author, t.duration, g.title as genreTitle
                FROM Playlist_composition cp
                JOIN Track t ON cp.КодТрека = t.id
                JOIN Genre g ON t.genre = g.id
                WHERE cp.КодПлейлиста = @playlistId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@playlistId", selectedSchedule.PlaylistId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            composition.Add(new PlaylistComposition
                            {
                                PlaylistId = reader.GetInt32(reader.GetOrdinal("КодПлейлиста")),
                                TrackId = reader.GetInt32(reader.GetOrdinal("КодТрека")),
                                Title = reader.GetString(reader.GetOrdinal("title")),
                                Author = reader.GetString(reader.GetOrdinal("author")),
                                Genre = reader.GetString(reader.GetOrdinal("genreTitle")),
                                Duration = (TimeSpan)reader["duration"]
                            });
                        }
                    }
                }
            }

            dataGridViewComposition.DataSource = composition;

            if (dataGridViewComposition.Columns["PlaylistId"] != null)
                dataGridViewComposition.Columns["PlaylistId"].Visible = false;
            if (dataGridViewComposition.Columns["TrackId"] != null)
                dataGridViewComposition.Columns["TrackId"].Visible = false;

            dataGridViewComposition.Columns["Title"].HeaderText = "Название трека";
            dataGridViewComposition.Columns["Author"].HeaderText = "Автор";
            dataGridViewComposition.Columns["Duration"].HeaderText = "Длительность";
            dataGridViewComposition.Columns["Genre"].HeaderText = "Жанр";

            // Сделаем столбцы только для чтения
            if (dataGridViewComposition.Columns["Genre"] != null)
                dataGridViewComposition.Columns["Genre"].ReadOnly = true;
            if (dataGridViewComposition.Columns["Title"] != null)
                dataGridViewComposition.Columns["Title"].ReadOnly = true;
            if (dataGridViewComposition.Columns["Author"] != null)
                dataGridViewComposition.Columns["Author"].ReadOnly = true;
            if (dataGridViewComposition.Columns["Duration"] != null)
                dataGridViewComposition.Columns["Duration"].ReadOnly = true;
        }

        private void dataGridViewShedulePlaylist_SelectionChanged(object sender, EventArgs e)
        {
            LoadPlaylistComposition();
        }

        private void dataGridViewShedulePlaylist_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            userEditingSchedule = true;
        }

        private void dataGridViewShedulePlaylist_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (!userEditingSchedule) return;
            userEditingSchedule = false;

            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewShedulePlaylist.Rows.Count)
            {
                var row = dataGridViewShedulePlaylist.Rows[e.RowIndex];
                if (row.DataBoundItem is PlaylistSchedule editedSchedule)
                {
                    // Валидация данных
                    if (editedSchedule.PresenterId <= 0 || string.IsNullOrWhiteSpace(editedSchedule.PresenterName))
                    {
                        MessageBox.Show("Некорректный ведущий.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (editedSchedule.PlaylistId <= 0 || string.IsNullOrWhiteSpace(editedSchedule.PlaylistName))
                    {
                        MessageBox.Show("Некорректный плейлист.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (editedSchedule.DateTime == DateTime.MinValue)
                    {
                        MessageBox.Show("Некорректная дата/время.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string query = "UPDATE Shedule_playlists SET КодВедущего = @presenterId, КодПлейлиста = @playlistId, ДатаВремя = @dateTime WHERE КодЗаписи = @id";
                            using (var command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@presenterId", editedSchedule.PresenterId);
                                command.Parameters.AddWithValue("@playlistId", editedSchedule.PlaylistId);
                                command.Parameters.AddWithValue("@dateTime", editedSchedule.DateTime);
                                command.Parameters.AddWithValue("@id", editedSchedule.Id);

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void search_button_Click(object sender, EventArgs e)
        {
            try
            {
                LoadSchedule();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            try
            {

                if (!(playlistComboBox.SelectedValue is int plId) || plId <= 0)
                {
                    MessageBox.Show("Выберите плейлист.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!dateTimePicker.Checked)
                {
                    MessageBox.Show("Выберите дату и время.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime dt = dateTimePicker.Value;

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    int newId = GenerateScheduleId(connection);

                    string insertQuery = "INSERT INTO Shedule_playlists (КодЗаписи, КодВедущего, КодПлейлиста, ДатаВремя) VALUES (@id, (select КодВедущего from Presenter where ФИО = @presenterId), @playlistId, @dateTime)";
                    using (var command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", newId);
                        command.Parameters.AddWithValue("@presenterId", Program.CurrentUsername);
                        command.Parameters.AddWithValue("@playlistId", plId);
                        command.Parameters.AddWithValue("@dateTime", dt);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Запись добавлена в расписание.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSchedule();
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
                        userMessage = $"Ошибка (код {ex.Number}): {ex.Message}";
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
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            if (dataGridViewShedulePlaylist.CurrentRow == null || dataGridViewShedulePlaylist.CurrentRow.DataBoundItem == null)
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedSchedule = (PlaylistSchedule)dataGridViewShedulePlaylist.CurrentRow.DataBoundItem;

            var result = MessageBox.Show("Вы уверены, что хотите удалить выбранную запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM Shedule_playlists WHERE КодЗаписи = @id";
                    using (var command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", selectedSchedule.Id);
                        int rows = command.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Запись успешно удалена.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadSchedule();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось найти запись для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private int GenerateScheduleId(SqlConnection connection)
        {
            string maxQuery = "SELECT ISNULL(MAX(КодЗаписи), 0) FROM Shedule_playlists";
            using (SqlCommand cmd = new SqlCommand(maxQuery, connection))
            {
                int maxId = (int)cmd.ExecuteScalar();
                return maxId + 1;
            }
        }
    }

    public class PlaylistSchedule
    {
        public int Id { get; set; }
        public int PresenterId { get; set; }
        public int PlaylistId { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime DateTimeStop { get; set; }
        public TimeSpan Duration { get; set; }
        public string PresenterName { get; set; }
        public string PlaylistName { get; set; }
         // Длительность плейлиста
    }
}
