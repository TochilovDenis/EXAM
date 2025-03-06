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
    }
}
