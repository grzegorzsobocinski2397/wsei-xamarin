using SQLite;
using System;

namespace AirMonitor.Models
{
	/// <summary>
	/// Representation of SQLite database with basic options like path or name.
	/// </summary>
	public class Database
	{
		#region Public Properties

		/// <summary>
		/// Name of the database, which is also the file name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Directory in which the database is stored.
		/// </summary>
		public string Directory { get; private set; }

		/// <summary>
		/// Full path to the database.
		/// </summary>
		public string Path { get; private set; }

		/// <summary>
		/// Flags used to avoid problems with multithreading.
		/// </summary>
		public SQLiteOpenFlags Flags { get; private set; } = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex;

		#endregion Public Properties

		#region Constructor

		public Database(string name)
		{
			Name = name;
			Directory = Environment.CurrentDirectory;
			Path = System.IO.Path.Combine(Directory, name);
		}

		#endregion Constructor
	}
}