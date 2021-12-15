using GameNight.UI.Core;
using GameNight.UI.ViewModels;
using GameNight.UI.Views;
using Xamarin.Forms;

namespace GameNight.UI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            HomeView view = new HomeView();
            MainPage = new NavigationPage(view);


            DependencyManager.RegisterDependencies();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
