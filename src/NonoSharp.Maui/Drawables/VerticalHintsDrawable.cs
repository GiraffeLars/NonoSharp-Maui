using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maui.Drawables
{
    internal class VerticalHintsDrawable : IDrawable
    {
        private GameAPI game;

        internal VerticalHintsDrawable(GameAPI game)
        {
            this.game = game; 
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float colWidth = dirtyRect.Width / game.Width;

            for (int x = 0; x < game.Width; x++)
            {
                Hints hints = game.VerticalHints[x];

                for (int y = hints.Count - 1; y >= 0; y--)
                {
                    Hint hint = hints.GetHint(y);
                    canvas.FontColor = hint.completed ? Colors.Gray : Colors.Black;

                    canvas.DrawString(
                        hint.number.ToString(),
                        colWidth * x,
                        dirtyRect.Height - (hints.Count - y) * 15 - 5,
                        colWidth,
                        20,
                        HorizontalAlignment.Center,
                        VerticalAlignment.Center);
                }
            }
        }
    }
}
