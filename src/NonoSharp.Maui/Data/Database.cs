using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Picross.Maui.Data
{
    public class Database
    {
        internal const string DatabaseFilename = "PicrossData.db3";

        internal const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

        internal static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        private SQLiteAsyncConnection? _con = null; 

        private async Task Init()
        {
            if (_con is not null)
                return;

            // Setup connection and create necessary tables
            _con = new SQLiteAsyncConnection(DatabasePath, Flags);
            await _con.CreateTableAsync<Settings>();
        }

        internal async Task<Settings> GetSettingsAsync() {
            await Init();

            Settings? settings = await _con!.Table<Settings>().FirstOrDefaultAsync();
            
            if (settings == null)
            {
                // Return default settings as the database has not written any yet
                // That means that the user has not changed any settings yet
                return new Settings();
            }

            return settings;
        }

        internal async Task<int> SaveSettingsAsync(Settings toSave)
        {
            await Init();

            if (await _con!.Table<Settings>().FirstOrDefaultAsync() != null)
            {
                return await _con!.UpdateAsync(toSave);
            }
            return await _con!.InsertAsync(toSave);
        }
    }
}
