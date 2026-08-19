using Picross.Game;
using System;

namespace Picross.Maui
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
                        var bytes = await LoadPuzzleAsync((int)button.CommandParameter);
                        await Navigation.PushAsync(new GamePage(await GameAPI.LoadFromSerializedAsync(bytes)));
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

        private async Task<byte[]> LoadPuzzleAsync(int i)
        {
            string puzzleFilename = await PuzzleLibrary.GetPuzzleFilenameAsync(i);
            var puzzleStream = await FileSystem.OpenAppPackageFileAsync($"Puzzles/{puzzleFilename}");
            using var ms = new MemoryStream();
            await puzzleStream.CopyToAsync(ms);
            puzzleStream.Close();
            byte[] bytes = ms.ToArray();
            ms.Close();
            return bytes;
        }
    }
}
