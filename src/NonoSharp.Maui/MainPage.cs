using Picross.Game;
using System.Text;

namespace Picross.Maui;

public class MainPage : ThemedPage
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
                new RowDefinition()
            }
        };

        indicator = new() {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        // Set row and column span to ensure the indicator is centered
        menu.SetColumnSpan(indicator, menu.ColumnDefinitions.Count);
        menu.SetRowSpan(indicator, menu.RowDefinitions.Count);

        SemanticProperties.SetDescription(indicator, "Loading");

        AddButtons(menu);
        menu.Add(indicator);

        Content = menu;
    }

    private void AddButtons(Grid grid)
    {
        // Create 3 buttons, 5x5, 10x10, 15x15
        for (int i = 0; i < 3; i++)
        {
            int size = (i + 1) * 5;
            String text = $"{size}x{size}";

            Button but = new() { Text = text, Margin = 10, HeightRequest = 50 };

            but.Clicked += async (s, e) =>
            {
                await OnGeneratePuzzleButtonClicked(size);
            };

            grid.Add(but, 1, i);
        }
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
            GameAPI api = await GameAPI.CreateRandomPuzzle(size, size);
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
}