using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Radiostation
{
    public class PresenterRepository
    {
        private readonly string connectionString;

        public PresenterRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Добавление ведущего в базу данных
        public void AddPresenter(Presenter presenter)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO Presenter (КодВедущего, ФИО, НомерТелефона, ДатаРождения) VALUES ((select MAX(КодВедущего) from Presenter) + 1, @ФИО, @НомерТелефона, @ДатаРождения)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ФИО", presenter.ФИО);
                    command.Parameters.AddWithValue("@НомерТелефона", presenter.НомерТелефона);
                    command.Parameters.AddWithValue("@ДатаРождения", presenter.ДатаРождения);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        // Поиск ведущих по критериям
        public List<Presenter> SearchPresenters(string name, string number, DateTime? birthDate)
        {
            var presenters = new List<Presenter>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Presenter WHERE 1=1";

                if (!string.IsNullOrEmpty(name))
                    query += " AND ФИО LIKE @ФИО";
                if (!string.IsNullOrEmpty(number))
                    query += " AND НомерТелефона LIKE @НомерТелефона";
                if (birthDate.HasValue)
                    query += " AND ДатаРождения = @ДатаРождения";

                using (var command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(name))
                        command.Parameters.AddWithValue("@ФИО", "%" + name + "%");
                    if (!string.IsNullOrEmpty(number))
                        command.Parameters.AddWithValue("@НомерТелефона", "%" + number + "%");
                    if (birthDate.HasValue)
                        command.Parameters.AddWithValue("@ДатаРождения", birthDate.Value);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var presenter = new Presenter
                            {
                                КодВедущего = reader.GetInt32(0),
                                ФИО = reader.GetString(1),
                                НомерТелефона = reader.GetString(2),
                                ДатаРождения = reader.GetDateTime(3)
                            };
                            presenters.Add(presenter);
                        }
                    }
                }
                connection.Close();
            }
            return presenters;
        }

        public void UpdatePresenter(int id, string name, string number, DateTime? birthDate)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE Presenter SET ФИО = @ФИО, НомерТелефона = @НомерТелефона, ДатаРождения = @ДатаРождения WHERE КодВедущего = @КодВедущего";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ДатаРождения", birthDate.Value);
                    command.Parameters.AddWithValue("@ФИО", name);
                    command.Parameters.AddWithValue("@НомерТелефона", number);
                    command.Parameters.AddWithValue("@КодВедущего", id);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    // Удаление ведущего по ID
    public void DeletePresenter(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Presenter WHERE КодВедущего = @КодВедущего";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@КодВедущего", id);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
