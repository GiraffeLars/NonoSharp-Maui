using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Core;

namespace Maui;

public class MainPage : ContentPage
{
    private GameAPI game;
    private GraphicsView boardView;
    private BoardDrawable drawable;

    public MainPage()
    {
        game = new GameAPI(10, 10);
        drawable = new BoardDrawable(game);

        boardView = new GraphicsView
        {
            Drawable = drawable
        };

        boardView.StartInteraction += OnBoardTouched;

        Content = new Grid
        {
            Children =
            {
                boardView
            }
        };
    }

    private void OnBoardTouched(object sender, TouchEventArgs e)
    {
        var touch = e.Touches.First();

        drawable.HandleTouch(touch.X, touch.Y);

        boardView.Invalidate();
    }
}