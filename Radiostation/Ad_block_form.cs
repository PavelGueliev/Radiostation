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
    public partial class Ad_block_form : Form
    {
        private string connectionString;
        private bool userEditingBlock = false;
        private bool isUpdating = false;
        public Ad_block_form()
        {
            connectionString = "Data Source=DESKTOP-6MQUQFM\\SQLEXPRESS;Initial Catalog=Radiostation;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True";

            this.Load += Ad_block_form_Load;
            InitializeComponent();
        }

        private void Ad_block_form_Load(object sender, EventArgs e)
        {
            LoadAdBlocks();
            LoadAdRecords(); // Загрузка списка доступных роликов в ComboBox
        }

        private void LoadAdBlocks()
        {
            var adBlocks = new List<AdBlock>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT КодБлока, Название, Продолжительность FROM Ad_block WHERE 1=1";
                if (!string.IsNullOrEmpty(adBlockTextBox.Text))
                {
                    query += " AND Название LIKE @nameFilter";
                }

                using (var command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(adBlockTextBox.Text))
                        command.Parameters.AddWithValue("@nameFilter", "%" + adBlockTextBox.Text + "%");

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            adBlocks.Add(new AdBlock
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("КодБлока")),
                                Title = reader.GetString(reader.GetOrdinal("Название")),
                                Duration = (TimeSpan)reader["Продолжительность"]
                            });
                        }
                    }
                }
            }

            dataGridViewAdBlock.DataSource = adBlocks;
            if (dataGridViewAdBlock.Columns["Id"] != null)
                dataGridViewAdBlock.Columns["Id"].Visible = false;

            dataGridViewAdBlock.Columns["Title"].HeaderText = "Название";
            dataGridViewAdBlock.Columns["Duration"].HeaderText = "Продолжительность";

            LoadComposition();
        }

        private void LoadComposition()
        {
            if (dataGridViewAdBlock.CurrentRow == null || dataGridViewAdBlock.CurrentRow.DataBoundItem == null)
            {
                dataGridViewComposition.DataSource = null;
                return;
            }

            var selectedBlock = (AdBlock)dataGridViewAdBlock.CurrentRow.DataBoundItem;
            var composition = new List<BlockComposition>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                SELECT cb.КодБлока, cb.КодРолика, r.НазваниеРолика, r.Продолжительность
                FROM Block_composition cb
                JOIN Ad_record r ON cb.КодРолика = r.КодРолика
                WHERE cb.КодБлока = @blockId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@blockId", selectedBlock.Id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            composition.Add(new BlockComposition
                            {
                                BlockId = reader.GetInt32(reader.GetOrdinal("КодБлока")),
                                RecordId = reader.GetInt32(reader.GetOrdinal("КодРолика")),
                                Title = reader.GetString(reader.GetOrdinal("НазваниеРолика")),
                                Duration = (TimeSpan)reader["Продолжительность"]
                            });
                        }
                    }
                }
            }

            dataGridViewComposition.DataSource = composition;
            if (dataGridViewComposition.Columns["BlockId"] != null)
                dataGridViewComposition.Columns["BlockId"].Visible = false;
            if (dataGridViewComposition.Columns["RecordId"] != null)
                dataGridViewComposition.Columns["RecordId"].Visible = false;
            if (dataGridViewComposition.Columns["Title"] != null)
                dataGridViewComposition.Columns["Title"].ReadOnly = true;
            if (dataGridViewComposition.Columns["Duration"] != null)
                dataGridViewComposition.Columns["Duration"].ReadOnly = true;
            dataGridViewComposition.Columns["Title"].HeaderText = "Название ролика";
            dataGridViewComposition.Columns["Duration"].HeaderText = "Продолжительность";
        }

        private void LoadAdRecords()
        {
            // Загрузим все названия роликов в ComboBox, чтобы пользователь мог выбрать какой ролик добавить
            // В таблице Ad_record: КодРолика, НазваниеРолика, Продолжительность
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT НазваниеРолика FROM Ad_record ORDER BY НазваниеРолика";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    var records = new List<string>();
                    records.Add(""); // Пустое значение

                    while (reader.Read())
                    {
                        records.Add(reader.GetString(reader.GetOrdinal("НазваниеРолика")));
                    }

                    authorsComboBox.DataSource = null;
                    authorsComboBox.DataSource = records;
                }
            }
        }

        private void dataGridViewAdBlock_SelectionChanged(object sender, EventArgs e)
        {
            LoadComposition();
        }

        private void dataGridViewAdBlock_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            userEditingBlock = true;
        }

        private void dataGridViewAdBlock_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (!userEditingBlock) return;
            userEditingBlock = false;

            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewAdBlock.Rows.Count)
            {
                var row = dataGridViewAdBlock.Rows[e.RowIndex];
                if (row.DataBoundItem is AdBlock editedBlock)
                {
                    // Валидация
                    if (string.IsNullOrWhiteSpace(editedBlock.Title))
                    {
                        MessageBox.Show("Название блока не может быть пустым.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (editedBlock.Duration <= TimeSpan.Zero)
                    {
                        MessageBox.Show("Продолжительность должна быть больше 0.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Сохраняем изменения в БД
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "UPDATE AD_block SET Название = @title, Продолжительность = @duration WHERE КодБлока = @id";
                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@title", editedBlock.Title);
                            command.Parameters.AddWithValue("@duration", editedBlock.Duration);
                            command.Parameters.AddWithValue("@id", editedBlock.Id);
                            try
                            {
                                command.ExecuteNonQuery();
                                MessageBox.Show("Блок изменен.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (SqlException ex)
                            {
                                string userMessage;

                                switch (ex.Number)
                                {
                                    case 547:
                                        // Ошибка нарушения целостности при связи (FK) или ограничения CHECK
                                        // "The INSERT statement conflicted with the FOREIGN KEY constraint" и т.п.
                                        userMessage = "Нарушение целостности данных.";
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
                                        userMessage = $"Ошибка SQL (код {ex.Number}): {ex.Message}";
                                        break;
                                }

                                // Выводим конечное сообщение пользователю
                                MessageBox.Show(userMessage,
                                                "Ошибка SQL",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Error);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка при обновлении базы данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                              
                            }
                            
                        }
                    }
                }
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            try
            {
                LoadAdBlocks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void addAdBlockButton_Click(object sender, EventArgs e)
        {
            string blockName = adBlockTextBox.Text;
            TimeSpan duration = TimeSpan.FromSeconds(1); // Пример: пусть по умолчанию 1 секунду

            if (string.IsNullOrWhiteSpace(blockName))
            {
                MessageBox.Show("Введите название рекламного блока.");
                return;
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Проверяем, что такого названия еще нет
                string checkQuery = "SELECT COUNT(*) FROM Ad_block WHERE Название = @name";
                using (var checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@name", blockName);
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Блок с таким названием уже существует. Введите другое название.");
                        return;
                    }
                }

                string insertQuery = "INSERT INTO Ad_block (КодБлока, Название, Продолжительность) VALUES (@id, @title, @duration)";
                using (var command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", GenerateAdBlockId(connection));
                    command.Parameters.AddWithValue("@title", blockName);
                    command.Parameters.AddWithValue("@duration", duration);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Рекламный блок добавлен.");
                LoadAdBlocks();
            }
        }

        private void removeAdBlockButton_Click(object sender, EventArgs e)
        {
            string blockName = adBlockTextBox.Text;

            if (string.IsNullOrWhiteSpace(blockName))
            {
                MessageBox.Show("Введите название рекламного блока для удаления.");
                return;
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Ad_block WHERE Название = @name";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", blockName);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Рекламный блок удалён.");
                    }
                    else
                    {
                        MessageBox.Show("Рекламный блок не найден.");
                    }
                }
                LoadAdBlocks();
            }
        }

        private void addAdButton_Click(object sender, EventArgs e)
        {
            if (dataGridViewAdBlock.CurrentRow == null || dataGridViewAdBlock.CurrentRow.DataBoundItem == null) return;
            var selectedBlock = (AdBlock)dataGridViewAdBlock.CurrentRow.DataBoundItem;

            string recordTitle = authorsComboBox.Text;
            if (string.IsNullOrWhiteSpace(recordTitle))
            {
                MessageBox.Show("Выберите название ролика для добавления.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = @"
                INSERT INTO Block_composition (КодРолика, КодБлока) 
                SELECT КодРолика, @blockId FROM Ad_record WHERE НазваниеРолика = @recordTitle";

                using (var command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@blockId", selectedBlock.Id);
                    command.Parameters.AddWithValue("@recordTitle", recordTitle);

                    try
                    {
                        int rows = command.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Ролик добавлен в рекламный блок.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Не удалось добавить ролик. Возможно, такого ролика не существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (SqlException ex)
                    {
                        string userMessage;

                        switch (ex.Number)
                        {
                            case 547:
                                // Ошибка нарушения целостности при связи (FK) или ограничения CHECK
                                // "The INSERT statement conflicted with the FOREIGN KEY constraint" и т.п.
                                userMessage = "Нарушение целостности данных.";
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
                                userMessage = $"Ошибка SQL (код {ex.Number}): {ex.Message}";
                                break;
                        }

                        // Выводим конечное сообщение пользователю
                        MessageBox.Show(userMessage,
                                        "Ошибка SQL",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                    catch
                    {
                        MessageBox.Show("Ролик уже есть в данном блоке или произошла ошибка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            LoadComposition();
        }

        private void removeAdButton_Click(object sender, EventArgs e)
        {
            if (dataGridViewComposition.CurrentRow == null || dataGridViewComposition.CurrentRow.DataBoundItem == null) return;
            var selectedComposition = (BlockComposition)dataGridViewComposition.CurrentRow.DataBoundItem;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM Block_composition WHERE КодРолика = @recordId AND КодБлока = @blockId";
                using (var command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@recordId", selectedComposition.RecordId);
                    command.Parameters.AddWithValue("@blockId", selectedComposition.BlockId);
                    try
                    {
                        int rows = command.ExecuteNonQuery();
                        MessageBox.Show("Ролик удален из рекламного блока.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (SqlException ex)
                    {
                        string userMessage;

                        switch (ex.Number)
                        {
                            case 547:
                                // Ошибка нарушения целостности при связи (FK) или ограничения CHECK
                                // "The INSERT statement conflicted with the FOREIGN KEY constraint" и т.п.
                                userMessage = "В блоке должен быть хотя бы 1 ролик";
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
                                userMessage = $"Ошибка SQL (код {ex.Number}): {ex.Message}";
                                break;
                        }

                        // Выводим конечное сообщение пользователю
                        MessageBox.Show(userMessage,
                                        "Ошибка SQL",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Не удалось удалить ролик из блока.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
            }

            LoadComposition();
        }

        private int GenerateAdBlockId(SqlConnection connection)
        {
            string maxQuery = "SELECT ISNULL(MAX(КодБлока), 0) FROM Ad_block";
            using (SqlCommand cmd = new SqlCommand(maxQuery, connection))
            {
                int maxId = (int)cmd.ExecuteScalar();
                return maxId + 1;
            }
        }

        private void Ad_block_form_Load_1(object sender, EventArgs e)
        {

        }
    }

    public class AdBlock
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
    }

    public class BlockComposition
    {
        public int BlockId { get; set; }
        public int RecordId { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
    }
}

