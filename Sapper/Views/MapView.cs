using LX;
using Sapper.Models;

namespace Sapper.Views
{
    class MapView : Control
    {
        static Image _backImage = Image.LoadFromResource("*.Tile_5.png");
        public MapView()
        {
            //this.Shadow = ShadowStyle.Outer1;
            this.Color = Color.Green;
            this.BorderSize = 1;
            this.BorderColor = Color.Primary;
            this.Radius = 4;
            this.Shape = CornerShape.Oval;
        }


        private Map _map;
        public Map Map
        {
            get { return _map; }
            set
            {
                if (value != null)
                {
                    _map = value;
                    var controls = this.Controls;

                    controls.Clear();

                    var back = new PictureBox();
                    back.ImageAlignment = Alignment.TopLeft;
                    back.Image = _backImage;
                    back.Tile = ImageTile.Horizontal | ImageTile.Vertical;
                    back.AddTo(this, Alignment.Fill);


                    var size = Cell.BoxSize;
                    var width = _map.Width;
                    var height = _map.Height;
                    var cells = _map.Cells;               

                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            var cell = cells[x, y];
                            if (cell != null && !controls.Contains(cell))
                            {
                                cell.Left = x * size;
                                cell.Top = y * size;
                                cell.AddTo(this);
                            }
                        }
                    }

                    this.Width = width * size + 1;
                    this.Height = height * size + 1;
                }
            }
        }
    }
}
