using NonoSharp.Maui.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace NonoSharp.Maui
{
    public partial class SettingsPage : ThemedPage
    {
        private static readonly List<(AppTheme Theme, string SettingsName)> themesSelection =
        [
            (AppTheme.Unspecified, "Use System Theme"),
            (AppTheme.Light, "Light"),
            (AppTheme.Dark, "Dark")
        ];

        private SettingsService settingsService;
        private Settings settings;
        private Picker themePicker;
        
        public SettingsPage()
        {
            settingsService = SettingsService.GetService();
            settings = SettingsService.CurrentSettings;

            Grid menu = new()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Fill,

                ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            },

                RowDefinitions =
            {
                new RowDefinition(),
                new RowDefinition()
            }
            };

            themePicker = new()
            {
                Title = "Select a theme",

                // Select the names of themes settings
                ItemsSource = themesSelection.Select(t => t.SettingsName).ToList(),
                TitleColor = Theme.PrimaryText
            };

            menu.Add(themePicker, 1, 0);

            themePicker.SelectedIndexChanged += async (s, e) => await ThemePicker_OnSelectedIndexChangedAsync(s, e);

            Content = menu;
        }

        private async Task ThemePicker_OnSelectedIndexChangedAsync(object? sender, EventArgs e)
        {
            if (sender == null) return;

            Picker picker = (Picker)sender;
            int selected = picker.SelectedIndex;

            if (selected == -1) return;

            AppTheme chosen = themesSelection[selected].Theme;
            settings.Theme = chosen;
            UpdateTheme();
            await settingsService.SaveSettingsAsync();
        }

        protected override void UpdateTheme()
        {
            themePicker.TitleColor = Theme.PrimaryText;
            base.UpdateTheme();
        }
    }
}