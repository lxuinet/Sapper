using LX;

namespace Sapper.Models
{
    class EndCell : Cell
    {
        static Image _image = Image.LoadIcon(BoxSize / 2, 1419);
        public EndCell()
        {
            this.Image = _image;
            this.Color = Color.Parent.Alpha(150);
            this.Enabled = false;
        }

        protected override void DoEnabledChanged()
        {
            base.DoEnabledChanged();
            if (this.Enabled)
            {
                this.Color = Color.Parent;
            }
        }

        protected override void DoClick(MouseEventArgs e)
        {
            base.DoClick(e);
            Map.EndGame(true);
        }


    }
}
