using NonoSharp;

namespace NonoSharp.Maui
{
    internal partial class SelectionPage : ThemedPage
    {
        private int availablePuzzles;
        private Grid menu;

        internal SelectionPage()
        {
            menu = new()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Fill,

                ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            },

                RowDefinitions =
            {
                new RowDefinition() {Height = new GridLength(1, GridUnitType.Star)},
                new RowDefinition() {Height = new GridLength(2, GridUnitType.Star)},
                new RowDefinition() {Height = new GridLength(2, GridUnitType.Star)},
                new RowDefinition() {Height = new GridLength(2, GridUnitType.Star)},
                new RowDefinition() {Height = new GridLength(2, GridUnitType.Star)},
                new RowDefinition() {Height = new GridLength(2, GridUnitType.Star)},
                new RowDefinition() {Height = new GridLength(1, GridUnitType.Star)}
            }
            };

            Content = menu;
        }

        protected override async void OnAppearing()
        {
            try
            {
                availablePuzzles = await PuzzleLibrary.GetPuzzleTotalAsync();
            } catch (Exception)
            {
                await DisplayAlertAsync("Puzzle fetching failed",
                    "Failed to fetch available puzzles. Returning to previous page.",
                    "OK");
                await Navigation.PopAsync();
                return;
            }

            await SetupButtons();
        }

        private async Task SetupButtons()
        {
            for (int i = 0; i < availablePuzzles; i++)
            {
                Button button = new()
                {
                    Text = $"{i + 1}",
                    CommandParameter = i,
                    HeightRequest = 50
                };
                button.Clicked += async (s, e) =>
                {
                    try
                    {
                        using Stream stream = await GetPuzzleStreamAsync((int)button.CommandParameter);
                        await Navigation.PushAsync(new GamePage(await NonogramAPI.LoadPuzzleAsync(stream)));
                    }
                    catch (Exception)
                    {
                        await DisplayAlertAsync(
                            "Puzzle unavailable",
                            $"Failed to load puzzle {(int)button.CommandParameter + 1}. Try a different puzzle.",
                            "CLOSE");
                    }

                };

                menu.Add(button, i % 5 + 1, i / 5 + 1);
            }
        }

        private async Task<Stream> GetPuzzleStreamAsync(int i)
        {
            string puzzleFilename = await PuzzleLibrary.GetPuzzleFilenameAsync(i);
            var puzzleStream = await FileSystem.OpenAppPackageFileAsync($"Puzzles/{puzzleFilename}");
            
            return puzzleStream;
        }
    }
}
