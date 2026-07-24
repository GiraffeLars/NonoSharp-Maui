using Picross.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Picross.Maui.Drawables
{
    internal class columnHintsDrawable : IDrawable
    {
        private readonly GameAPI game;

        // The total amount of space a number needs, includes the margin, like a box
        private float numberOffset = 22f;

        // The required height needed for all the hints. When used in GamePage, this ensures the grid is centered
        internal float RequiredHeight { get; private set; }

        internal columnHintsDrawable(GameAPI game)
        {
            this.game = game; 
        }

        /// <summary>
        /// Sets the available spacing for the column hints. Also sets <c>this.RequiredHeight</c>.
        /// </summary>
        /// <param name="totalWidth">Total width available for the hints, as calculated in GamePage</param>
        /// <param name="totalHeight">Total height available for the hints, as calculated in GamePage</param>
        /// <param name="maxHints">Maximum amount of hints in any of the column hints</param>
        internal void SetAvailableSize(double totalWidth, double totalHeight, int maxHints)
        {
            // Calculate spacing between numbers, but cap spacing so they are never too far apart
            numberOffset = Math.Min((float) (totalHeight / maxHints), 22f);

            RequiredHeight = numberOffset * maxHints;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float colWidth = dirtyRect.Width / game.Width;
            float textHeight = numberOffset * 0.9f; // Use the number offset (i.e. the 'box' of each number) and take an arbitrary percentage

            // Traverse backwards through the hint so that the last hint
            // is almost touching the grid
            for (int x = 0; x < game.Width; x++)
            {
                Hints hints = game.ColumnHints[x];

                for (int y = hints.Count - 1; y >= 0; y--)
                {
                    Hint hint = hints[y];
                    canvas.FontColor = hint.Completed ? Colors.Gray : Colors.Black;


                    float xPos = colWidth * x;

                    // Calculate the y-position using the calculated offset & accounting for the fact that we start from the last hint
                    float yPos = dirtyRect.Height - (hints.Count - y) * numberOffset;

                    canvas.DrawString(
                        hint.Number.ToString(),
                        xPos,
                        yPos,
                        colWidth,
                        textHeight,
                        HorizontalAlignment.Center,
                        VerticalAlignment.Center);
                }
            }
        }
    }
}
