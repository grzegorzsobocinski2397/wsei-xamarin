using AirMonitor.Models;
using SQLite;
using System.Collections.Generic;

namespace AirMonitor.Helpers
{
	/// <summary>
	/// Helper for the SQLite database. Used for writing/reading data.
	/// </summary>
	public class DatabaseHelper
	{
		#region Private Fields

		/// <summary>
		/// Representation of SQLite database with basic options like path or name.
		/// </summary>
		private Database Database { get; set; }

		/// <summary>
		/// Connection to the database.
		/// </summary>
		private SQLiteConnection DBConnection { get; set; }

		#endregion Private Fields

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

		/// <summary>
		/// Map the measurements into entites and write them into the database. Wipe data before.
		/// </summary>
		/// <param name="measurements"></param>
		public void SaveMeasurements(IEnumerable<Measurement> measurements)
		{
			DBConnection.RunInTransaction(() =>
			{
				DBConnection.DeleteAll<MeasurementEntity>();
				DBConnection.DeleteAll<MeasurementItemEntity>();
				DBConnection.DeleteAll<MeasurementValue>();
				DBConnection.DeleteAll<AirQualityIndex>();
				DBConnection.DeleteAll<AirQualityStandard>();

				foreach (Measurement measurement in measurements)
				{
					DBConnection.InsertAll(measurement.Current.Values, false);
					DBConnection.InsertAll(measurement.Current.Indexes, false);
					DBConnection.InsertAll(measurement.Current.Standards, false);

					MeasurementItemEntity measurementCurrentEntity = new MeasurementItemEntity(measurement.Current);
					DBConnection.Insert(measurementCurrentEntity);

					MeasurementEntity measurementEntity = new MeasurementEntity(measurement, measurementCurrentEntity);
					DBConnection.Insert(measurementEntity);
				}
			});
		}

		/// <summary>
		/// Read the database for installation entities, map them and return.
		/// </summary>
		public IEnumerable<Installation> ReadInstallations()
		{
			List<InstallationEntity> installationEnitities = DBConnection.Table<InstallationEntity>().ToList();
			List<Installation> installations = new List<Installation>();

			foreach (InstallationEntity installationEntity in installationEnitities)
			{
				installations.Add(new Installation(installationEntity));
			}

			return installations;
		}

		#endregion Public Methods

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