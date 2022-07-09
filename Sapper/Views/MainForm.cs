using System.Collections.Generic;
using LX;
using Sapper.Models;

namespace Sapper.Views
{
    class MainForm : Control
    {
        public MainForm()
        {
            Window.Title = "Sapper LXUI.NET";

            // add hotkeys for exit app
            this.StartHotKey(Key.Escape).Press += delegate { LX.App.Exit(); return true; };
            this.StartHotKey(Key.BrowserBack).Press += delegate { LX.App.Exit(); return true; };

            this.Color = Color.Primary.Light(50);
            this.Padding = 8;
            this.AddToRoot(Alignment.Fill);

            var mapView = new MapView();
            mapView.AddTo(this, Alignment.Center);

            var header = new Control();
            header.Layout = new HorizontalList(8);
            header.HorizontalScrollBar.Height = 2;
            header.Shadow = ShadowStyle.Bottom2;
            header.AutoHeight = true;
            header.Color = Color.Primary;
            header.Padding = 8;
            header.AddTo(this, Alignment.TopFill | Alignment.NotLayouted);
            header.OnSizeChanged += delegate { this.PaddingTop = (int)header.Height + 8; };

            var lives = header.Add("0", Alignment.TopCenter);
            lives.Add(Image.LoadIcon(105), Alignment.LeftCenter).ImageColor = Color.Red;

            lives.StartTimer(100).Tick += delegate
            {
                lives.Text = mapView.Map.Lives.ToString();
            };

            var sizes = new List<Size>();
            sizes.Add(new LX.Size(7, 7));
            sizes.Add(new LX.Size(7, 15));
            sizes.Add(new LX.Size(7, 50));
            sizes.Add(new LX.Size(10, 15));
            sizes.Add(new LX.Size(10, 50));
            sizes.Add(new LX.Size(15, 15));
            sizes.Add(new LX.Size(15, 50));
            sizes.Add(new LX.Size(20, 15));
            sizes.Add(new LX.Size(20, 50));

            foreach (Size size in sizes)
            {
                var sizeButton = new Button();
                sizeButton.Text = $"{size.Width}x{size.Height}";
                sizeButton.AddTo(header, Alignment.TopCenter);
                sizeButton.OnClick += delegate
                {
                    var map = new Map(3, size.Width, size.Height);
                    mapView.Map = map;
                    this.ScrollX = 0;
                    this.ScrollY = 0;
                };

                if (size.Width == 7 && size.Height == 7)
                {
                    sizeButton.Click();
                    sizeButton.Focused = true;
                }
            }

        }

    }
}
