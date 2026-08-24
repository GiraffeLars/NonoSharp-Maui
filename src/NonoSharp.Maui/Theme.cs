using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Graphics;
using NonoSharp.Maui.Data;

namespace NonoSharp.Maui
{
    internal class Theme
    {
        private static bool IsDarkMode()
        {
            Settings settings = SettingsService.CurrentSettings;

            if (settings.Theme == AppTheme.Unspecified)
            {
                AppTheme systemTheme = Application.Current!.RequestedTheme;
                return systemTheme == AppTheme.Dark;
            }

            return settings.Theme == AppTheme.Dark;
        }

        public static Color FilledCell => IsDarkMode() ? Colors.LightGray : Colors.Black;
        public static Color SolvedCell => Colors.Green;
        public static Color GridLine => Colors.Gray;
        public static Color CrossColor => IsDarkMode() ? Colors.LightGray : Colors.Black;
        public static Color IncompleteHint => IsDarkMode() ? Colors.White : Colors.Black;
        public static Color CompletedHint => Colors.Gray;
        public static Color BackgroundColor => IsDarkMode() ? GetBlackBackground() : Color.FromArgb("#F2F0EF"); // Off-white, grayish
        public static Color PrimaryText => IsDarkMode() ? Colors.White : Colors.Black;

        private static Color GetBlackBackground()
        {
            var hasValue = Application.Current!.Resources.TryGetValue("OffBlack", out object blackColor);

            if (hasValue)
            {
                return (Color)blackColor;
            }
            return Colors.Black;
        }
    }
}
