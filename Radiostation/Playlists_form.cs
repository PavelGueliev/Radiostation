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
    public partial class Playlists_form : Form
    {
        private string connectionString;
        private bool userEditingPlaylist = false;
        private bool userEditingComposition = false;
        private bool isUpdating = false;
        public Playlists_form()
        {
            InitializeComponent();
            connectionString = "Data Source=DESKTOP-6MQUQFM\\SQLEXPRESS;Initial Catalog=Radiostation;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True";

            this.Load += FormPlaylists_Load;
        }

        private void FormPlaylists_Load(object sender, EventArgs e)
        {
            LoadPlaylists();
            LoadTitles();
            LoadAuthors();
        }

        private void LoadPlaylists()
        {

            var playlists = new List<Playlist>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT КодПлейлиста, Название, Продолжительность FROM Playlist Where 1=1";
                if (!string.IsNullOrEmpty(playlistTextBox.Text))
                {
                    query += " AND Название LIKE @Playlist";
                }
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Playlist", "%" + playlistTextBox.Text + "%");
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            playlists.Add(new Playlist
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("КодПлейлиста")),
                                Title = reader.GetString(reader.GetOrdinal("Название")),
                                Duration = (TimeSpan)reader["Продолжительность"]
                            });
                        }
                    }
                }

            }

            dataGridViewPlaylists.DataSource = playlists;
            if (dataGridViewPlaylists.Columns["Id"] != null)
                dataGridViewPlaylists.Columns["Id"].Visible = false;

            dataGridViewPlaylists.Columns["Title"].HeaderText = "Название";
            dataGridViewPlaylists.Columns["Duration"].HeaderText = "Продолжительность";

            // Загружаем состав для первого выбранного плейлиста, если есть
            LoadComposition();
        }

        private void LoadComposition()
        {
            if (dataGridViewPlaylists.CurrentRow == null)
            {
                dataGridViewComposition.DataSource = null;
                return;
            }

            if (dataGridViewPlaylists.CurrentRow.DataBoundItem is Playlist selectedPlaylist)
            {
                var composition = new List<PlaylistComposition>();
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT КодПлейлиста, КодТрека, Author, Track.Title, Genre.title as genreTitle, Track.Duration FROM Playlist_composition JOIN Track on Track.Id = КодТрека JOIN Genre ON Track.genre=Genre.id Where КодПлейлиста = @playlistId";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@playlistId", selectedPlaylist.Id);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                composition.Add(new PlaylistComposition
                                {
                                    PlaylistId = reader.GetInt32(reader.GetOrdinal("КодПлейлиста")),
                                    TrackId = reader.GetInt32(reader.GetOrdinal("КодТрека")),
                                    Author = reader.GetString(reader.GetOrdinal("Author")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Duration = reader.GetTimeSpan(reader.GetOrdinal("Duration")),
                                    Genre = reader.GetString(reader.GetOrdinal("genreTitle"))
                                });
                            }
                        }
                    }
                }

                dataGridViewComposition.DataSource = composition;
                if (dataGridViewComposition.Columns["PlaylistId"] != null)
                    dataGridViewComposition.Columns["PlaylistId"].Visible = false;

                dataGridViewComposition.Columns["TrackId"].HeaderText = "Код трека";
                dataGridViewComposition.Columns["TrackId"].Visible = false;
                dataGridViewComposition.Columns["Author"].HeaderText = "Автор";
                dataGridViewComposition.Columns["Title"].HeaderText = "Название";
                dataGridViewComposition.Columns["Genre"].HeaderText = "Жанр";
                dataGridViewComposition.Columns["Duration"].HeaderText = "Длительность";
                if (dataGridViewComposition.Columns["Genre"] != null)
                    dataGridViewComposition.Columns["Genre"].ReadOnly = true;
                if (dataGridViewComposition.Columns["Title"] != null)
                    dataGridViewComposition.Columns["Title"].ReadOnly = true;
                if (dataGridViewComposition.Columns["Author"] != null)
                    dataGridViewComposition.Columns["Author"].ReadOnly = true;
                if (dataGridViewComposition.Columns["Duration"] != null)
                    dataGridViewComposition.Columns["Duration"].ReadOnly = true;
            }
        }

        private void dataGridViewPlaylists_SelectionChanged(object sender, EventArgs e)
        {
            LoadComposition();
        }

        private void dataGridViewPlaylists_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            userEditingPlaylist = true;
        }

        private void LoadAuthors(string selectedTitle = null)
        {
            isUpdating = true; // Начинаем обновление
            try
            {
                string query = "SELECT DISTINCT Author FROM Track WHERE 1=1";
                if (!string.IsNullOrEmpty(selectedTitle))
                {
                    query += " AND Title = @title";
                }

                string oldSelectedAuthor = authorsComboBox.SelectedItem as string;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(selectedTitle))
                            command.Parameters.AddWithValue("@title", selectedTitle);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            var authors = new List<string>();
                            authors.Add("");

                            while (reader.Read())
                            {
                                authors.Add(reader.GetString(reader.GetOrdinal("Author")));
                            }

                            authorsComboBox.DataSource = null;
                            authorsComboBox.DataSource = authors;

                            // Возвращаем старый выбранный автор, если он есть
                            if (!string.IsNullOrEmpty(oldSelectedAuthor) && authors.Contains(oldSelectedAuthor))
                            {
                                authorsComboBox.SelectedItem = oldSelectedAuthor;
                            }

                            // Если передан selectedTitle и он есть в списке titleComboBox
                            if (!string.IsNullOrEmpty(selectedTitle) && titlesComboBox.Items.Contains(selectedTitle))
                            {
                                titlesComboBox.SelectedItem = selectedTitle;
                            }
                        }
                    }
                }
            }
            finally
            {
                isUpdating = false; // Завершаем обновление
            }
        }

        private void LoadTitles(string selectedAuthor = null)
        {
            isUpdating = true; // Начинаем обновление
            try
            {
                string query = "SELECT DISTINCT Title FROM Track WHERE 1=1";
                if (!string.IsNullOrEmpty(selectedAuthor))
                {
                    query += " AND Author = @author";
                }

                string oldSelectedTitle = titlesComboBox.SelectedItem as string;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(selectedAuthor))
                            command.Parameters.AddWithValue("@author", selectedAuthor);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            var titles = new List<string>();
                            titles.Add("");

                            while (reader.Read())
                            {
                                titles.Add(reader.GetString(reader.GetOrdinal("Title")));
                            }

                            titlesComboBox.DataSource = null;
                            titlesComboBox.DataSource = titles;

                            if (!string.IsNullOrEmpty(oldSelectedTitle) && titles.Contains(oldSelectedTitle))
                            {
                                titlesComboBox.SelectedItem = oldSelectedTitle;
                            }

                            if (!string.IsNullOrEmpty(selectedAuthor) && authorsComboBox.Items.Contains(selectedAuthor))
                            {
                                authorsComboBox.SelectedItem = selectedAuthor;
                            }
                        }
                    }
                }
            }
            finally
            {
                isUpdating = false; // Завершаем обновление
            }
        }

        private void dataGridViewPlaylists_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (!userEditingPlaylist) return;
            userEditingPlaylist = false;

            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewPlaylists.Rows.Count)
            {
                var row = dataGridViewPlaylists.Rows[e.RowIndex];
                if (row.DataBoundItem is Playlist editedPlaylist)
                {
                    // Валидация
                    if (string.IsNullOrWhiteSpace(editedPlaylist.Title))
                    {
                        MessageBox.Show("Название плейлиста не может быть пустым.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (editedPlaylist.Duration <= TimeSpan.Zero)
                    {
                        MessageBox.Show("Продолжительность должна быть больше 0.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Сохраняем изменения в БД
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "UPDATE Playlist SET Название = @title, Продолжительность = @duration WHERE КодПлейлиста = @id";
                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@title", editedPlaylist.Title);
                            command.Parameters.AddWithValue("@duration", editedPlaylist.Duration);
                            command.Parameters.AddWithValue("@id", editedPlaylist.Id);

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }

            userEditingPlaylist = false;
        }

        private void addTrackButton_Click(object sender, EventArgs e)
        {
            string trackTitle = titlesComboBox.Text;
            string trackAuthor = authorsComboBox.Text;

            // Проверка на пустые значения
            if (string.IsNullOrWhiteSpace(trackTitle))
            {
                MessageBox.Show("Название трека не должно быть пустым.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Прерываем выполнение, если значение некорректно
            }

            if (string.IsNullOrWhiteSpace(trackAuthor))
            {
                MessageBox.Show("Имя автора не должно быть пустым.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Прерываем выполнение, если значение некорректно
            }


            if (dataGridViewPlaylists.CurrentRow == null || dataGridViewPlaylists.CurrentRow.DataBoundItem == null) return;
            var selectedPlaylist = (Playlist)dataGridViewPlaylists.CurrentRow.DataBoundItem;

            // Добавляем трек в плейлист
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Playlist_composition (КодПлейлиста, КодТрека) VALUES (@playlistId, (select Id from Track where title = @trackTitle and author = @trackAuthor))";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@playlistId", selectedPlaylist.Id);
                    command.Parameters.AddWithValue("@trackTitle", trackTitle);
                    command.Parameters.AddWithValue("@trackAuthor", trackAuthor);
                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Трек добавлен в плейлист.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось добавить трек. Возможно, он уже есть в плейлисте.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            LoadComposition();
        }

        private void removeTrackButton_Click(object sender, EventArgs e)
        {
            if (dataGridViewComposition.CurrentRow == null || dataGridViewComposition.CurrentRow.DataBoundItem == null) return;
            var selectedComposition = (PlaylistComposition)dataGridViewComposition.CurrentRow.DataBoundItem;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Playlist_composition WHERE КодПлейлиста = @playlistId AND КодТрека = @trackId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@playlistId", selectedComposition.PlaylistId);
                    command.Parameters.AddWithValue("@trackId", selectedComposition.TrackId);
                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Трек удален из плейлиста.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить трек.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            LoadComposition();
        }

        private void titlesComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }

        private void authorsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isUpdating)
            {
                LoadTitles(authorsComboBox.Text);
            }
        }

        private void titlesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isUpdating)
            {
                LoadAuthors(titlesComboBox.Text);
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            try
            {
                LoadPlaylists();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void addPlaylistButton_Click(object sender, EventArgs e)
        {
            string playlistName = playlistTextBox.Text; // Название плейлиста из TextBox
            TimeSpan duration = TimeSpan.FromMinutes(60); // Пример: продолжительность плейлиста 1 час

            if (string.IsNullOrWhiteSpace(playlistName))
            {
                MessageBox.Show("Введите название плейлиста.");
                return;
            }

            string query = "INSERT INTO Playlist (КодПлейлиста, Название, Продолжительность) VALUES (@КодПлейлиста, @Название, @Продолжительность)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string maxQuery = "SELECT COUNT(КодПлейлиста) FROM Playlist Where Название=@Название";
                using (SqlCommand cmd = new SqlCommand(maxQuery, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@Название", playlistName);

                    int count = (int)cmd.ExecuteScalar();
                    connection.Close();
                    if (count>0)
                    {
                        MessageBox.Show("Введите другое название плейлиста.");
                        return;
                    }
                }
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@КодПлейлиста", GeneratePlaylistId(connection));
                        command.Parameters.AddWithValue("@Название", playlistName);
                        command.Parameters.AddWithValue("@Продолжительность", duration);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Плейлист добавлен.");
                    }
                    LoadPlaylists();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }

        }

        private void removePlaylistButton_Click(object sender, EventArgs e)
        {
            string playlistName = playlistTextBox.Text;

            if (string.IsNullOrWhiteSpace(playlistName))
            {
                MessageBox.Show("Введите название плейлиста для удаления.");
                return;
            }

            string query = "DELETE FROM Playlist WHERE Название = @Название";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Название", playlistName);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Плейлист удалён.");
                        }
                        else
                        {
                            MessageBox.Show("Плейлист не найден.");
                        }
                    }
                    LoadPlaylists();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        // Пример простого метода для генерации ID
        private int GeneratePlaylistId(SqlConnection connection)
        {
            // Возьмем максимальный код плейлиста и прибавим 1
            string maxQuery = "SELECT ISNULL(MAX(КодПлейлиста), 0) FROM Playlist";
            using (SqlCommand cmd = new SqlCommand(maxQuery, connection))
            {
                int maxId = (int)cmd.ExecuteScalar();
                return maxId + 1;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

    public class Playlist
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
    }

    public class PlaylistComposition
    {
        public int PlaylistId { get; set; }
        public int TrackId { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
