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
    public partial class Guest : Form
    {
        private string connectionString;
        private bool userEditing = false;

        public Guest()
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
            if (dataGridViewOnlineEvents.Columns["EventTime"] != null)
                dataGridViewOnlineEvents.Columns["EventTime"].ReadOnly = true;
            if (dataGridViewOnlineEvents.Columns["Author"] != null)
                dataGridViewOnlineEvents.Columns["Author"].ReadOnly = true;
            if (dataGridViewOnlineEvents.Columns["TrackTitle"] != null)
                dataGridViewOnlineEvents.Columns["TrackTitle"].ReadOnly = true;
            if (dataGridViewOnlineEvents.Columns["Genre"] != null)
                dataGridViewOnlineEvents.Columns["Genre"].ReadOnly = true;
            if (dataGridViewOnlineEvents.Columns["Duration"] != null)
                dataGridViewOnlineEvents.Columns["Duration"].ReadOnly = true;

        }
        private void exit_button_Click(object sender, EventArgs e)
        {
            this.Close();
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
                            g.Title AS Жанр,
                            oe.ДатаВремя AS [Дата воспроизведения]
                        FROM 
                            Online_event oe
                            JOIN Track t ON oe.КодТрека = t.id
                            JOIN Genre g ON t.genre = g.id
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
    }
}
