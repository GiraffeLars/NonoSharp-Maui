using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using NonoSharp.Exceptions;
using NonoSharp.Maui.Drawables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NonoSharp.Maui
{
    public partial class NonogramBuilderPage : ThemedPage
    {
        BuilderDrawable drawable;
        GraphicsView builderView;
        Grid menu;

        public NonogramBuilderPage(NonogramBuilder builder)
        {
            drawable = new(builder);

            menu = new Grid
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,

                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(5, GridUnitType.Star) },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                }
            };

            builderView = new GraphicsView
            {
                Drawable = drawable
            };
            builderView.StartInteraction += OnTouchStart;

            Button titleButton = new Button() { Text = "Set puzzle title", Margin = 10 };
            titleButton.Clicked += async (s, e) =>
            {
                try
                {
                    menu.IsEnabled = false;
                    var popup = new GiveTitlePopup(builder.Title);
                    IPopupResult<String?> result = await this.ShowPopupAsync<String?>(popup);

                    if (result.WasDismissedByTappingOutsideOfPopup) return;
                    builder.Title = result.Result;
                } finally
                {
                    menu.IsEnabled = true;
                }
            };

            Button saveButton = new Button() { Text = "Save to file", Margin = 10 };
            saveButton.Clicked += async (s, e) =>
            {
                try
                {
                    menu.IsEnabled = false;

                    string filename = (builder.Title ?? "NonoSharp_puzzle") + ".ns";
                    string path = Path.Combine(FileSystem.AppDataDirectory, filename);
                    await builder.SaveAsFileAsync(path);
                    await DisplayAlertAsync("Successfully saved puzzle!", "The puzzle was successfully saved " +
                        $"to {path}", "OK");
                }
                catch (PuzzleNotSolvableException exc)
                {
                    await DisplayAlertAsync("Could not save puzzle!", exc.Message, "OK");
                }
                catch (Exception exc)
                {
                    Debug.WriteLine($"Puzzle saving failed: {exc.Message}" + $" Inner: {exc.InnerException}");
                    await DisplayAlertAsync("Could not save puzzle!", "An error has occurred while trying to " +
                        "save the puzzle to disk. Please try again.", "OK");
                }
                finally
                { 
                    menu.IsEnabled = true; 
                }
                    

            };

            menu.Add(builderView, 1, 0);
            menu.Add(titleButton, 1, 1);
            menu.Add(saveButton, 1, 2);

            Content = menu;
            builderView.Invalidate();
        }

        private void OnTouchStart(object? sender, TouchEventArgs e)
        {
            var touch = e.Touches.First();

            drawable.HandleTouch(touch);
            builderView.Invalidate();
        }
    }
}
