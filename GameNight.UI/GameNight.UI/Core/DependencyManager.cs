using GameNight.Core.Controllers;
using GameNight.Core.Http;
using GameNight.Core.Http.Interfaces;
using GameNight.UI.HubClient.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Unity;
using Unity.Microsoft.DependencyInjection;
using Xamarin.Forms;

namespace GameNight.UI.Core
{
    public static class DependencyManager
    {
        private static UnityContainer _container = new UnityContainer();

        public static void RegisterDependencies()
        {
            _container.RegisterType<IGameHttpClient, GameHttpClient>();

            _container.BuildServiceProvider(BuildServiceCollection());
        }

        public static IServiceCollection BuildServiceCollection()
        {
            IServiceCollection collection = new ServiceCollection();

            collection.AddHttpClient("GameNight", (client) =>
            {
                client.BaseAddress = new Uri("http://gamenight.jessepecar.com");
            });

            collection.AddSingleton<IHubClient>(new HubClient.HubClient());
            collection.AddSingleton<IGameHttpClient, GameHttpClient>();
            collection.AddSingleton<GameController>();
            collection.AddSingleton<DiceController>();
            return collection;
        }

        public static TResolve Resolve<TResolve>()
        {
            return _container.Resolve<TResolve>();
        }

        public static async Task PushNavigation(ContentPage page)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {

                if (App.Current.MainPage.Navigation.NavigationStack.Any(ns => ns.GetType() == page.GetType()))
                {
                    App.Current.MainPage.Navigation.RemovePage(page.GetType().GetProperty("View").GetValue(page) as Page);
                }

                await App.Current.MainPage.Navigation.PushAsync(page);
            });
        }
    }
}
