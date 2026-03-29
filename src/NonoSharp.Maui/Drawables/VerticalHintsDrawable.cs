using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maui.Drawables
{
    internal class VerticalHintsDrawable : IDrawable
    {
        private GameAPI game;
        internal static int NUMBER_HEIGHT = 20;
        internal static int NUMBER_OFFSET = 15;

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
                    canvas.FontColor = hint.Completed ? Colors.Gray : Colors.Black;

                    canvas.DrawString(
                        hint.Number.ToString(),
                        colWidth * x,
                        dirtyRect.Height - (hints.Count - y) * NUMBER_OFFSET - 5,
                        colWidth,
                        NUMBER_HEIGHT,
                        HorizontalAlignment.Center,
                        VerticalAlignment.Center);
                }
            }
        }
    }
}
