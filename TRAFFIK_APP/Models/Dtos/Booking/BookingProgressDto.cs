namespace TRAFFIK_APP.Models
{
    public class BookingProgressDto
    {
        public int BookingId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public string CurrentStage { get; set; } = "Pending";
        public DateTime? CompletionDate { get; set; }

        public List<string> StageSequence { get; set; } = new()
        {
             "Wash started", "Wash done", "Dry started", "Dry done", "Service completed"
        };
        public double ProgressFraction => StageSequence.IndexOf(CurrentStage) switch
        {
            var i when i >= 0 => (double)(i + 1) / StageSequence.Count,
            _ => 0
        };

        public bool IsCompleted => CurrentStage == "Service completed";
    }
}


