using System.Text;

namespace Maui;

public class MainPage : ContentPage
{
	public MainPage()
	{
        Grid menu = new()
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


        AddButtons(menu);
        Content = menu;
    }

    private void AddButtons(Grid grid)
    {
        for (int i = 0; i < 3; i++)
        {
            int size = (i + 1) * 5;
            String text = $"{size}x{size}";

            Button but = new() { Text = text, Margin = 10, HeightRequest = 50 };

            but.Clicked += async (s, e) =>
            {
                // Change to game page with size x size grid corresponding to button upon click
                await Navigation.PushAsync(new GamePage(size, size));
            };

            grid.Add(but, 1, i);
        }
    }
}