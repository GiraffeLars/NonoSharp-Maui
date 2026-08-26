using System;
using System.Collections.Generic;
using System.Text;

namespace NonoSharp.Maui.Drawables
{
    internal class BuilderDrawable : IDrawable
    {
        private NonogramBuilder builder;
        private float cellSize;

        internal BuilderDrawable(NonogramBuilder builder)
        {
            this.builder = builder;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float boardSize = Math.Min(dirtyRect.Width, dirtyRect.Height);
            cellSize = boardSize / builder.Width;

            DrawCells(canvas);
            DrawLines(canvas);
        }


        // TODO: Rework to make BoardDrawable and BuilderDrawable use the same draw methods
        private void DrawCells(ICanvas canvas)
        {
            canvas.FillColor = Theme.FilledCell;
            canvas.StrokeColor = Theme.CrossColor;
            canvas.StrokeSize = 4;

            for (int x = 0; x < builder.Width; x++)
            {
                for (int y = 0; y < builder.Height; y++)
                {
                    if (builder.IsCellEmpty(x, y))
                    {
                        continue;
                    }

                    float x0 = x * cellSize;
                    float y0 = y * cellSize;
                    float x1 = x0 + cellSize;
                    float y1 = y0 + cellSize;

                    canvas.FillRectangle(x0, y0, cellSize, cellSize);
                }
            }
        }
        private void DrawLines(ICanvas canvas)
        {
            canvas.StrokeColor = Theme.GridLine;
            for (int x = 0; x <= builder.Width; x++)
            {
                if (x % 5 == 0)
                {
                    canvas.StrokeSize = 4;
                }
                else
                {
                    canvas.StrokeSize = 2;
                }

                canvas.DrawLine(x * cellSize, 0, x * cellSize, builder.Height * cellSize);
            }

            for (int y = 0; y <= builder.Height; y++)
            {
                if (y % 5 == 0)
                {
                    canvas.StrokeSize = 4;
                }
                else
                {
                    canvas.StrokeSize = 2;
                }


                canvas.DrawLine(0, y * cellSize, builder.Width * cellSize, y * cellSize);
            }
        }

        internal void HandleTouch(PointF touch)
        {
            int x = (int) Math.Floor(touch.X / cellSize);
            int y = (int) Math.Floor(touch.Y / cellSize);

            if (x < 0 || y < 0 || x >= builder.Width || y >= builder.Height)
            {
                return;
            }

            if (builder.IsCellFilled(x, y))
            {
                builder.EmptyCell(x, y);
            } else
            {
                builder.FillCell(x, y);
            }
        }
    }
}
