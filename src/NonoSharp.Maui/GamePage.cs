using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Picross.Game;
using Picross.Maui.Drawables;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Picross.Maui;

public class GamePage : ContentPage
{
    private GameAPI game;
    private GraphicsView boardView;
    private GraphicsView verticalHintsView;
    private GraphicsView horizontalHintsView;
    private BoardDrawable boardDrawable;
    private VerticalHintsDrawable verticalHintsDrawable;
    private HorizontalHintsDrawable horizontalHintsDrawable;

    private const int BUTTON_HEIGHT = 50;
    private const int BUTTON_MARGIN = 10;
    private const float BOARD_SCREEN_PERCENTAGE = 0.75f;

    private int maxHorizontalHints; // The greatest number of horizontal hints in a row
    private int maxVerticalHints; // The greatest number of vertical hints in a column

    private Button toggleButton;

    private Grid commandButtonsGrid;
    private Button undoButton;
    private Button redoButton;

    private Grid mainGrid;

    public GamePage(int width, int height)
    {
        game = new GameAPI(width, height);

        FillHintData();
        CreateViews();
        CreateMainGrid();

        Content = mainGrid;
    }

    /// <summary>
    /// Dynamically calculates the screen space for the grid and hints.
    /// </summary>
    /// Uses the screen size to calculate the grid size with <c>BOARD_SCREEN_PERCENTAGE</c>
    /// Calculates the width and height for the hint views, then lets the drawables calculated the total used space
    /// Using that, finalize the drawing.
    /// <param name="width">Screen width</param>
    /// <param name="height">Screen height</param>
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        // Check for valid width and height
        if (width <= 0 || height <= 0)
        {
            return;
        }

        // Calculate the allocated board size & set the request
        double boardSize = Math.Min(width, height) * BOARD_SCREEN_PERCENTAGE;

        boardView.HeightRequest = boardSize;
        boardView.WidthRequest = boardSize;

        double buttonSize = BUTTON_HEIGHT + BUTTON_MARGIN * 2; // margin * 2 as margin is 10 px on both 
        double availableHintHeight = height - boardSize - buttonSize;
        double availableHintWidth = width - boardSize;

        // Set the hint spacing and calculate required width/height to ensure grid centering
        verticalHintsDrawable.SetAvailableSize(boardSize, availableHintHeight, maxVerticalHints);
        horizontalHintsDrawable.SetAvailableSize(availableHintWidth, boardSize, maxHorizontalHints);

        // Give the hints their allocated screen space
        verticalHintsView.HeightRequest = verticalHintsDrawable.RequiredHeight;
        verticalHintsView.WidthRequest = boardSize;

        horizontalHintsView.HeightRequest = boardSize;
        horizontalHintsView.WidthRequest = horizontalHintsDrawable.RequiredWidth;
        commandButtonsGrid.WidthRequest = horizontalHintsDrawable.RequiredWidth;
        undoButton.WidthRequest = horizontalHintsDrawable.RequiredWidth / 2;
        redoButton.WidthRequest = horizontalHintsDrawable.RequiredWidth / 2;

        InvalidateViews();
    }

    /// <summary>
    /// Invalidates all views
    /// </summary>
    private void InvalidateViews()
    {
        boardView.Invalidate();
        verticalHintsView.Invalidate();
        horizontalHintsView.Invalidate();
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

    [MemberNotNull(nameof(boardView), nameof(verticalHintsView), nameof(horizontalHintsView), nameof(verticalHintsDrawable), nameof(horizontalHintsDrawable))]
    private void CreateViews()
    {
        boardDrawable = new BoardDrawable(game);
        verticalHintsDrawable = new VerticalHintsDrawable(game);
        horizontalHintsDrawable = new HorizontalHintsDrawable(game);

        boardView = new GraphicsView
        {
            Drawable = boardDrawable,
        };

        verticalHintsView = new GraphicsView
        {
            Drawable = verticalHintsDrawable,
        };

        horizontalHintsView = new GraphicsView
        {
            Drawable = horizontalHintsDrawable
        };

        boardView.StartInteraction += OnBoardTouched;
    }

    [MemberNotNull(nameof(mainGrid))]
    private void CreateMainGrid()
    {
        mainGrid = new Grid
        {
            // This centers the entire grid structure on the page
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,

            RowDefinitions =
        {
            new RowDefinition { Height = GridLength.Auto },    // 0: Top hints
            new RowDefinition { Height = GridLength.Auto },    // 1: Board
            new RowDefinition { Height = GridLength.Auto }     // 2: Button
        },
            ColumnDefinitions =
        {
            new ColumnDefinition { Width = GridLength.Auto }, // 0: Left hints
            new ColumnDefinition { Width = GridLength.Auto },                    // 1: Board
        }
        };

        CreateToggleButton();
        CreateCommandButtons();

        // Add children
        mainGrid.Add(boardView, 1, 1);          // bottom-right
        mainGrid.Add(verticalHintsView, 1, 0);  // top-right (above board)
        mainGrid.Add(horizontalHintsView, 0, 1);// bottom-left (beside board)
        mainGrid.Add(toggleButton, 1, 2);       // underneath the board
        mainGrid.Add(commandButtonsGrid, 0, 2);       // underneath left hints
    }


    [MemberNotNull(nameof(toggleButton))]
    private void CreateToggleButton()
    {
        // Controls button
        toggleButton = new Button
        {
            Text = "Mode: Fill",
            Margin = new Thickness(BUTTON_MARGIN),
            HeightRequest = BUTTON_HEIGHT,
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
    }


    [MemberNotNull(nameof(undoButton), nameof(redoButton), nameof(commandButtonsGrid))]
    private void CreateCommandButtons()
    {
        commandButtonsGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Auto }
            }
        };

        // Undo button
        undoButton = new Button
        {
            Text = "Undo",
            Margin = new Thickness(0, BUTTON_MARGIN),
            HeightRequest = BUTTON_HEIGHT,
            //WidthRequest = 20,
            VerticalOptions = LayoutOptions.Start,
            IsEnabled = false
        };

        // When clicked
        undoButton.Clicked += (sender, e) =>
        {
            game.Undo();
            UpdateCommandButtons();
            InvalidateViews();
        };

        // Redo
        redoButton = new Button
        {
            Text = "Redo",
            Margin = new Thickness(0, BUTTON_MARGIN),
            HeightRequest = BUTTON_HEIGHT,
            //WidthRequest = 20,
            VerticalOptions = LayoutOptions.Start,
            IsEnabled = false
        };

        // When clicked
        redoButton.Clicked += (sender, e) =>
        {
            game.Redo();
            UpdateCommandButtons();
            InvalidateViews();
        };

        commandButtonsGrid.Add(undoButton, 0, 0);
        commandButtonsGrid.Add(redoButton, 1, 0);
    }

    private void UpdateCommandButtons()
    {
        undoButton.IsEnabled = game.CanUndo;
        redoButton.IsEnabled = game.CanRedo;
    }

    private void OnBoardTouched(object sender, TouchEventArgs e)
    {
        var touch = e.Touches.First();
        boardDrawable.HandleTouch(touch.X, touch.Y);

        UpdateCommandButtons();
        InvalidateViews();
    }
}