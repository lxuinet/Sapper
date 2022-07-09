using LX;

namespace Sapper.Models
{
    class StartCell : Cell
    {
        static Image _image = Image.LoadIcon(BoxSize / 2, 1419);
        public StartCell()
        {
            this.Image = _image;
            this.Color = Color.Parent.Alpha(255);
        }

        protected override void DoEnabledChanged()
        {
            base.DoEnabledChanged();
            this.Color = Color.Parent.Alpha(150);
        }

    }
}
