public class BookingUpdateDto
{
    public DateTime ScheduledDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public bool IsConfirmed { get; set; }
}