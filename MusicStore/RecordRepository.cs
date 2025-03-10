using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MusicStore
{
    public class RecordRepository
    {
        public async Task<List<Record>> GetAllRecordsAsync()
        {
            var records = new List<Record>();
            using (var conn = Init_Conn.GetConnection())
            {
                await conn.OpenAsync();
                const string sql = @"SELECT Id, Title, Artist, Publisher, TrackCount, 
                           Genre, ReleaseYear, CostPrice, SellingPrice 
                           FROM Records ORDER BY Title";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            records.Add(new Record
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Artist = reader.GetString(2),
                                Publisher = reader.GetString(3),
                                TrackCount = reader.GetInt32(4),
                                Genre = reader.GetString(5),
                                ReleaseYear = reader.GetInt32(6),
                                CostPrice = reader.GetDecimal(7),
                                SellingPrice = reader.GetDecimal(8)
                            });
                        }
                    }
                }
            }
            return records;
        }

        public void AddRecord(Record record)
        {
            using (var conn = Init_Conn.GetConnection())
            {
                conn.Open();
                const string sql = @"INSERT INTO Records (Title, Artist, Publisher, TrackCount, 
                           Genre, ReleaseYear, CostPrice, SellingPrice)
                           VALUES (@Title, @Artist, @Publisher, @TrackCount, 
                                  @Genre, @ReleaseYear, @CostPrice, @SellingPrice)";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", record.Title);
                    cmd.Parameters.AddWithValue("@Artist", record.Artist);
                    cmd.Parameters.AddWithValue("@Publisher", record.Publisher);
                    cmd.Parameters.AddWithValue("@TrackCount", record.TrackCount);
                    cmd.Parameters.AddWithValue("@Genre", record.Genre);
                    cmd.Parameters.AddWithValue("@ReleaseYear", record.ReleaseYear);
                    cmd.Parameters.AddWithValue("@CostPrice", record.CostPrice);
                    cmd.Parameters.AddWithValue("@SellingPrice", record.SellingPrice);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateRecord(Record record)
        {
            using (var conn = Init_Conn.GetConnection())
            {
                conn.Open();
                const string sql = @"UPDATE Records SET 
                               Title = @Title,
                               Artist = @Artist,
                               Publisher = @Publisher,
                               TrackCount = @TrackCount,
                               Genre = @Genre,
                               ReleaseYear = @ReleaseYear,
                               CostPrice = @CostPrice,
                               SellingPrice = @SellingPrice
                               WHERE Id = @Id";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", record.Id);
                    cmd.Parameters.AddWithValue("@Title", record.Title);
                    cmd.Parameters.AddWithValue("@Artist", record.Artist);
                    cmd.Parameters.AddWithValue("@Publisher", record.Publisher);
                    cmd.Parameters.AddWithValue("@TrackCount", record.TrackCount);
                    cmd.Parameters.AddWithValue("@Genre", record.Genre);
                    cmd.Parameters.AddWithValue("@ReleaseYear", record.ReleaseYear);
                    cmd.Parameters.AddWithValue("@CostPrice", record.CostPrice);
                    cmd.Parameters.AddWithValue("@SellingPrice", record.SellingPrice);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteRecord(int id)
        {
            using (var conn = Init_Conn.GetConnection())
            {
                conn.Open();
                const string sql = @"DELETE FROM Records WHERE Id = @Id";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public async Task SaleRecordAsync(int recordId, string customerName, decimal price)
        {
            using (var conn = Init_Conn.GetConnection())
            {
                await conn.OpenAsync();

                // Проверяем наличие записи
                const string checkSql = "SELECT COUNT(*) FROM Records WHERE Id = @Id";
                using (var cmd = new SqlCommand(checkSql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", recordId);
                    var count = await cmd.ExecuteScalarAsync();
                    if ((int)count == 0)
                        throw new InvalidOperationException("Запись не найдена");
                }

                // Добавляем продажу
                const string saleSql = @"
                INSERT INTO Sales (RecordId, CustomerName, Price)
                VALUES (@RecordId, @CustomerName, @Price)";
                using (var cmd = new SqlCommand(saleSql, conn))
                {
                    cmd.Parameters.AddWithValue("@RecordId", recordId);
                    cmd.Parameters.AddWithValue("@CustomerName", customerName);
                    cmd.Parameters.AddWithValue("@Price", price);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

        }

        public async Task AddStockOperationAsync(int recordId, string operationType, int quantity, string reason)
        {
            using (var conn = Init_Conn.GetConnection())
            {
                await conn.OpenAsync();

                const string sql = @"
                INSERT INTO StockOperations (RecordId, OperationType, Quantity, Reason)
                VALUES (@RecordId, @OperationType, @Quantity, @Reason)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@RecordId", recordId);
                    cmd.Parameters.AddWithValue("@OperationType", operationType);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@Reason", reason);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task CreatePromotionAsync(int recordId, DateTime startDate,
                                         DateTime endDate, decimal discountPercentage, string description)
        {
            using (var conn = Init_Conn.GetConnection())
            {
                await conn.OpenAsync();

                const string sql = @"
                INSERT INTO Promotions (RecordId, StartDate, EndDate, DiscountPercentage, Description)
                VALUES (@RecordId, @StartDate, @EndDate, @DiscountPercentage, @Description)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@RecordId", recordId);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);
                    cmd.Parameters.AddWithValue("@DiscountPercentage", discountPercentage);
                    cmd.Parameters.AddWithValue("@Description", description);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


    }
}
