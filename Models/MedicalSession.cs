namespace ClinicAppointmentAPI.Models
{
	public class MedicalSession
	{
		public int Id { get; set; }

		public string Title { get; set; } = "";

		public string Description { get; set; } = "";

		public string RoomNumber { get; set; } = "";

		public DateTime SessionDate { get; set; }

		public int MaxPatients { get; set; }
	}
}