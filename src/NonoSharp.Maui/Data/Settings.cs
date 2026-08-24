using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace NonoSharp.Maui.Data
{
    public class Settings
    {

        [PrimaryKey]
        public int ID { get; set; } = 1; // Settings is a single-row database, set property must be present otherwise an error is thrown when updating

        public AppTheme Theme { get; set; } = AppTheme.Unspecified;
    }
}
