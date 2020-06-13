﻿namespace AirMonitor.Models
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

		/// <summary>
		/// Map the <see cref="Installation"/> to an entity applicable for SQLite database.
		/// </summary>
		/// <param name="installation"></param>
		public InstallationEntity(Installation installation)
		{
			Id = installation.Id;
			Location = installation.Location.ToString();
			Address = installation.Address.ToString();
			Elevation = installation.Elevation;
		}

		#endregion Constructor
	}
}