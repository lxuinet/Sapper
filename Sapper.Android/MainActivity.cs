using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content.PM;
using LX;
using Sapper.Views;

namespace Sapper.Android
{
    [Activity(Label = "@string/app_name", 
        Theme = "@style/AppTheme", 
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class MainActivity : AndroidActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            App.OnRun += () => new MainForm();
            base.OnCreate(savedInstanceState);
        }
    }
}