using AirMonitor.Models;
using SQLite;
using System.Collections;
using System.Collections.Generic;

namespace AirMonitor.Helpers
{
	/// <summary>
	/// Helper for the SQLite database. Used for writing/reading data.
	/// </summary>
	public class DatabaseHelper
	{
		#region Private Fields

		private Database Database { get; set; }

		#endregion Private Fields

		private SQLiteConnection DBConnection { get; set; }

		#region Constructor

		public DatabaseHelper()
		{
			Database = new Database("AirlyDB");
			InitializeDatabase();
		}

		#endregion Constructor

		#region Public Methods
		/// <summary>
		/// Map the existing installations and save them in the database. Erase all table data before.
		/// </summary>
		/// <param name="installations">Current installations of Airly sensors.</param>
		public void SaveInstallations(IEnumerable<Installation> installations)
		{
			List<InstallationEntity> installationEntities = new List<InstallationEntity>();

			foreach (Installation installation in installations)
			{
				InstallationEntity installationEnitity = new InstallationEntity(installation);
				installationEntities.Add(installationEnitity);
			}


			DBConnection.RunInTransaction(() =>
			{
				DBConnection.DeleteAll<InstallationEntity>();
				DBConnection.InsertAll(installationEntities);
			}
			);
		}
		#endregion

		#region Private Methods

		/// <summary>
		/// Connect to database and create tables.
		/// </summary>
		private void InitializeDatabase()
		{
			DBConnection = new SQLiteConnection(Database.Path, Database.Flags);

			DBConnection.CreateTable<InstallationEntity>();
			DBConnection.CreateTable<MeasurementEntity>();
			DBConnection.CreateTable<MeasurementItemEntity>();
			DBConnection.CreateTable<MeasurementValue>();
			DBConnection.CreateTable<AirQualityIndex>();
			DBConnection.CreateTable<AirQualityStandard>();
		}

		#endregion Private Methods
	}
}