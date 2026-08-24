using NonoSharp;
using NonoSharp.Maui.Data;
using System.Diagnostics;
using System.Text;

namespace NonoSharp.Maui;

public partial class MainPage : ThemedPage
{
    private readonly Grid menu;
    private readonly ActivityIndicator indicator;

    public MainPage() : base()
    {
        menu = new()
        {
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Fill,

            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            },

            RowDefinitions =
            {
                new RowDefinition(),
                new RowDefinition(),
                new RowDefinition(),
                new RowDefinition()
            }
        };

        indicator = new() {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        AddButtons(menu);
        menu.Add(indicator);

        // Set row and column span to ensure the indicator is centered
        menu.SetColumnSpan(indicator, menu.ColumnDefinitions.Count);
        menu.SetRowSpan(indicator, menu.RowDefinitions.Count);

        SemanticProperties.SetDescription(indicator, "Loading");

        // Set theme in case of mismatch between user selected theme and system theme
        UpdateTheme();

        Content = menu;
    }

    private void AddButtons(Grid grid)
    {
        int margin = 10;
        int height = 50;

        Button premadePuzzles = new() { Text = "Puzzles", Margin=margin, HeightRequest= height };
        premadePuzzles.Clicked += async (s, e) =>
        {
            await Navigation.PushAsync(new SelectionPage());
        };
        grid.Add(premadePuzzles, 1, 0);

        // Create 3 buttons, 5x5, 10x10, 15x15
        for (int i = 0; i < 3; i++)
        {
            int size = (i + 1) * 5;
            String text = $"Random {size}x{size}";

            Button but = new() { Text = text, Margin = margin, HeightRequest = height };

            but.Clicked += async (s, e) =>
            {
                await OnGeneratePuzzleButtonClicked(size);
            };

            grid.Add(but, 1, i+1);
        }

        Button settings = new() { Text = "Settings", Margin = margin, HeightRequest = height };
        settings.Clicked += async (s, e) =>
        {
            await Navigation.PushAsync(new SettingsPage());
        };

        grid.Add(settings, 1, 4);
    }

    private async Task OnGeneratePuzzleButtonClicked(int size)
    {
        try
        {
            // Disable the buttons
            menu.IsEnabled = false;
            indicator.IsRunning = true;
            indicator.IsEnabled = true;
            // Change to game page with size x size grid corresponding to button upon click
            NonogramAPI api = await NonoSharp.NonogramAPI.CreateRandomPuzzleAsync(size, size);
            await Navigation.PushAsync(new GamePage(api));
        }
        finally
        {
            indicator.IsRunning = false;
            indicator.IsEnabled = false;
            // Ensure the menu is enabled and buttons can be pressed once the user returns
            menu.IsEnabled = true;
        }
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (args.PreviousPage is SettingsPage)
        {
            // Update the theme in case it was changed in the settings
            UpdateTheme();
        }

        base.OnNavigatedTo(args);
    }
}