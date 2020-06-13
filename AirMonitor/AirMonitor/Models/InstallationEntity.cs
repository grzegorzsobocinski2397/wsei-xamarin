namespace AirMonitor.Models
{
	/// <summary>
	/// Model for the SQLite database.
	/// </summary>
	public class InstallationEntity
	{
		#region Public Properties

		public string Id { get; set; }
		public string Location { get; set; }
		public string Address { get; set; }
		public double Elevation { get; set; }

		#endregion Public Properties

		#region Constructor

		public InstallationEntity()
		{
		}

		#endregion Constructor
	}
}