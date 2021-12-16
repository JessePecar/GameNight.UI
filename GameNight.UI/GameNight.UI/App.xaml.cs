using GameNight.UI.Core;
using GameNight.UI.ViewModels;
using GameNight.UI.Views;
using System;
using System.IO;
using Xamarin.Forms;
using static System.Environment;

namespace GameNight.UI
{
    public partial class App : Application
    {
        public static Guid DeviceKey { get; private set; }
        public App()
        {
            InitializeComponent();

            HomeView view = new HomeView();
            MainPage = new NavigationPage(view);

            LoadDeviceKey();

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

        private void LoadDeviceKey()
        {
            string filePath = Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), "DeviceKey.txt");
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    DeviceKey = Guid.Parse(sr.ReadToEnd());
                }
            }
            DeviceKey = Guid.NewGuid();
            File.Create(filePath);
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine(DeviceKey);
            }

        }
    }
}
