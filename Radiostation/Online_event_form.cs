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
    public partial class Online_event_form : Form
    {
        private string connectionString;
        private bool userEditing = false;

        public Online_event_form()
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

        // Метод для загрузки данных в ComboBox и DataGridView
        private void LoadData(List<OnlineEvent> events = null)
        {
            if (events == null)
            {
                events = new List<OnlineEvent>();
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Загрузка авторов
                    var authors = new List<string>();
                    string queryAuthors = "SELECT DISTINCT author FROM Track";
                    using (var cmd = new SqlCommand(queryAuthors, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            authors.Add(reader["author"].ToString());
                        }
                    }
                    authorComboBox.Items.Clear();
                    foreach (var author in authors)
                    {
                        authorComboBox.Items.Add(author);
                    }

                    // Загрузка жанров
                    var genres = new List<ComboBoxItem>();
                    string queryGenres = "SELECT id, title FROM Genre";
                    using (var cmd = new SqlCommand(queryGenres, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            genres.Add(new ComboBoxItem(reader["title"].ToString(), reader["id"]));
                        }
                    }
                    genreComboBox.Items.Clear();
                    foreach (var genre in genres)
                    {
                        genreComboBox.Items.Add(genre);
                    }

                    // Загрузка названий треков
                    var titles = new List<string>();
                    string queryTitles = "SELECT title FROM Track";
                    using (var cmd = new SqlCommand(queryTitles, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            titles.Add(reader["title"].ToString());
                        }
                    }
                    titleComboBox.Items.Clear();
                    foreach (var title in titles)
                    {
                        titleComboBox.Items.Add(title);
                    }

                    // Загрузка событий в DataGridView
                    string queryEvents = @"
                        SELECT 
                            oe.КодСобытия,
                            t.title AS Трек,
                            t.author AS Автор,
                            g.title AS Жанр,
                            t.Duration AS Продолжительность,
                            oe.ДатаВремя AS [Дата воспроизведения]
                        FROM 
                            Online_event oe
                            JOIN Track t ON oe.КодТрека = t.id
                            JOIN Genre g ON t.genre = g.id
                        ORDER BY 
                            oe.ДатаВремя DESC";
                    using (var cmd = new SqlCommand(queryEvents, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events.Add(new OnlineEvent
                            {
                                EventId = reader.GetInt32(reader.GetOrdinal("КодСобытия")),
                                EventTime = reader.GetDateTime(reader.GetOrdinal("Дата воспроизведения")),
                                Duration = reader.GetTimeSpan(reader.GetOrdinal("Продолжительность")),
                                TrackTitle = reader["Трек"].ToString(),
                                Author = reader["Автор"].ToString(),
                                Genre = reader["Жанр"].ToString()
                            });
                        }
                    }
                }
            }

            dataGridViewOnlineEvents.DataSource = events;
            if (dataGridViewOnlineEvents.Columns["EventId"] != null)
            {
                dataGridViewOnlineEvents.Columns["EventId"].Visible = false;
            }

            dataGridViewOnlineEvents.Columns["EventTime"].HeaderText = "Дата воспроизведения";
            dataGridViewOnlineEvents.Columns["Author"].HeaderText = "Автор";
            dataGridViewOnlineEvents.Columns["TrackTitle"].HeaderText = "Трек";
            dataGridViewOnlineEvents.Columns["Genre"].HeaderText = "Жанр";
            dataGridViewOnlineEvents.Columns["Duration"].HeaderText = "Длительнотсь";

        }

        // Класс для хранения пары значение-текст в ComboBox
        public class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
            public ComboBoxItem(string text, object value)
            {
                Text = text;
                Value = value;
            }
            public override string ToString()
            {
                return Text;
            }
        }

        // Класс для представления события
        public class OnlineEvent
        {
            public int EventId { get; set; }
            public DateTime EventTime { get; set; }
            public TimeSpan Duration { get; set; }
            public string TrackTitle { get; set; }
            public string Author { get; set; }
            public string Genre { get; set; }
        }

        // Обработчик кнопки "Обновить базу событий"
        private void button1_Click(object sender, EventArgs e)
        {
            string updateQuery = @"
                WITH CTE_SortedTracks AS (
                    SELECT 
                        rp.КодПлейлиста,
                        rp.ДатаВремя AS StartTime,
                        t.id AS TrackID,
                        t.duration,
                        ROW_NUMBER() OVER (PARTITION BY rp.КодПлейлиста ORDER BY rp.ДатаВремя) AS RowNum
                    FROM 
                        Расписание_плейлистов rp
                        JOIN Плейлист p ON p.КодПлейлиста = rp.КодПлейлиста
                        JOIN Состав_Плейлиста sp ON sp.КодПлейлиста = p.КодПлейлиста
                        JOIN Track t ON sp.КодТрека = t.id
                )
                INSERT INTO Online_event (КодТрека, ДатаВремя)
                SELECT 
                    st.TrackID,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', st.duration) * (st.RowNum - 1), st.StartTime) AS EventTime
                FROM 
                    CTE_SortedTracks st;
            ";

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var cmd = new SqlCommand(updateQuery, connection);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    MessageBox.Show($"База данных событий успешно обновлена. Добавлено записей: {rowsAffected}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ReFresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении базы данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Обработчик кнопки "Найти"
        private void search_button_Click(object sender, EventArgs e)
        {
            string searchAuthor = authorComboBox.SelectedItem != null ? authorComboBox.SelectedItem.ToString() : "";
            string searchTitle = titleComboBox.SelectedItem != null ? titleComboBox.SelectedItem.ToString() : "";
            ComboBoxItem selectedGenre = genreComboBox.SelectedItem as ComboBoxItem;
            string searchGenre = selectedGenre != null ? selectedGenre.Value.ToString() : "";
            DateTime searchDate = dateTimePicker.Value.Date;

            List<OnlineEvent> events = new List<OnlineEvent>();

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            oe.КодСобытия,
                            t.title AS Трек,
                            t.author AS Автор,
                            g.Название AS Жанр,
                            oe.ДатаВремя AS [Дата воспроизведения]
                        FROM 
                            Online_event oe
                            JOIN Track t ON oe.КодТрека = t.id
                            JOIN Жанр g ON t.genre = g.КодЖанра
                        WHERE 
                            1=1";

                    if (!string.IsNullOrEmpty(searchAuthor))
                    {
                        query += " AND t.author = @Author";
                    }

                    if (!string.IsNullOrEmpty(searchTitle))
                    {
                        query += " AND t.title = @Title";
                    }

                    if (!string.IsNullOrEmpty(searchGenre))
                    {
                        query += " AND t.genre = @Genre";
                    }

                    if (dateTimePicker.Checked)
                    {
                        query += " AND CONVERT(date, oe.ДатаВремя) = @EventDate";
                    }

                    query += " ORDER BY oe.ДатаВремя DESC";

                    var cmd = new SqlCommand(query, connection);

                    if (!string.IsNullOrEmpty(searchAuthor))
                        cmd.Parameters.AddWithValue("@Author", searchAuthor);

                    if (!string.IsNullOrEmpty(searchTitle))
                        cmd.Parameters.AddWithValue("@Title", searchTitle);

                    if (!string.IsNullOrEmpty(searchGenre))
                        cmd.Parameters.AddWithValue("@Genre", searchGenre);

                    if (dateTimePicker.Checked)
                        cmd.Parameters.AddWithValue("@EventDate", searchDate);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events.Add(new OnlineEvent
                            {
                                EventId = reader.GetInt32(reader.GetOrdinal("КодСобытия")),
                                TrackTitle = reader["Трек"].ToString(),
                                Author = reader["Автор"].ToString(),
                                Genre = reader["Жанр"].ToString(),
                                EventTime = reader.GetDateTime(reader.GetOrdinal("Дата воспроизведения"))
                            });
                        }
                    }

                    LoadData(events);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при поиске: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Обработчик кнопки "Удалить"
        private void delete_button_Click(object sender, EventArgs e)
        {
            if (dataGridViewOnlineEvents.SelectedRows.Count > 0)
            {
                int eventId = Convert.ToInt32(dataGridViewOnlineEvents.SelectedRows[0].Cells["EventId"].Value);
                var result = MessageBox.Show("Вы уверены, что хотите удалить выбранное событие?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;

                using (var connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "DELETE FROM Online_event WHERE КодСобытия = @EventId";
                        var cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@EventId", eventId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Событие успешно удалено.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ReFresh();
                        }
                        else
                        {
                            MessageBox.Show("Событие не найдено или уже удалено.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении события: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите событие для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Обработчик кнопки "Выход"
        private void exit_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Обработчик начала редактирования ячейки в DataGridView
        private void dataGridViewOnlineEvents_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            userEditing = true;
        }

        // Обработчик завершения редактирования строки в DataGridView
        private void dataGridViewOnlineEvents_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (!userEditing) return;
            userEditing = false;

            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewOnlineEvents.Rows.Count)
            {
                var row = dataGridViewOnlineEvents.Rows[e.RowIndex];
                if (row.DataBoundItem is OnlineEvent editedEvent)
                {
                    // Валидация данных при сохранении
                    if (string.IsNullOrWhiteSpace(editedEvent.TrackTitle))
                    {
                        MessageBox.Show("Название трека не может быть пустым.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ReFresh();
                        return;
                    }

                    if (editedEvent.EventTime <= DateTime.MinValue)
                    {
                        MessageBox.Show("Дата воспроизведения некорректна.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ReFresh();
                        return;
                    }

                    // Обновляем данные в базе
                    using (var connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();

                            // Получаем ID трека по названию (предполагается, что названия уникальны)
                            string getTrackIdQuery = "SELECT id FROM Track WHERE title = @Title";
                            var getTrackIdCmd = new SqlCommand(getTrackIdQuery, connection);
                            getTrackIdCmd.Parameters.AddWithValue("@Title", editedEvent.TrackTitle);
                            object trackIdObj = getTrackIdCmd.ExecuteScalar();
                            if (trackIdObj == null)
                            {
                                MessageBox.Show("Не удалось найти трек с таким названием.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ReFresh();
                                return;
                            }
                            int trackId = Convert.ToInt32(trackIdObj);

                            // Обновляем событие
                            string updateQuery = "UPDATE Online_event SET КодТрека = @TrackId, ДатаВремя = @EventTime WHERE КодСобытия = @EventId";
                            var updateCmd = new SqlCommand(updateQuery, connection);
                            updateCmd.Parameters.AddWithValue("@TrackId", trackId);
                            updateCmd.Parameters.AddWithValue("@EventTime", editedEvent.EventTime);
                            updateCmd.Parameters.AddWithValue("@EventId", editedEvent.EventId);

                            int rowsAffected = updateCmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Событие успешно обновлено.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ReFresh();
                            }
                            else
                            {
                                MessageBox.Show("Не удалось обновить событие. Возможно, оно было удалено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при обновлении события: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
