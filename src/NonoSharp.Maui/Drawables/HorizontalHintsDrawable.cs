using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maui.Drawables
{
    internal class HorizontalHintsDrawable : IDrawable
    {
        private GameAPI game;

        internal HorizontalHintsDrawable(GameAPI game)
        {
            this.game = game;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float colHeight = dirtyRect.Height / game.Height;
            for (int y = 0; y < game.Height; y++)
            {
                Hints hints = game.HorizontalHints[y];

                for (int x = hints.Count - 1; x >= 0; x--)
                {
                    Hint hint = hints.GetHint(x);
                    canvas.FontColor = hint.Completed ? Colors.Gray : Colors.Black;

                    float xPos = dirtyRect.Width - (hints.Count - x) * 10;
                    float yPos = y * colHeight;

                    canvas.DrawString(
                        hint.Number.ToString(),
                        xPos,
                        yPos,
                        colHeight,
                        colHeight,
                        HorizontalAlignment.Left,
                        VerticalAlignment.Center
                        );
                }
            }
        }
    }
}
