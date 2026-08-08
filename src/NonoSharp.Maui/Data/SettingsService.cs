using System;
using System.Collections.Generic;
using System.Text;

namespace Picross.Maui.Data
{
    public class SettingsService
    {
        private readonly Database _db;
        public static Settings CurrentSettings { get; private set; } = new();
        
        public SettingsService(Database db) {
            _db = db; 
        }

        public static SettingsService GetService()
        {
            return IPlatformApplication.Current!.Services.GetRequiredService<SettingsService>();
        }

        public async Task InitializeAsync()
        {
            CurrentSettings = await _db.GetSettingsAsync();
        }

        public async Task SaveSettingsAsync()
        {
            await _db.SaveSettingsAsync(CurrentSettings);
        }
    }
}
