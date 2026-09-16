using NonoSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NonoSharp.Maui.Drawables
{
    internal class columnCluesDrawable : IDrawable
    {
        private readonly NonogramAPI game;

        // The total amount of space a number needs, includes the margin, like a box
        private float numberOffset = 22f;

        // The required height needed for all the clues. When used in GamePage, this ensures the grid is centered
        internal float RequiredHeight { get; private set; }

        internal columnCluesDrawable(NonogramAPI game)
        {
            this.game = game; 
        }

        /// <summary>
        /// Sets the available spacing for the column clues. Also sets <c>this.RequiredHeight</c>.
        /// </summary>
        /// <param name="totalWidth">Total width available for the clues, as calculated in GamePage</param>
        /// <param name="totalHeight">Total height available for the clues, as calculated in GamePage</param>
        /// <param name="maxClues">Maximum amount of clues in any of the column clues</param>
        internal void SetAvailableSize(double totalWidth, double totalHeight, int maxClues)
        {
            // Calculate spacing between numbers, but cap spacing so they are never too far apart
            numberOffset = Math.Min((float) (totalHeight / maxClues), 22f);

            RequiredHeight = numberOffset * maxClues;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float colWidth = dirtyRect.Width / game.Width;
            float textHeight = numberOffset * 0.9f; // Use the number offset (i.e. the 'box' of each number) and take an arbitrary percentage

            // Traverse backwards through the clue so that the last clue
            // is almost touching the grid
            for (int x = 0; x < game.Width; x++)
            {
                Clues clues = game.ColumnClues[x];

                for (int y = clues.Count - 1; y >= 0; y--)
                {
                    Clue clue = clues[y];
                    canvas.FontColor = clue.Completed ? Theme.CompletedClue : Theme.IncompleteClue;


                    float xPos = colWidth * x;

                    // Calculate the y-position using the calculated offset & accounting for the fact that we start from the last clue
                    float yPos = dirtyRect.Height - (clues.Count - y) * numberOffset;

                    canvas.DrawString(
                        clue.Number.ToString(),
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
