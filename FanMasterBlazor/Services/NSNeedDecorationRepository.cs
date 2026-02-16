using FanMasterBlazor.Data;
using Microsoft.Data.SqlClient;

namespace FanMasterBlazor.Services;

public class NSNeedDecorationRepository(ConnectionState connectionState)
{
    public async Task<List<NSNeedDecorationRecord>> GetAllAsync()
    {
        const string sql = """
            SELECT NSNeedDecorationID, ForecastYear, ForecastMonth, Category, Program, ProductType,
                   VendorStream, Units, SourceFileName, SourceAsOfDate, LoadTimestampUTC
            FROM dbo.NSNeedDecoration
            ORDER BY ForecastYear DESC, ForecastMonth DESC, NSNeedDecorationID DESC;
            """;

        var results = new List<NSNeedDecorationRecord>();
        await using var conn = new SqlConnection(connectionState.BuildConnectionString());
        await conn.OpenAsync();
        await using var cmd = new SqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(new NSNeedDecorationRecord
            {
                NSNeedDecorationID = reader.GetInt64(0),
                ForecastYear = reader.GetInt16(1),
                ForecastMonth = reader.GetByte(2),
                Category = reader.GetString(3),
                Program = reader.GetString(4),
                ProductType = reader.GetString(5),
                VendorStream = reader.GetString(6),
                Units = reader.GetInt32(7),
                SourceFileName = reader.IsDBNull(8) ? null : reader.GetString(8),
                SourceAsOfDate = reader.IsDBNull(9) ? null : DateOnly.FromDateTime(reader.GetDateTime(9)),
                LoadTimestampUTC = reader.GetDateTime(10)
            });
        }

        return results;
    }

    public async Task<long> InsertAsync(NSNeedDecorationRecord item)
    {
        const string sql = """
            INSERT INTO dbo.NSNeedDecoration (
                ForecastYear, ForecastMonth, Category, Program, ProductType,
                VendorStream, Units, SourceFileName, SourceAsOfDate
            )
            OUTPUT INSERTED.NSNeedDecorationID
            VALUES (
                @ForecastYear, @ForecastMonth, @Category, @Program, @ProductType,
                @VendorStream, @Units, @SourceFileName, @SourceAsOfDate
            );
            """;

        await using var conn = new SqlConnection(connectionState.BuildConnectionString());
        await conn.OpenAsync();
        await using var cmd = new SqlCommand(sql, conn);
        AddCommonParameters(cmd, item);
        var id = await cmd.ExecuteScalarAsync();
        return Convert.ToInt64(id);
    }

    public async Task UpdateAsync(NSNeedDecorationRecord item)
    {
        const string sql = """
            UPDATE dbo.NSNeedDecoration
            SET ForecastYear = @ForecastYear,
                ForecastMonth = @ForecastMonth,
                Category = @Category,
                Program = @Program,
                ProductType = @ProductType,
                VendorStream = @VendorStream,
                Units = @Units,
                SourceFileName = @SourceFileName,
                SourceAsOfDate = @SourceAsOfDate
            WHERE NSNeedDecorationID = @NSNeedDecorationID;
            """;

        await using var conn = new SqlConnection(connectionState.BuildConnectionString());
        await conn.OpenAsync();
        await using var cmd = new SqlCommand(sql, conn);
        AddCommonParameters(cmd, item);
        cmd.Parameters.AddWithValue("@NSNeedDecorationID", item.NSNeedDecorationID);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(long id)
    {
        const string sql = "DELETE FROM dbo.NSNeedDecoration WHERE NSNeedDecorationID = @id;";

        await using var conn = new SqlConnection(connectionState.BuildConnectionString());
        await conn.OpenAsync();
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        await cmd.ExecuteNonQueryAsync();
    }

    private static void AddCommonParameters(SqlCommand cmd, NSNeedDecorationRecord item)
    {
        cmd.Parameters.AddWithValue("@ForecastYear", item.ForecastYear);
        cmd.Parameters.AddWithValue("@ForecastMonth", item.ForecastMonth);
        cmd.Parameters.AddWithValue("@Category", item.Category);
        cmd.Parameters.AddWithValue("@Program", item.Program);
        cmd.Parameters.AddWithValue("@ProductType", item.ProductType);
        cmd.Parameters.AddWithValue("@VendorStream", item.VendorStream);
        cmd.Parameters.AddWithValue("@Units", item.Units);
        cmd.Parameters.AddWithValue("@SourceFileName", (object?)item.SourceFileName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@SourceAsOfDate", item.SourceAsOfDate?.ToDateTime(TimeOnly.MinValue) ?? (object)DBNull.Value);
    }
}
