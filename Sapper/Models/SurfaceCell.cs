using System;
using System.Collections.Generic;
using LX;

namespace Sapper.Models
{
    class SurfaceCell : Cell
    {
        static List<Image> _surfaces;
        static Random _random;
        static SurfaceCell()
        {
            _random = new Random(DateTime.Now.Millisecond);
            _surfaces = new List<Image>();
            foreach (var key in App.Resources.FindKeys("*.Object_*.png"))
            {
                _surfaces.Add(Image.LoadFromResource(key));
            }
        }



        public SurfaceCell()
        {
            this.Enabled = false;
            this.BorderSize = 0;
            this.Shadow = ShadowStyle.Normal5;
            this.ImageAlignment = Alignment.Zoom;
            this.Image = _surfaces[_random.Next(0, _surfaces.Count)];
            this.Flip = _random.Next(0, 2) == 1 ? ImageFlip.Horizontal : ImageFlip.None;
        }

    }
}
