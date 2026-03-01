using Core;
using Microsoft.Maui.Graphics;

namespace Maui;

public class BoardDrawable : IDrawable
{
    private GameAPI game;

    public BoardDrawable(GameAPI game)
    {
        this.game = game;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {

    }

    public void HandleTouch(float touchX, float touchY)
    {

    }
}