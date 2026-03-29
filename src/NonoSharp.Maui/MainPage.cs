using Core;
using Maui.Drawables;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Maui;

public class MainPage : ContentPage
{
    private GameAPI game;
    private GraphicsView boardView;
    private GraphicsView verticalHintsView;
    private GraphicsView horizontalHintsView;
    private BoardDrawable boardDrawable;
    private int horizontalHintSpace; // The space required for the horizontal hints
    private int verticalHintSpace; // The space required for the horizontal hints

    private int maxHorizontalHints; // The greatest number of horizontal hints in a row
    private int maxVerticalHints; // The greatest number of vertical hints in a column

    // 1. Give your grid a class-level scope so we can resize it later
    private Grid mainGrid;

    public MainPage()
    {
        game = new GameAPI(10, 10);
        boardDrawable = new BoardDrawable(game);

        FillHintData();
        CreateViews();
        CreateMainGrid();

        Content = mainGrid;
    }

    // Dynamically constrain the Grid so the board's row and column remain perfectly square
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (width > 0 && height > 0)
        {
            // Subtract the 50px hint margins to find the maximum possible board space
            double availableWidth = width - horizontalHintSpace - 10;
            double availableHeight = height - verticalHintSpace * maxVerticalHints;

            // The board needs to be a square, so we take the smaller of the two dimensions
            double boardSize = Math.Min(availableWidth, availableHeight);

            // Force the grid row and column to use this exact size. 
            // This ensures horizontalHintsView.Height EXACTLY matches boardView's drawn height.
            mainGrid.RowDefinitions[1].Height = new GridLength(boardSize);
            mainGrid.ColumnDefinitions[1].Width = new GridLength(boardSize);
        }
    }

    private int GetMaxHints(Hints[] hints)
    {
        int max = 0;

        for (int i = 0; i < hints.Length; i++)
        {
            int curr = hints[i].Count;

            if (max < curr)
            {
                max = curr;
            }
        }
        return max;
    }

    [MemberNotNull(nameof(maxHorizontalHints), nameof(maxVerticalHints))]
    private void FillHintData()
    {
        maxHorizontalHints = GetMaxHints(game.HorizontalHints);
        maxVerticalHints = GetMaxHints(game.VerticalHints);
    }

    [MemberNotNull(nameof(boardView), nameof(verticalHintsView), nameof(horizontalHintsView))]
    private void CreateViews()
    {

        boardView = new GraphicsView
        {
            Drawable = boardDrawable,
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
    }

    [MemberNotNull(nameof(mainGrid), nameof(verticalHintSpace), nameof(horizontalHintSpace))]
    private void CreateMainGrid()
    {
        horizontalHintSpace = 60;
        mainGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Star },     // top hints row
                new RowDefinition { Height = GridLength.Star },     // board row
                new RowDefinition { Height = GridLength.Star }      // Button
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(horizontalHintSpace) }, // left hints column
                new ColumnDefinition { Width = GridLength.Star }     // board column
            }
        };

        verticalHintSpace = VerticalHintsDrawable.NUMBER_HEIGHT + VerticalHintsDrawable.NUMBER_OFFSET;

        // Controls button
        Button toggleButton = new Button
        {
            Text = "Mode: Fill",
            Margin = new Thickness(10),
            HeightRequest = 50,
            VerticalOptions = LayoutOptions.Start
        };

        // When clicked
        toggleButton.Clicked += (sender, e) =>
        {
            boardDrawable.filling = !boardDrawable.filling;
            if (boardDrawable.filling)
            {
                toggleButton.Text = "Mode: Fill";
            }
            else
            {
                toggleButton.Text = "Mode: Cross";
            }
        };

        // Add children
        mainGrid.Add(boardView, 1, 1);          // bottom-right
        mainGrid.Add(verticalHintsView, 1, 0);  // top-right (above board)
        mainGrid.Add(horizontalHintsView, 0, 1);// bottom-left (beside board)
        mainGrid.Add(toggleButton, 1, 2);       // underneath the board
    }
    private void OnBoardTouched(object sender, TouchEventArgs e)
    {
        var touch = e.Touches.First();
        boardDrawable.HandleTouch(touch.X, touch.Y);

        boardView.Invalidate();
        verticalHintsView.Invalidate();
        horizontalHintsView.Invalidate();
    }
}