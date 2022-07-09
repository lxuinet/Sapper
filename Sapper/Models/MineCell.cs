using LX;

namespace Sapper.Models
{
    class MineCell : Cell
    {
        static Image _image = Image.LoadIcon(BoxSize / 2, 1413);
        public MineCell()
        {
            this.Visible = false;
            this.ImageColor = Color.Red;
            this.CanAction = true;
            this.Color = Color.Yellow.Alpha(100);
        }

        protected override void DoAction(MouseEventArgs e)
        {
            base.DoAction(e);
            this.Image = this.Image == null ? _image : null;
            this.ImageColor = Color.Red;
        }

        protected override void DoEnabledChanged()
        {
            base.DoEnabledChanged();
            Image = _image;
            this.Color = Color.Transparent;
        }

        protected override void DoClick(MouseEventArgs e)
        {
            base.DoClick(e);
            this.Color = Color.Red.Alpha(150);
            this.ImageColor = Color.Content;
            Map.EndGame(false);
        }
    }
}
