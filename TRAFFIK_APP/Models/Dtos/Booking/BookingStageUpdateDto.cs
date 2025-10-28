namespace TRAFFIK_APP.Models.Dtos.Booking
{
	public class BookingStageUpdateDto
	{
		public int Id { get; set; }
		public int BookingId { get; set; }
		public int VehicleId { get; set; }
		public string CurrentStage { get; set; } = "Pending";
		public List<string> AvailableStages { get; set; } = new()
		{
			"Started", "Inspection", "Completed", "Paid"
		};
		public string SelectedStage { get; set; } = string.Empty;
		public bool IsConfirmed { get; set; }
		public DateTime UpdatedAt { get; set; }
		public string UpdatedBy { get; set; } = string.Empty;
	}
}