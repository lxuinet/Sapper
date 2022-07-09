using LX;

namespace Sapper.Models
{
    class FreeCell : Cell
    {
        static Image _image = Image.LoadIcon(BoxSize / 2, 1413);

        public int Mines { get; }
        public FreeCell(int mines)
        {
            this.Mines = mines;
            this.Visible = false;
            this.CanAction = true;
            this.Color = Color.Yellow.Alpha(100);
        }

        protected override void DoEnabledChanged()
        {
            base.DoEnabledChanged();
            
            this.Color = Color.Transparent;
            this.Image = null;

            if (Mines > 0)
            {
                var text = this.Add(Mines.ToString(), Alignment.Center | Alignment.NotLayouted);
                text.TextStyle = ColorStyle.Normal;
                text.TextColor = Color.Black;
            }
        }

        protected override void DoAction(MouseEventArgs e)
        {
            base.DoAction(e);
            this.Image = this.Image == null ? _image : null;
            this.ImageColor = Color.Red;
        }

    }
}
