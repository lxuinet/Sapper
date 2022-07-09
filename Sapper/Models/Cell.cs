using System.Linq;

using LX;

namespace Sapper.Models
{
    enum CellSize { Size1 = 1, Size2 = 2, Size3 = 3, Size4 = 4, Size5 = 5 }
    class Cell : PictureBox
    {
        public static int BoxSize { get; } = 48;
        static Image _image = Image.LoadIcon(BoxSize / 2, 1413);

        private CellSize _cellSize = CellSize.Size1;
        public CellSize CellSize
        {
            get { return _cellSize; }
            set
            {
                _cellSize = value;
                this.Size = (BoxSize * (int)value) + 1;
            }
        }

        public Cell()
        {
            this.AutoSize = false;
            this.CellSize = CellSize.Size1;
            this.Style = ColorStyle.Normal | ColorStyle.Hovered | ColorStyle.Downed;
            this.UserMouse = UserMode.On;
            this.Color = Color.Transparent;
            this.ImageAlignment = Alignment.Center;
            this.ContentStyle = ColorStyle.Normal;
            this.BorderSize = 1;
            this.BorderStyle = ColorStyle.Normal;
            this.BorderColor = Color.Gray;
        }


        public Map Map { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        internal void Set(Map map, int x, int y)
        {
            this.Map = map;
            this.X = x;
            this.Y = y;
        }

        protected override void DoVisibleChanged()
        {
            base.DoVisibleChanged();
        }

        protected override void DoEnabledChanged()
        {
            base.DoEnabledChanged();
            this.Visible = true;
        }

        protected override void DoClick(MouseEventArgs e)
        {
            base.DoClick(e);
            OpenAround(X, Y);
        }

        protected override void DoAction(MouseEventArgs e)
        {
            base.DoAction(e);
        }

        private void OpenAround(int x, int y)
        {
            var cells = Map.Cells;

            var current = cells[x, y];

            if (!current.Enabled || current is EndCell)
            {
                return;
            }

            current.Enabled = false;

            var hasMine = Map.GetCells(x - 1, y - 1, x + 1, y + 1).OfType<MineCell>().Any();

            Map.GetCells(x - 1, y - 1, x + 1, y + 1).ForEach(cell =>
            {
                if (cell is EndCell)
                {
                    cell.Enabled = true;
                }
                cell.Visible = true;
                if (!hasMine)
                {
                    OpenAround(cell.X, cell.Y);
                }
            });

        }

    }
}
