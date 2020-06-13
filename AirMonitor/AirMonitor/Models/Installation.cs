using Newtonsoft.Json;
using Xamarin.Essentials;

namespace AirMonitor.Models
{
	/// <summary>
	/// Representation of an airly station.
	/// </summary>
	public class Installation
	{
		#region Public Properties

		public string Id { get; set; }
		public Location Location { get; set; }
		public Address Address { get; set; }
		public double Elevation { get; set; }

		#endregion Public Properties

		#region Constructor

		/// <summary>
		/// Deserialize entity from the SQLite database.
		/// </summary>
		public Installation(InstallationEntity installationEntity)
		{
			Id = installationEntity.Id;
			Location = JsonConvert.DeserializeObject<Location>(installationEntity.Location);
			Address = JsonConvert.DeserializeObject<Address>(installationEntity.Address);
			Elevation = installationEntity.Elevation;
		}

		#endregion Constructor
	}
}