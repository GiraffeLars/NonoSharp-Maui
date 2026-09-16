using NonoSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NonoSharp.Maui.Drawables
{
    internal class rowCluesDrawable : IDrawable
    {
        private readonly NonogramAPI game;

        // The total amount of space a number needs, includes the margin, like a box
        private float numberOffset = 22f;

        // The required Width needed for all the clues. When used in GamePage, this ensures the grid is centered
        internal float RequiredWidth { get; private set; }

        internal rowCluesDrawable(NonogramAPI game)
        {
            this.game = game;
        }

        /// <summary>
        /// Sets the available spacing for the column clues. Also sets <c>this.RequiredWidth</c>.
        /// </summary>
        /// <param name="totalWidth">Total width available for the clues, as calculated in GamePage</param>
        /// <param name="totalHeight">Total height available for the clues, as calculated in GamePage</param>
        /// <param name="maxClues">Maximum amount of clues in any of the column clues</param>
        internal void SetAvailableSize(double totalWidth, double totalHeight, int maxClues)
        {
            // Calculate spacing between numbers, but cap spacing so they are never too far apart
            numberOffset = Math.Min((float)(totalWidth / maxClues), 22f);

            RequiredWidth = numberOffset * maxClues;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float colHeight = dirtyRect.Height / game.Height;
            float textWidth = numberOffset * 0.9f; // Use the number offset (i.e. the 'box' of each number) and take an arbitrary percentage

            for (int y = 0; y < game.Height; y++)
            {
                Clues clues = game.RowClues[y];

                // Traverse backwards through the clue so that the last clue
                // is almost touching the grid
                for (int x = clues.Count - 1; x >= 0; x--)
                {
                    Clue clue = clues[x];
                    canvas.FontColor = clue.Completed ? Theme.CompletedClue : Theme.IncompleteClue;

                    float xPos = dirtyRect.Width - (clues.Count - x) * numberOffset;
                    float yPos = y * colHeight;

                    canvas.DrawString(
                        clue.Number.ToString(),
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
