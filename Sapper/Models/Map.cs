using System;
using System.Collections.Generic;
using System.Linq;

using Sapper.Views;

namespace Sapper.Models
{
   
    public static class LinqHelper
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }
    }

    delegate void Func(int x, int y, Cell[,] cells);

    class Map
    {
        private int _lives = 3;
        public int Lives { get { return _lives; } }

        private int _mines = 0;
        public int Mines { get { return _mines; } }

        public int Width { get; }
        public int Height { get; }
        public Cell[,] Cells { get; }

        public Map(int lives, int width, int height)
        {
            this._lives = lives;
            this.Width = width;
            this.Height = height;
            this.Cells = new Cell[width, height];

            Generate();
        }

        public void Enumerate(int x0, int y0, int x1, int y1, Func predicate)
        {
            var width = this.Width - 1;
            var height = this.Height - 1;
            var cells = this.Cells;

            x0 = Math.Min(Math.Max(x0, 0), width);
            y0 = Math.Min(Math.Max(y0, 0), height);
            x1 = Math.Min(Math.Max(x1, 0), width);
            y1 = Math.Min(Math.Max(y1, 0), height);

            for (int i = x0; i <= x1; i++)
            {
                for (int j = y0; j <= y1; j++)
                {
                    predicate(i, j, cells);
                }
            }
        }

        public IEnumerable<Cell> GetCells(int x0, int y0, int x1, int y1)
        {
            var width = this.Width;
            var height = this.Height;
            var cells = this.Cells;

            x0 = Math.Min(Math.Max(x0, 0), width - 1);
            y0 = Math.Min(Math.Max(y0, 0), height - 1);
            x1 = Math.Min(Math.Max(x1, 0), width - 1);
            y1 = Math.Min(Math.Max(y1, 0), height - 1);

            for (int i = x0; i <= x1; i++)
            {
                for (int j = y0; j <= y1; j++)
                {
                    var cell = cells[i, j];
                    if (cell != null)
                    {
                        yield return cell;
                    }
                }
            }
        }

        public IEnumerable<Cell> GetCells()
        {
            return GetCells(0, 0, this.Width, this.Height);
        }


        private void Generate()
        {
            var width = this.Width;
            var height = this.Height;
            var cells = this.Cells;

            var random = new Random(DateTime.Now.Millisecond);

            PutCell(0, 0, new StartCell(), false);
            PutCell(width - 1, height - 1, new EndCell(), false);

            // generate textures
            var blocks = Math.Max(3, width * height / 50);
            for (int s = 5; s > 0; s--)
            {
                for (int i = 0; i < blocks / s; i++)
                {
                    var cell = new SurfaceCell();
                    cell.CellSize = (CellSize)s;
                    for (int r = 0; r < 5; r++)
                    {
                        var x = random.Next(0, width);
                        var y = random.Next(0, height);

                        if (PutCell(x, y, cell, true))
                        {
                            break;
                        }
                    }

                }

            }

            // generate mines
            var mines = width * height / 5;
            for (int i = 0; i < mines; i++)
            {
                var cell = new MineCell();

                for (int r = 0; r < 5; r++)
                {
                    var x = random.Next(0, width);
                    var y = random.Next(0, height);

                    if (PutCell(x, y, cell, true))
                    {
                        _mines++;
                        break;
                    }
                }

            }

            HasPath();

            // fill free blocks
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var cell = cells[x, y];
                    if (cell == null)
                    {
                        int minesAround = GetCells(x - 1, y - 1, x + 1, y + 1)
                            .OfType<MineCell>()
                            .Count();

                        var freeCell = new FreeCell(minesAround);

                        PutCell(x, y, freeCell, false);
                    }
                }
            }

        }

        private bool PutCell(int x0, int y0, Cell cell, bool scanPath)
        {
            var cellSize = (int)cell.CellSize;

            int x1 = x0 + cellSize - 1;
            int y1 = y0 + cellSize - 1;

            // check for map out
            if (x0 < 0 || y0 < 0 || x1 >= this.Width || y1 >= this.Height)
            {
                return false;
            }

            // check for free space 
            if (GetCells(x0, y0, x1, y1).Any())
            {
                return false;
            }

            // set cells
            Enumerate(x0, y0, x1, y1, (x, y, cells) => cells[x, y] = cell);

            if (scanPath && !HasPath())
            {
                // clear cells
                Enumerate(x0, y0, x1, y1, (x, y, cells) => cells[x, y] = null);
                return false;
            }

            cell.Set(this, x0, y0);

            return true;
        }


        private class WayPoint
        {
            public bool IsWall;
            public bool[] Ways;

            public WayPoint(bool isWall = false)
            {
                IsWall = isWall;
                Ways = new bool[8];
            }

        }
        private bool HasPath()
        {
            var width = this.Width;
            var height = this.Height;
            var points = new WayPoint[width, height];


            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var cell = Cells[i, j];
                    points[i, j] = new WayPoint(cell is SurfaceCell || cell is MineCell);
                }
            }

            bool hasPath = false;

            int x = 0;
            int y = 0;

            while (true)
            {
                if (x == width - 1 && y == height - 1)
                {
                    hasPath = true;
                    break;
                }

                var current = points[x, y];
                if (current == null)
                {
                    current = new WayPoint();
                    points[x, y] = current;
                }

                bool hasWay = false;

                for (int i = 0; i < 8; i++)
                {
                    if (!current.Ways[i])
                    {
                        current.Ways[i] = true;
                        if (CheckPoint(i, ref x, ref y, width, height, points))
                        {
                            hasWay = true;
                            break;
                        }
                    }
                }

                if (hasWay)
                {
                    continue;
                }
                else
                {
                    break;
                }

            }

            return hasPath;
        }

        private bool CheckPoint(int direction, ref int x, ref int y, int width, int height, WayPoint[,] points)
        {
            int cx = x;
            int cy = y;

            if (direction == 0 || direction == 1 || direction == 7)
            {
                cx++;
            }

            if (direction == 3 || direction == 4 || direction == 5)
            {
                cx--;
            }

            if (direction == 1 || direction == 2 || direction == 3)
            {
                cy++;
            }

            if (direction == 5 || direction == 6 || direction == 7)
            {
                cy--;
            }

            if (cx >= 0 && cy >= 0 && cx < width && cy < height && (points[cx, cy] == null || !points[cx, cy].IsWall))
            {
                x = cx;
                y = cy;
                return true;
            }

            return false;
        }


        public void EndGame(bool complete)
        {
            if (!complete)
            {
                _lives--;
            }
            if (complete || _lives <= 0)
            {
                GetCells().ForEach(cell => cell.Enabled = false);

                MessageBox.ShowDialog(complete ? "Congratulations! You won :)" : "Sorry, you lose :(");
            }
        }

    }
}
