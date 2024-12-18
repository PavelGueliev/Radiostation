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
    public partial class Shedule_ads_block_form : Form
    {
        private string connectionString;
        private bool userEditingSchedule = false;

        public Shedule_ads_block_form()
        {
            InitializeComponent();
            connectionString = "Data Source=DESKTOP-6MQUQFM\\SQLEXPRESS;Initial Catalog=Radiostation;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True";

            this.Load += Shedule_ads_block_form_Load;

        }

        private void Shedule_ads_block_form_Load(object sender, EventArgs e)
        {
            LoadPresenters();
            LoadAdBlocks();
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
            presenterComboBox.SelectedIndex = -1;
        }

        private void LoadAdBlocks()
        {
            var blocks = new List<KeyValuePair<int, string>>();
            blocks.Add(new KeyValuePair<int, string>(-1, ""));

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT КодБлока, Название FROM Ad_block ORDER BY Название";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        blocks.Add(new KeyValuePair<int, string>(
                            reader.GetInt32(reader.GetOrdinal("КодБлока")),
                            reader.GetString(reader.GetOrdinal("Название"))
                        ));
                    }
                }
            }

            adBlockComboBox.DataSource = blocks;
            adBlockComboBox.DisplayMember = "Value";
            adBlockComboBox.ValueMember = "Key";
            adBlockComboBox.SelectedIndex = -1;
        }

        private void LoadSchedule()
        {
            var scheduleList = new List<AdSchedule>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT b.Продолжительность, s.КодЗаписи, s.КодВедущего, s.КодБлока, s.ДатаВремя, v.ФИО, b.Название AS НазваниеБлока " +
                               "FROM Shedule_ads_block s " +
                               "JOIN Presenter v ON s.КодВедущего = v.КодВедущего " +
                               "JOIN Ad_block b ON s.КодБлока = b.КодБлока " +
                               "WHERE 1=1";

                if (presenterComboBox.SelectedValue is int selectedPresenterId && selectedPresenterId > 0)
                {
                    query += " AND s.КодВедущего = @presenterId";
                }

                if (adBlockComboBox.SelectedValue is int selectedBlockId && selectedBlockId > 0)
                {
                    query += " AND s.КодБлока = @blockId";
                }

                if (dateTimePicker.Checked)
                {
                    // При сравнении DATETIME используем дату и время. Если нужно фильтр только по дате, можно сравнивать с BETWEEN
                    query += " AND CONVERT(date, s.ДатаВремя) = @date"; // Фильтр по дате (игнорируя время)
                }

                using (var command = new SqlCommand(query, connection))
                {
                    if (presenterComboBox.SelectedValue is int pId && pId > 0)
                        command.Parameters.AddWithValue("@presenterId", pId);

                    if (adBlockComboBox.SelectedValue is int bId && bId > 0)
                        command.Parameters.AddWithValue("@blockId", bId);

                    if (dateTimePicker.Checked)
                        command.Parameters.AddWithValue("@date", dateTimePicker.Value.Date);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            scheduleList.Add(new AdSchedule
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("КодЗаписи")),
                                PresenterId = reader.GetInt32(reader.GetOrdinal("КодВедущего")),
                                BlockId = reader.GetInt32(reader.GetOrdinal("КодБлока")),
                                DateTime = reader.GetDateTime(reader.GetOrdinal("ДатаВремя")),
                                PresenterName = reader.GetString(reader.GetOrdinal("ФИО")),
                                BlockName = reader.GetString(reader.GetOrdinal("НазваниеБлока")),
                                Duration = reader.GetTimeSpan(reader.GetOrdinal("Продолжительность"))
                            });
                        }
                    }
                }
            }

            dataGridViewSheduleBlock.DataSource = scheduleList;

            if (dataGridViewSheduleBlock.Columns["Id"] != null)
                dataGridViewSheduleBlock.Columns["Id"].Visible = false;
            if (dataGridViewSheduleBlock.Columns["PresenterId"] != null)
                dataGridViewSheduleBlock.Columns["PresenterId"].Visible = false;
            if (dataGridViewSheduleBlock.Columns["BlockId"] != null)
                dataGridViewSheduleBlock.Columns["BlockId"].Visible = false;

            dataGridViewSheduleBlock.Columns["PresenterName"].HeaderText = "Ведущий";
            dataGridViewSheduleBlock.Columns["BlockName"].HeaderText = "Рекламный блок";
            dataGridViewSheduleBlock.Columns["DateTime"].HeaderText = "Дата и время";
            dataGridViewSheduleBlock.Columns["Duration"].HeaderText = "Длительность";
            if (dataGridViewSheduleBlock.Columns["PresenterName"] != null)
                dataGridViewSheduleBlock.Columns["PresenterName"].ReadOnly = true;
            if (dataGridViewSheduleBlock.Columns["BlockName"] != null)
                dataGridViewSheduleBlock.Columns["BlockName"].ReadOnly = true;
            if (dataGridViewSheduleBlock.Columns["Duration"] != null)
                dataGridViewSheduleBlock.Columns["Duration"].ReadOnly = true;

            LoadBlockComposition();
        }

        private void LoadBlockComposition()
        {
            if (dataGridViewSheduleBlock.CurrentRow == null || dataGridViewSheduleBlock.CurrentRow.DataBoundItem == null)
            {
                dataGridViewComposition.DataSource = null;
                return;
            }

            var selectedSchedule = (AdSchedule)dataGridViewSheduleBlock.CurrentRow.DataBoundItem;

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
                    command.Parameters.AddWithValue("@blockId", selectedSchedule.BlockId);
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

            dataGridViewComposition.Columns["Title"].HeaderText = "Название ролика";
            dataGridViewComposition.Columns["Duration"].HeaderText = "Продолжительность";
            if (dataGridViewComposition.Columns["Title"] != null)
                dataGridViewComposition.Columns["Title"].ReadOnly = true;
            if (dataGridViewComposition.Columns["Duration"] != null)
                dataGridViewComposition.Columns["Duration"].ReadOnly = true;
        }

        private void dataGridViewSheduleBlock_SelectionChanged(object sender, EventArgs e)
        {
            LoadBlockComposition();
        }

        private void dataGridViewSheduleBlock_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            userEditingSchedule = true;
        }

        private void dataGridViewSheduleBlock_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (!userEditingSchedule) return;
            userEditingSchedule = false;

            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewSheduleBlock.Rows.Count)
            {
                var row = dataGridViewSheduleBlock.Rows[e.RowIndex];
                if (row.DataBoundItem is AdSchedule editedSchedule)
                {
                    // Валидация данных
                    if (editedSchedule.PresenterId <= 0 || string.IsNullOrWhiteSpace(editedSchedule.PresenterName))
                    {
                        MessageBox.Show("Некорректный ведущий.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (editedSchedule.BlockId <= 0 || string.IsNullOrWhiteSpace(editedSchedule.BlockName))
                    {
                        MessageBox.Show("Некорректный рекламный блок.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                            string query = "UPDATE Shedule_ads_block SET КодВедущего = @presenterId, КодБлока = @blockId, ДатаВремя = @dateTime WHERE КодЗаписи = @id";
                            using (var command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@presenterId", editedSchedule.PresenterId);
                                command.Parameters.AddWithValue("@blockId", editedSchedule.BlockId);
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
                if (!(presenterComboBox.SelectedValue is int pId) || pId <= 0)
                {
                    MessageBox.Show("Выберите ведущего.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!(adBlockComboBox.SelectedValue is int bId) || bId <= 0)
                {
                    MessageBox.Show("Выберите рекламный блок.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    string insertQuery = "INSERT INTO Shedule_ads_block (КодЗаписи, КодВедущего, КодБлока, ДатаВремя) VALUES (@id, @presenterId, @blockId, @dateTime)";
                    using (var command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", newId);
                        command.Parameters.AddWithValue("@presenterId", pId);
                        command.Parameters.AddWithValue("@blockId", bId);
                        command.Parameters.AddWithValue("@dateTime", dt);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Запись добавлена в расписание.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSchedule();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            if (dataGridViewSheduleBlock.CurrentRow == null || dataGridViewSheduleBlock.CurrentRow.DataBoundItem == null)
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedSchedule = (AdSchedule)dataGridViewSheduleBlock.CurrentRow.DataBoundItem;

            var result = MessageBox.Show("Вы уверены, что хотите удалить выбранную запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM Shedule_ads_block WHERE КодЗаписи = @id";
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
            string maxQuery = "SELECT ISNULL(MAX(КодЗаписи), 0) FROM Shedule_ads_block";
            using (SqlCommand cmd = new SqlCommand(maxQuery, connection))
            {
                int maxId = (int)cmd.ExecuteScalar();
                return maxId + 1;
            }
        }
    }

    public class AdSchedule
    {
        public int Id { get; set; }
        public int PresenterId { get; set; }
        public int BlockId { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan Duration { get; set; }

        public string PresenterName { get; set; }
        public string BlockName { get; set; }
    }

}
