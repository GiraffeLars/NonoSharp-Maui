using NonoSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NonoSharp.Maui.Drawables
{
    internal class rowHintsDrawable : IDrawable
    {
        private readonly NonogramAPI game;

        // The total amount of space a number needs, includes the margin, like a box
        private float numberOffset = 22f;

        // The required Width needed for all the hints. When used in GamePage, this ensures the grid is centered
        internal float RequiredWidth { get; private set; }

        internal rowHintsDrawable(NonogramAPI game)
        {
            this.game = game;
        }

        /// <summary>
        /// Sets the available spacing for the column hints. Also sets <c>this.RequiredWidth</c>.
        /// </summary>
        /// <param name="totalWidth">Total width available for the hints, as calculated in GamePage</param>
        /// <param name="totalHeight">Total height available for the hints, as calculated in GamePage</param>
        /// <param name="maxHints">Maximum amount of hints in any of the column hints</param>
        internal void SetAvailableSize(double totalWidth, double totalHeight, int maxHints)
        {
            // Calculate spacing between numbers, but cap spacing so they are never too far apart
            numberOffset = Math.Min((float)(totalWidth / maxHints), 22f);

            RequiredWidth = numberOffset * maxHints;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float colHeight = dirtyRect.Height / game.Height;
            float textWidth = numberOffset * 0.9f; // Use the number offset (i.e. the 'box' of each number) and take an arbitrary percentage

            for (int y = 0; y < game.Height; y++)
            {
                Hints hints = game.RowHints[y];

                // Traverse backwards through the hint so that the last hint
                // is almost touching the grid
                for (int x = hints.Count - 1; x >= 0; x--)
                {
                    Hint hint = hints[x];
                    canvas.FontColor = hint.Completed ? Theme.CompletedHint : Theme.IncompleteHint;

                    float xPos = dirtyRect.Width - (hints.Count - x) * numberOffset;
                    float yPos = y * colHeight;

                    canvas.DrawString(
                        hint.Number.ToString(),
                        xPos,
                        yPos,
                        textWidth,
                        colHeight,
                        HorizontalAlignment.Left,
                        VerticalAlignment.Center
                        );
                }
            }
        }
    }
}
