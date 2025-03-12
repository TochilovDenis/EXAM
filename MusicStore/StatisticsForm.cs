using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicStore
{
    public partial class StatisticsForm : Form
    {
        private readonly RecordRepository _repository;
        private readonly BindingSource bindingSource;

        public StatisticsForm()
        {
            InitializeComponent();
            _repository = new RecordRepository();
            bindingSource = new BindingSource();

            // Заполняем комбобокс значениями
            cbTimePeriod.Items.AddRange(new string[]
            {
                "День",
                "Неделя",
                "Месяц",
                "Год"
            });
            cbTimePeriod.SelectedIndex = 0; // Устанавливаем значение по умолчанию
        }

        private async void btnShowStats_Click_1(object sender, EventArgs e)
        {
            try
            {
                string timePeriod = cbTimePeriod.SelectedItem?.ToString() ?? "День";
                var stats = await GetStatisticsAsync(timePeriod);
                bindingSource.DataSource = stats;
                dgvStatistics.DataSource = bindingSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статистики: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<List<StatisticsItem>> GetStatisticsAsync(string timePeriod)
        {
            var stats = new List<StatisticsItem>();
            using (var conn = Init_Conn.GetConnection())
            {
                await conn.OpenAsync();
                var startDate = GetStartDate(timePeriod);
                var endDate = DateTime.Now;

                // Получаем новинки
                const string newReleasesSql = @"
                SELECT TOP 10 Title, Artist, ReleaseYear 
                FROM Records 
                WHERE ReleaseYear >= YEAR(@StartDate)
                ORDER BY ReleaseYear DESC, Title";

                using (var cmd = new SqlCommand(newReleasesSql, conn))
                {
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            stats.Add(new StatisticsItem
                            {
                                Category = "Новинки",
                                Title = reader.GetString(0),
                                Artist = reader.GetString(1),
                                Year = reader.GetInt32(2)
                            });
                        }
                    }
                }

                // Получаем самые продаваемые
                const string bestSellersSql = @"
                SELECT TOP 10 r.Title, r.Artist, COUNT(s.Id) as SalesCount
                FROM Records r
                JOIN Sales s ON r.Id = s.RecordId
                WHERE s.SaleDate >= @StartDate
                GROUP BY r.Title, r.Artist
                ORDER BY SalesCount DESC";

                using (var cmd = new SqlCommand(bestSellersSql, conn))
                {
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            stats.Add(new StatisticsItem
                            {
                                Category = "Продажи",
                                Title = reader.GetString(0),
                                Artist = reader.GetString(1),
                                SalesCount = reader.GetInt32(2)
                            });
                        }
                    }
                }

                // Получаем популярные авторы
                const string popularArtistsSql = @"
                SELECT TOP 10 Artist, COUNT(s.Id) as SalesCount
                FROM Records r
                JOIN Sales s ON r.Id = s.RecordId
                WHERE s.SaleDate >= @StartDate
                GROUP BY Artist
                ORDER BY SalesCount DESC";

                using (var cmd = new SqlCommand(popularArtistsSql, conn))
                {
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            stats.Add(new StatisticsItem
                            {
                                Category = "Авторы",
                                Artist = reader.GetString(0),
                                SalesCount = reader.GetInt32(1)
                            });
                        }
                    }
                }

                // Получаем популярные жанры
                const string popularGenresSql = @"
                SELECT TOP 10 Genre, COUNT(s.Id) as SalesCount
                FROM Records r
                JOIN Sales s ON r.Id = s.RecordId
                WHERE s.SaleDate >= @StartDate
                GROUP BY Genre
                ORDER BY SalesCount DESC";

                using (var cmd = new SqlCommand(popularGenresSql, conn))
                {
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            stats.Add(new StatisticsItem
                            {
                                Category = "Жанры",
                                Genre = reader.GetString(0),
                                SalesCount = reader.GetInt32(1)
                            });
                        }
                    }
                }
            }
            return stats;
        }

        private DateTime GetStartDate(string timePeriod)
        {
            switch (timePeriod)
            {
                case "День":
                    return DateTime.Now.Date;
                case "Неделя":
                    return DateTime.Now.Date.AddDays(-7);
                case "Месяц":
                    return DateTime.Now.Date.AddMonths(-1);
                case "Год":
                    return DateTime.Now.Date.AddYears(-1);
                default:
                    return DateTime.Now.Date;
            }
        }
        public class StatisticsItem
        {
            public string Category { get; set; }
            public string Title { get; set; }
            public string Artist { get; set; }
            public string Genre { get; set; }
            public int Year { get; set; }
            public int SalesCount { get; set; }

        }
    }
}
