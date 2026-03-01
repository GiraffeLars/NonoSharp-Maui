using Core;
using Maui.Drawables;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Maui;

public class MainPage : ContentPage
{
    private GameAPI game;
    private GraphicsView boardView;
    private GraphicsView verticalHintsView;
    private GraphicsView horizontalHintsView;
    private BoardDrawable drawable;

    // 1. Give your grid a class-level scope so we can resize it later
    private Grid mainGrid;

    public MainPage()
    {
        game = new GameAPI(10, 10);
        drawable = new BoardDrawable(game);

        boardView = new GraphicsView
        {
            Drawable = drawable,
        };

        verticalHintsView = new GraphicsView
        {
            Drawable = new VerticalHintsDrawable(game),
        };

        horizontalHintsView = new GraphicsView
        {
            Drawable = new HorizontalHintsDrawable(game)
        };

        boardView.StartInteraction += OnBoardTouched;

        mainGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(50) }, // top hints row
                new RowDefinition { Height = GridLength.Star },     // board row
                new RowDefinition { Height = GridLength.Auto }
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(50) }, // left hints column
                new ColumnDefinition { Width = GridLength.Star }     // board column
            }
        };
        

        // Add children
        mainGrid.Add(boardView, 1, 1);          // bottom-right
        mainGrid.Add(verticalHintsView, 1, 0);  // top-right (above board)
        mainGrid.Add(horizontalHintsView, 0, 1);// bottom-left (beside board)

        Content = mainGrid;
    }

    // Dynamically constrain the Grid so the board's row and column remain perfectly square
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (width > 0 && height > 0)
        {
            // Subtract the 50px hint margins to find the maximum possible board space
            double availableWidth = width - 50;
            double availableHeight = height - 50;

            // The board needs to be a square, so we take the smaller of the two dimensions
            double boardSize = Math.Min(availableWidth, availableHeight);

            // Force the grid row and column to use this exact size. 
            // This ensures horizontalHintsView.Height EXACTLY matches boardView's drawn height.
            mainGrid.RowDefinitions[1].Height = new GridLength(boardSize);
            mainGrid.ColumnDefinitions[1].Width = new GridLength(boardSize);
        }
    }

    private void OnBoardTouched(object sender, TouchEventArgs e)
    {
        var touch = e.Touches.First();
        drawable.HandleTouch(touch.X, touch.Y);

        boardView.Invalidate();
        verticalHintsView.Invalidate();
        horizontalHintsView.Invalidate();
    }
}