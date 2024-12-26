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

        // Добавление ведущего
        public void AddPresenter(Presenter presenter)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 1) Генерируем новый КодВедущего (если нет IDENTITY в таблице Presenter)
                int newPresenterId = GetNextPresenterId(connection);

                // 2) INSERT в таблицу Presenter
                string insertPresenterQuery = @"
            INSERT INTO Presenter (КодВедущего, ФИО, НомерТелефона, ДатаРождения)
            VALUES (@id, @fio, @tel, @birth)
        ";
                using (var cmd1 = new SqlCommand(insertPresenterQuery, connection))
                {
                    cmd1.Parameters.AddWithValue("@id", newPresenterId);
                    cmd1.Parameters.AddWithValue("@fio", presenter.ФИО);
                    cmd1.Parameters.AddWithValue("@tel", presenter.НомерТелефона);
                    cmd1.Parameters.AddWithValue("@birth", presenter.ДатаРождения.Date);
                    cmd1.ExecuteNonQuery();
                }

                // 3) Создаем запись в Users_radiostation
                // Предположим, логин = ФИО или "Presenter_N" + newPresenterId
                // Пароль берем из presenter.Пароль
                // Role = "Presenter"
                string login = presenter.НомерТелефона; // Пример автогенерации
                string insertUserQuery = @"
            INSERT INTO Users_radiostation (Логин, Пароль, Role, PresenterID)
            VALUES (@login, @pwd, 'Presenter', @pid)
        ";
                using (var cmd2 = new SqlCommand(insertUserQuery, connection))
                {
                    cmd2.Parameters.AddWithValue("@login", login);
                    cmd2.Parameters.AddWithValue("@pwd", presenter.Пароль ?? "");
                    cmd2.Parameters.AddWithValue("@pid", newPresenterId);
                    cmd2.ExecuteNonQuery();
                }
            }
        }

        // Поиск ведущих по критериям
        public List<Presenter> SearchPresenters(string name, string number, DateTime? birthDate)
        {
            var presenters = new List<Presenter>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Объединяем Presenter c Users_radiostation (где Role='Presenter')
                // чтобы получить пароль из поля [Пароль].
                string query = @"
            SELECT p.КодВедущего,
                   p.ФИО,
                   p.НомерТелефона,
                   p.ДатаРождения,
                   u.Пароль
            FROM Presenter p
            LEFT JOIN Users_radiostation u ON p.КодВедущего = u.PresenterID
                                           AND u.Role = 'Presenter'
            WHERE isDeleted=0 
        ";

                if (!string.IsNullOrEmpty(name))
                    query += " AND p.ФИО LIKE @name";
                if (!string.IsNullOrEmpty(number))
                    query += " AND p.НомерТелефона LIKE @number";
                if (birthDate.HasValue)
                    query += " AND p.ДатаРождения = @birthDate";

                using (var command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(name))
                        command.Parameters.AddWithValue("@name", "%" + name + "%");
                    if (!string.IsNullOrEmpty(number))
                        command.Parameters.AddWithValue("@number", "%" + number + "%");
                    if (birthDate.HasValue)
                        command.Parameters.AddWithValue("@birthDate", birthDate.Value.Date);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pr = new Presenter
                            {
                                КодВедущего = reader.GetInt32(reader.GetOrdinal("КодВедущего")),
                                ФИО = reader.GetString(reader.GetOrdinal("ФИО")),
                                НомерТелефона = reader.GetString(reader.GetOrdinal("НомерТелефона")),
                                ДатаРождения = reader.GetDateTime(reader.GetOrdinal("ДатаРождения"))
                            };

                            // Пароль может быть NULL в базе, поэтому проверяем IsDBNull:
                            if (!reader.IsDBNull(reader.GetOrdinal("Пароль")))
                                pr.Пароль = reader.GetString(reader.GetOrdinal("Пароль"));
                            else
                                pr.Пароль = "";

                            presenters.Add(pr);
                        }
                    }
                }
            }
            return presenters;
        }

        // Удаление ведущего
        public void DeletePresenter(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Presenter SET isDeleted = 1 WHERE КодВедущего = @id;" +
                    "DELETE FROM Users_radiostation WHERE PresenterID = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                // Users_radiostation удалится само, благодаря ON DELETE CASCADE
            }
        }

        public void DeepDeletePresenter(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Presenter WHERE КодВедущего = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                // Users_radiostation удалится само, благодаря ON DELETE CASCADE
            }
        }

        // Обновление ведущего
        public void UpdatePresenter(int id, string fio, string phone, DateTime birthDate, string password)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 1) Обновляем таблицу Presenter
                string updatePresenterQuery = @"
            UPDATE Presenter
            SET ФИО = @fio, НомерТелефона = @phone, ДатаРождения = @birth
            WHERE КодВедущего = @id
        ";
                using (var cmd1 = new SqlCommand(updatePresenterQuery, connection))
                {
                    cmd1.Parameters.AddWithValue("@fio", fio);
                    cmd1.Parameters.AddWithValue("@phone", phone);
                    cmd1.Parameters.AddWithValue("@birth", birthDate.Date);
                    cmd1.Parameters.AddWithValue("@id", id);
                    cmd1.ExecuteNonQuery();
                }

                // 2) Обновляем или создаём запись в Users_radiostation
                //    WHERE PresenterID = id AND Role = 'Presenter'
                // Проверим, есть ли такая запись
                string checkUserQuery = @"
            SELECT COUNT(*)
            FROM Users_radiostation
            WHERE PresenterID = @id AND Role='Presenter'
        ";
                int count;
                using (var cmd2 = new SqlCommand(checkUserQuery, connection))
                {
                    cmd2.Parameters.AddWithValue("@id", id);
                    count = (int)cmd2.ExecuteScalar();
                }

                if (count == 0)
                {
                    // создаём запись в Users_radiostation
                    // Логин можно генерировать или хранить, пусть "Presenter_<id>"
                    string login = "Presenter_" + id;
                    string insertUserQuery = @"
                INSERT INTO Users_radiostation (Логин, Пароль, Role, PresenterID)
                VALUES (@login, @pwd, 'Presenter', @pid)
            ";
                    using (var cmd3 = new SqlCommand(insertUserQuery, connection))
                    {
                        cmd3.Parameters.AddWithValue("@login", login);
                        cmd3.Parameters.AddWithValue("@pwd", password ?? "");
                        cmd3.Parameters.AddWithValue("@pid", id);
                        cmd3.ExecuteNonQuery();
                    }
                }
                else
                {
                    // обновляем пароль
                    string updateUserQuery = @"
                UPDATE Users_radiostation
                SET Пароль = @pwd
                WHERE PresenterID = @id AND Role='Presenter'
            ";
                    using (var cmd4 = new SqlCommand(updateUserQuery, connection))
                    {
                        cmd4.Parameters.AddWithValue("@pwd", password ?? "");
                        cmd4.Parameters.AddWithValue("@id", id);
                        cmd4.ExecuteNonQuery();
                    }
                }
            }
        }

        // Пример метода для определения нового КодВедущего
        private int GetNextPresenterId(SqlConnection connection)
        {
            string maxQuery = "SELECT ISNULL(MAX(КодВедущего), 0) FROM Presenter";
            using (var cmd = new SqlCommand(maxQuery, connection))
            {
                int maxId = (int)cmd.ExecuteScalar();
                return maxId + 1;
            }
        }
    }
}
