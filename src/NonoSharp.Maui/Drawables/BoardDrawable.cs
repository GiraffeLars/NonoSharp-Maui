using Microsoft.Maui.Graphics;
using Picross.Game;

namespace Picross.Maui.Drawables;

internal class BoardDrawable : IDrawable
{
    private GameAPI game;
    private float cellSize;
    internal FillType fillType = FillType.FILL;
    internal bool lockFillType = false;
    internal FillType OldFillType { get; set; } = FillType.FILL;

    public BoardDrawable(GameAPI game)
    {
        this.game = game;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        float boardSize = Math.Min(dirtyRect.Width, dirtyRect.Height);
        cellSize = boardSize / game.Width;

        DrawCells(canvas);
        DrawLines(canvas);
    }

    private void DrawCells(ICanvas canvas)
    {
        canvas.FillColor = game.IsPuzzleSolved() ? Theme.SolvedCell : Theme.FilledCell;
        canvas.StrokeColor = Theme.CrossColor;
        canvas.StrokeSize = 4;

        for (int x = 0; x < game.Width; x++)
        {
            for (int y = 0; y < game.Height; y++)
            {
                if (game.IsCellEmpty(x, y))
                {
                    continue;
                }

                float x0 = x * cellSize;
                float y0 = y * cellSize;
                float x1 = x0 + cellSize;
                float y1 = y0 + cellSize;

                if (game.IsCellFilled(x, y))
                {
                    canvas.FillRectangle(x0, y0, cellSize, cellSize);
                } else
                {
                    canvas.DrawLine(x0, y0, x1, y1);
                    canvas.DrawLine(x0, y1, x1, y0);
                }
            }
        }
    }
    private void DrawLines(ICanvas canvas)
    {
        canvas.StrokeColor = Theme.GridLine;
        for (int x = 0; x <= game.Width; x++)
        {
            if (x % 5 == 0)
            {
                canvas.StrokeSize = 4;
            }
            else
            {
                canvas.StrokeSize = 2;
            }

            canvas.DrawLine(x * cellSize, 0, x * cellSize, game.Height * cellSize);
        }

        for (int y = 0; y <= game.Height; y++)
        {
            if (y % 5 == 0)
            {
                canvas.StrokeSize = 4;
            }
            else
            {
                canvas.StrokeSize = 2;
            }


            canvas.DrawLine(0, y * cellSize, game.Width * cellSize, y * cellSize);
        }
    }

    /// <summary>
    /// Converts touch coordinates to cell coordinates. Cell-coordinates are in <c>int</c> and can thus safely
    /// be casted as such.
    /// </summary>
    /// <param name="touchX">x coordinate of touch</param>
    /// <param name="touchY">y coordinate of touch</param>
    /// <returns><c>Point</c> of cell touched.</returns>
    public Point ConvertTouchToCell(double touchX, double touchY)
    {
        return new Point(
            Math.Floor(touchX / cellSize),
            Math.Floor(touchY / cellSize)
            );
    }

    public Point ConvertTouchToCell(Point touchCoordinates)
    {
        return ConvertTouchToCell(touchCoordinates.X, touchCoordinates.Y);
    }

    /// <summary>
    /// Converts touch coordinates to cell coordinates, then properly handles the cell. 
    /// See <seealso cref="ConvertTouchToCell(double, double)"/> and <seealso cref="HandleCell(int, int)"/>
    /// </summary>
    /// <param name="touchX"></param>
    /// <param name="touchY"></param>
    public void HandleTouch(float touchX, float touchY)
    {
        Point cell = ConvertTouchToCell(touchX, touchY);
        HandleCell((int) cell.X, (int) cell.Y);
    }

    /// <summary>
    /// Handles a clicked cell by updating its state according to selected mode.
    /// </summary>
    /// <param name="x">x coordinate of cell</param>
    /// <param name="y">y coordinate of cell</param>
    public void HandleCell(int x, int y)
    {

        // TODO Rework the whole cell handling. This one is getting overly complicated
        // since we now rely on fill type more than cell status.

        if (x < 0 || x >= game.Width || y < 0 || y >= game.Height)
        {
            return;
        }

        if (!lockFillType)
        {
            // This is the first move in a potential drag, we need to determine the users intention
            DetermineTouchIntention(x, y);
            lockFillType = true;
        }

        SetCell(x, y);
    }

    /// <summary>
    /// Handles a clicked cell by updating its state according to selected mode. See also <seealso cref="HandleCell(int, int)"/>.
    /// </summary>
    /// <param name="cell">The coordinates of the clicked cell (so in cell coordinates)</param>
    public void HandleCell(Point cell)
    {
        HandleCell((int)cell.X, (int)cell.Y);
    }


    private void DetermineTouchIntention(int x, int y)
    {
        if (game.IsCellFilled(x, y))
        {
            if (fillType == FillType.FILL)
            {
                fillType = FillType.EMPTY;
            }
            // Do not update the fill type if we click a filled cell when either cross or empty is selected, keep current one
        }
        else if (game.IsCellCrossed(x, y))
        {
            if (fillType == FillType.CROSS)
            {
                fillType = FillType.EMPTY;
            }
            // Do not update the fill type if we click a crossed cell when either fill or empty is selected, keep current one
        }

        // When a empty cell is clicked, we should leave the fill type as is.
    }

    private void SetCell(int x, int y)
    {
        // Do not change already filled cells, as this would introduce a new action to undo
        if (fillType == FillType.FILL && !game.IsCellFilled(x, y))
        {
            game.FillCell(x, y);
        }
        // Again, do not change crossed cells to cross to avoid introducing new undo actions
        else if (fillType == FillType.CROSS && !game.IsCellCrossed(x, y))
        {
            game.CrossCell(x, y);
        }
        else if (fillType == FillType.EMPTY && !game.IsCellEmpty(x, y))
        {
            // Only empty cells that were part of the intended types to remove
            // E.g. only empty cells that are filled in if the intention was to empty filled in cells
            if ((OldFillType == FillType.FILL && game.IsCellFilled(x, y)) ||
                (OldFillType == FillType.CROSS && game.IsCellCrossed(x, y)) )
            {
                game.EmptyCell(x, y);
            }
        }
    }
}