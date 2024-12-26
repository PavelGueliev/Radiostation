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
    public partial class Shedule_Ads_And_Playlists : Form
    {
        private string connectionString = "Data Source=DESKTOP-6MQUQFM\\SQLEXPRESS;Initial Catalog=Radiostation;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True";

        public Shedule_Ads_And_Playlists()
        {
            InitializeComponent();
            LoadPresenters();
            loadGrid();
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
        }

        private List<UnifiedScheduleItem> LoadSchedule(int? presenterId, DateTime? dateFilter)
        {
            var resultList = new List<UnifiedScheduleItem>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 1) Запрос на расписание рекламных блоков (Shedule_ads_block)
                // 2) UNION ALL с расписанием плейлистов (Shedule_playlists)
                // Поля в обоих SELECT должны совпадать по типу и порядку
                // Сортировку делаем в конечном SELECT * FROM (...) ORDER BY PlayDateTime ASC

                string unionQuery = @"
SELECT 
    s.КодЗаписи      AS RecordId,
    s.КодВедущего    AS PresenterId,
    s.ДатаВремя      AS PlayDateTime,
    pr.ФИО           AS PresenterName,
    ab.Название      AS ItemName,
    'Рекламный блок'             AS ItemType,
    ab.Продолжительность
FROM Shedule_ads_block s
JOIN Presenter pr ON s.КодВедущего = pr.КодВедущего
JOIN Ad_block ab ON s.КодБлока = ab.КодБлока
WHERE s.IsDeleted = 0

UNION ALL

SELECT
    sp.КодЗаписи     AS RecordId,
    sp.КодВедущего   AS PresenterId,
    sp.ДатаВремя     AS PlayDateTime,
    pr2.ФИО          AS PresenterName,
    pls.Название     AS ItemName,
    'Плейлист'       AS ItemType,
    pls.Продолжительность
FROM Shedule_playlists sp
JOIN Presenter pr2 ON sp.КодВедущего = pr2.КодВедущего
JOIN Playlist pls ON sp.КодПлейлиста = pls.КодПлейлиста
WHERE sp.IsDeleted = 0
";

                // Оборачиваем в SELECT * FROM (...) AS unified для удобной фильтрации и сортировки:
                string mainQuery = @"
SELECT *
FROM
(
    " + unionQuery + @"
) AS unified
WHERE 1=1
";

                // Фильтр по ведущему
                if (presenterId.HasValue && presenterId.Value > 0)
                    mainQuery += " AND unified.PresenterId = @presenterId";

                // Фильтр по дате (если нужно)
                if (dateFilter.HasValue)
                    mainQuery += " AND CONVERT(date, unified.PlayDateTime) = @dateFilter";

                // Добавляем сортировку
                mainQuery += " ORDER BY unified.PlayDateTime ASC";

                using (var command = new SqlCommand(mainQuery, connection))
                {
                    if (presenterId.HasValue && presenterId.Value > 0)
                        command.Parameters.AddWithValue("@presenterId", presenterId.Value);

                    if (dateFilter.HasValue)
                        command.Parameters.AddWithValue("@dateFilter", dateFilter.Value.Date);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new UnifiedScheduleItem
                            {
                                RecordId = reader.GetInt32(reader.GetOrdinal("RecordId")),
                                PresenterId = reader.GetInt32(reader.GetOrdinal("PresenterId")),
                                PlayDateTime = reader.GetDateTime(reader.GetOrdinal("PlayDateTime")),
                                PresenterName = reader.GetString(reader.GetOrdinal("PresenterName")),
                                Duration = reader.GetTimeSpan(reader.GetOrdinal("Продолжительность")),
                                DateTimeStop = reader.GetDateTime(reader.GetOrdinal("PlayDateTime")).Add(reader.GetTimeSpan(reader.GetOrdinal("Продолжительность"))),
                                ItemName = reader.GetString(reader.GetOrdinal("ItemName")),
                                ItemType = reader.GetString(reader.GetOrdinal("ItemType"))
                            };
                            resultList.Add(item);
                        }
                    }
                }
            }

            return resultList;
        }

        private void loadGrid()
        {
            try
            {
                int? selectedPresenterId = null;
                if (presenterComboBox.SelectedValue != null)
                {
                    int tmp;
                    if (int.TryParse(presenterComboBox.SelectedValue.ToString(), out tmp) && tmp > 0)
                        selectedPresenterId = tmp;
                }

                DateTime? dateFilter = null;
                if (dateTimePicker1.Checked)
                    dateFilter = dateTimePicker1.Value;

                // вы можете также считать рекламный блок (adBlockComboBox), если хотите фильтровать
                // для плейлистов — другая comboBox?

                // Загружаем
                var scheduleList = LoadSchedule(selectedPresenterId, dateFilter);

                // Отображаем
                dataGridViewShedulePlaylistsAds.DataSource = scheduleList;

                // Доп. настройка столбцов
                if (dataGridViewShedulePlaylistsAds.Columns["RecordId"] != null)
                    dataGridViewShedulePlaylistsAds.Columns["RecordId"].Visible = false;
                if (dataGridViewShedulePlaylistsAds.Columns["PresenterId"] != null)
                    dataGridViewShedulePlaylistsAds.Columns["PresenterId"].Visible = false;

                dataGridViewShedulePlaylistsAds.Columns["PlayDateTime"].HeaderText = "Дата/Время начала";
                dataGridViewShedulePlaylistsAds.Columns["PresenterName"].HeaderText = "Ведущий";
                dataGridViewShedulePlaylistsAds.Columns["ItemName"].HeaderText = "Блок / Плейлист";
                dataGridViewShedulePlaylistsAds.Columns["Duration"].HeaderText = "Продолжительность";
                dataGridViewShedulePlaylistsAds.Columns["DateTimeStop"].HeaderText = "Дата/Время конца";
                dataGridViewShedulePlaylistsAds.Columns["ItemType"].HeaderText = "Тип";
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ошибка SQL: " + ex.Message,
                                "Ошибка",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message,
                                "Ошибка",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Обработчик кнопки "Найти": вызываем LoadSchedule(...) и результат привязываем к dataGridView.
        /// </summary>
        private void search_button_Click(object sender, EventArgs e)
        {
            loadGrid();
        }

        /// <summary>
        /// Класс для объединённого результата (Ad / Playlist)
        /// </summary>
        public class UnifiedScheduleItem
        {
            public int RecordId { get; set; }
            public int PresenterId { get; set; }
            public DateTime PlayDateTime { get; set; }
            public DateTime DateTimeStop { get; set; }
            public TimeSpan Duration { get; set; }

            public string PresenterName { get; set; }
            public string ItemName { get; set; }
            public string ItemType { get; set; } // "Ad" или "Playlist"
        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
