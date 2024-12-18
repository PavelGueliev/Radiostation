using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radiostation
{
    public class ServiceTrack
    {
        private readonly string connectionString;

        public ServiceTrack(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Получить все треки из базы данных.
        /// </summary>
        public List<Track> GetAllTracks()
        {
            var tracks = new List<Track>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT Track.id, Track.title as trackTitle, author, Genre.title as genereTitle, duration FROM Track JOIN Genre on Track.genre=Genre.id";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var track = new Track
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Title = reader.GetString(reader.GetOrdinal("trackTitle")),
                            Author = reader.GetString(reader.GetOrdinal("author")),
                            Genre = reader.GetString(reader.GetOrdinal("genereTitle")),
                            Duration = reader.GetTimeSpan(reader.GetOrdinal("duration"))
                        };
                        tracks.Add(track);
                    }
                }
            }
            

            return tracks;
        }

        /// <summary>
        /// Добавить новый трек в базу данных.
        /// </summary>
        public void AddTrack(Track track)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "INSERT INTO Track (id, title, author, genre, duration) VALUES ((select MAX(id) from Track) + 1, @title, @author, (select id from Genre where title=@genre), @duration)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@title", track.Title);
                        command.Parameters.AddWithValue("@author", track.Author);
                        command.Parameters.AddWithValue("@genre", track.Genre);
                        command.Parameters.AddWithValue("@duration", track.Duration);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при добавлении трека: " + ex.Message, ex);
            }
        }

        public void UpdateTrack(Track track)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE Track SET title = @title, author = @author, genre = (select id from Genre where title=@genre), duration=@duration WHERE id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", track.Id);
                    command.Parameters.AddWithValue("@title", track.Title);
                    command.Parameters.AddWithValue("@author", track.Author);
                    command.Parameters.AddWithValue("@genre", track.Genre);
                    command.Parameters.AddWithValue("@duration", track.Duration);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        /// <summary>
        /// Удалить трек по его ID.
        /// </summary>
        public void DeleteTrack(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "DELETE FROM Track WHERE id = @id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new Exception("Трек с указанным ID не найден.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при удалении трека: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Поиск треков по названию, автору и жанру.
        /// Если какой-то из параметров null или пуст, он игнорируется.
        /// </summary>
        public List<Track> SearchTracks(string title, string author, int? genre)
        {
            var tracks = new List<Track>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var queryBuilder = new StringBuilder("SELECT Track.id, Track.title, author, Genre.Title as genreTitle, duration FROM Track JOIN Genre ON Genre.id=Track.genre WHERE 1=1");

                    if (!string.IsNullOrEmpty(title))
                        queryBuilder.Append(" AND Track.title LIKE @title");
                    if (!string.IsNullOrEmpty(author))
                        queryBuilder.Append(" AND author LIKE @author");
                    if (!genre.Equals(null))
                        queryBuilder.Append(" AND genre LIKE @genre");

                    using (var command = new SqlCommand(queryBuilder.ToString(), connection))
                    {
                        if (!string.IsNullOrEmpty(title))
                            command.Parameters.AddWithValue("@title", "%" + title + "%");
                        if (!string.IsNullOrEmpty(author))
                            command.Parameters.AddWithValue("@author", "%" + author + "%");
                        if (!genre.Equals(null))
                            command.Parameters.AddWithValue("@genre",  genre);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var track = new Track
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Title = reader.GetString(reader.GetOrdinal("title")),
                                    Author = reader.GetString(reader.GetOrdinal("author")),
                                    Genre = reader.GetString(reader.GetOrdinal("genreTitle")),
                                    Duration = reader.GetTimeSpan(reader.GetOrdinal("duration"))
                                };
                                tracks.Add(track);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при поиске треков: " + ex.Message, ex);
            }

            return tracks;
        }
    }
}
