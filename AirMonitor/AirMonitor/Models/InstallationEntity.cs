namespace AirMonitor.Models
{
	/// <summary>
	/// Model for the SQLite database.
	/// </summary>
	public struct InstallationEntity
	{
		public string Id { get; set; }
		public string Location { get; set; }
		public string Address { get; set; }
		public double Elevation { get; set; }
	}
}
