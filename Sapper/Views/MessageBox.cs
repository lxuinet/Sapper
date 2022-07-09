using LX;

namespace Sapper.Views
{
    class MessageBox : Control
    {
        public MessageBox(string text)
        {
            this.Color = Color.Primary.Alpha(100);
            this.UserMouse = UserMode.On;

            var content = new Control();
            content.Padding = 16;
            content.BorderSize = 1;
            content.Shape = CornerShape.Oval;
            content.Radius = 3;
            content.Color = Color.Primary;
            content.AutoSize = true;
            content.Layout = new VerticalList(16);
            content.AddTo(this, Alignment.Center);

            content.Add(text, Alignment.TopCenter);


            var button = new Button();
            button.Text = "Close";
            button.CanFocus = false;
            button.AddTo(content, Alignment.TopCenter);
            button.OnClick += delegate
            {
                this.Remove();
            };
        }

        public static void ShowDialog(string text)
        {
            var dialod = new MessageBox(text);
            dialod.AddToRoot(Alignment.Fill);
        }
    }
}
