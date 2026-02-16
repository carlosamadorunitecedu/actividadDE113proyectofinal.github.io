namespace FanMasterBlazor.Data;

public class NSNeedDecorationRecord
{
    public long NSNeedDecorationID { get; set; }

    [Range(2000, 2100, ErrorMessage = "El año debe estar entre 2000 y 2100.")]
    public short ForecastYear { get; set; }

    [Range(1, 12, ErrorMessage = "El mes debe estar entre 1 y 12.")]
    public byte ForecastMonth { get; set; }

    [Required, StringLength(50)]
    public string Category { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Program { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string ProductType { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string VendorStream { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Units no puede ser negativo.")]
    public int Units { get; set; }

    [StringLength(255)]
    public string? SourceFileName { get; set; }

    public DateOnly? SourceAsOfDate { get; set; }

    public DateTime LoadTimestampUTC { get; set; }
}
