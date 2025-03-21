using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task AddRecordAsync(Record record)
        {
            using (var conn = Init_Conn.GetConnection())
            {
                await conn.OpenAsync();
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

        public async Task UpdateRecord(Record record)
        {
            using (var conn = Init_Conn.GetConnection())
            {
                await conn.OpenAsync();
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

        public async Task DeleteRecord(int id)
        {
            using (var conn = Init_Conn.GetConnection())
            {
                await conn.OpenAsync();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Проверяем наличие бронирований
                        const string checkReservationsSql = @"
                    SELECT COUNT(*) 
                    FROM ReservedRecords 
                    WHERE RecordId = @RecordId 
                    AND IsConfirmed = 1 
                    AND ExpireDate > GETDATE()";

                        using (var cmd = new SqlCommand(checkReservationsSql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@RecordId", id);
                            var count = await cmd.ExecuteScalarAsync();
                            if ((int)count > 0)
                            {
                                throw new InvalidOperationException(
                                    "Нельзя удалить запись, так как на неё есть активные бронирования");
                            }
                        }

                        // Удаляем бронирования
                        const string deleteReservationsSql = @"
                    DELETE FROM ReservedRecords 
                    WHERE RecordId = @RecordId";

                        using (var cmd = new SqlCommand(deleteReservationsSql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@RecordId", id);
                            await cmd.ExecuteNonQueryAsync();
                        }

                        // Удаляем запись
                        const string deleteRecordSql = @"
                    DELETE FROM Records 
                    WHERE Id = @Id";

                        using (var cmd = new SqlCommand(deleteRecordSql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Id", id);
                            await cmd.ExecuteNonQueryAsync();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task SaleRecordAsync(int recordId, int? customerId, decimal price)
        {
            try
            {
                using (var conn = Init_Conn.GetConnection())
                {
                    await conn.OpenAsync();

                    if (conn.State != ConnectionState.Open)
                    {
                        throw new Exception("Ошибка соединения с базой данных");
                    }

                    // Проверяем наличие записи
                    const string checkSql = "SELECT COUNT(*) FROM Records WHERE Id = @Id";
                    using (var cmd = new SqlCommand(checkSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", recordId);
                        var count = await cmd.ExecuteScalarAsync();
                        if ((int)count == 0)
                            throw new InvalidOperationException("Запись не найдена");
                    }

                    // Начинаем транзакцию
                    using (var transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            // Добавляем продажу
                            const string saleSql = @"
                    INSERT INTO Sales (RecordId, CustomerName, Price)
                    VALUES (@RecordId, @CustomerName, @Price)";
                            using (var cmd = new SqlCommand(saleSql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@RecordId", recordId);
                                cmd.Parameters.AddWithValue("@CustomerName", Properties.Settings.Default.CurrentUserLogin);
                                cmd.Parameters.AddWithValue("@Price", price);
                                await cmd.ExecuteNonQueryAsync();
                            }

                            // Обновляем сумму потраченных средств и скидку для постоянного покупателя
                            if (!string.IsNullOrEmpty(Properties.Settings.Default.CurrentUserLogin))
                            {
                                await UpdateCustomerDiscountAsync(conn, transaction, price);
                            }

                            // Подтверждаем транзакцию
                            transaction.Commit();
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Ошибка при сохранении продажи: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при сохранении продажи: {ex.Message}");
            }
        }

        private async Task UpdateCustomerDiscountAsync(SqlConnection conn, SqlTransaction transaction, decimal amount)
        {
            const string sql = @"
                                UPDATE cd
                                SET cd.TotalSpent = cd.TotalSpent + @Amount,
                                cd.DiscountPercentage = 
                                    CASE 
                                        WHEN cd.TotalSpent + @Amount >= 10000 THEN 15.00
                                        WHEN cd.TotalSpent + @Amount >= 5000 THEN 10.00
                                        WHEN cd.TotalSpent + @Amount >= 1000 THEN 5.00
                                        ELSE cd.DiscountPercentage
                                    END
                            FROM CustomerDiscounts cd
                            WHERE cd.CustomerId = (SELECT Id FROM Users WHERE Login = @Login)";

            using (var cmd = new SqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@Login", Properties.Settings.Default.CurrentUserLogin);
                cmd.Parameters.AddWithValue("@Amount", amount);
                await cmd.ExecuteNonQueryAsync();
            }
        }


        public async Task<decimal> GetCustomerDiscountAsync(int customerId)
        {
            using (var conn = Init_Conn.GetConnection())
            {
                await conn.OpenAsync();
                const string sql = "SELECT DiscountPercentage FROM CustomerDiscounts WHERE CustomerId = @CustomerId";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    var result = await cmd.ExecuteScalarAsync();
                    return result == null ? 0 : Convert.ToDecimal(result);
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

        public async Task ReserveRecordAsync(int recordId, string customerName, DateTime expireDate)
        {
            using (var conn = Init_Conn.GetConnection())
            {
                await conn.OpenAsync();

                // Проверяем наличие записи и её доступность
                const string checkSql = @"
                SELECT COUNT(*) FROM Records 
                WHERE Id = @Id AND Id NOT IN (
                    SELECT RecordId FROM ReservedRecords 
                    WHERE IsConfirmed = 1 AND ExpireDate > GETDATE()
                )";
                using (var cmd = new SqlCommand(checkSql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", recordId);
                    var count = await cmd.ExecuteScalarAsync();
                    if ((int)count == 0)
                        throw new InvalidOperationException("Запись недоступна для бронирования");
                }

                // Создаем бронирование
                const string reserveSql = @"
                INSERT INTO ReservedRecords (RecordId, CustomerName, ExpireDate)
                VALUES (@RecordId, @CustomerName, @ExpireDate)";
                using (var cmd = new SqlCommand(reserveSql, conn))
                {
                    cmd.Parameters.AddWithValue("@RecordId", recordId);
                    cmd.Parameters.AddWithValue("@CustomerName", customerName);
                    cmd.Parameters.AddWithValue("@ExpireDate", expireDate);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Record>> SearchRecordsAsync(string title, string artist, string genre)
        {
            using (var conn = Init_Conn.GetConnection())
            {
                await conn.OpenAsync();
                const string sql = @"SELECT Id, Title, Artist, Publisher, TrackCount, 
                                     Genre, ReleaseYear, CostPrice, SellingPrice 
                                     FROM Records 
                                     WHERE (@Title IS NULL OR Title LIKE @TitlePattern)
                                     AND (@Artist IS NULL OR Artist LIKE @ArtistPattern)
                                     AND (@Genre IS NULL OR Genre LIKE @GenrePattern)
                                     ORDER BY Title";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    // Добавляем параметры с шаблонами поиска
                    cmd.Parameters.AddWithValue("@Title", string.IsNullOrEmpty(title) ? (object)DBNull.Value : title);
                    cmd.Parameters.AddWithValue("@TitlePattern", string.IsNullOrEmpty(title) ? (object)DBNull.Value : $"%{title}%");
                    cmd.Parameters.AddWithValue("@Artist", string.IsNullOrEmpty(artist) ? (object)DBNull.Value : artist);
                    cmd.Parameters.AddWithValue("@ArtistPattern", string.IsNullOrEmpty(artist) ? (object)DBNull.Value : $"%{artist}%");
                    cmd.Parameters.AddWithValue("@Genre", string.IsNullOrEmpty(genre) ? (object)DBNull.Value : genre);
                    cmd.Parameters.AddWithValue("@GenrePattern", string.IsNullOrEmpty(genre) ? (object)DBNull.Value : $"%{genre}%");

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var records = new List<Record>();
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
                        return records;
                    }
                }
            }
        }
    }
}
