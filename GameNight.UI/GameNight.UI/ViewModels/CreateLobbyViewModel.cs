using GameNight.Core.Controllers;
using GameNight.Models.Game;
using GameNight.UI.Core;
using GameNight.UI.HubClient.Interface;
using GameNight.UI.ViewModels.Games;
using GameNight.UI.Views;
using GameNight.UI.Views.Games;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GameNight.UI.ViewModels
{
    public class CreateLobbyViewModel : BaseViewModel
    {
        public CreateLobbyViewModel()
        {
            CreateClicked = new Command(async () => await ExecuteFunction(async () => await CreateNewGame()), () => CanExecute);

            CanExecute = true;
        }

        #region Properties

        private ICommand _createClicked;
        public ICommand CreateClicked
        {
            get => _createClicked;
            set
            {
                _createClicked = value;
                RaisePropertyChange();
            }

        }

        protected override bool CanExecute
        {
            set
            {
                base.CanExecute = value;
                CreateClicked.CanExecute(value);
                RaisePropertyChange();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChange();
            }
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                RaisePropertyChange();
            }
        }

        #endregion

        private async Task ExecuteFunction(Func<Task> execute)
        {
            CanExecute = false;
            await execute.Invoke();
            CanExecute = true;
        }

        private Dictionary<int, GameType> GetEnumDictionary()
        {
            return Enum.GetValues(typeof(GameType)).Cast<GameType>().Select(gt => ((int)gt, gt)).ToDictionary(d => d.Item1, d => d.gt);

        }
        private async Task CreateNewGame()
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Invalid information entered", "Ok");
                return;
            }

            GameManager response = await DependencyManager.Resolve<GameController>().InitializeNewGame(Password);

            if (response != null)
            {
                DependencyManager.Resolve<IHubClient>().SetupHandlerForViewModel((conn) =>
                {
                    conn.On<int>("GameJoinedSuccessfully", async (gameTypeId) =>
                    {
                        GameType gameType = GetEnumDictionary().GetValueOrDefault(gameTypeId);
                        switch (gameType)
                        {
                            case (GameType.TableTopRPG):
                                TableTopView.ReinstateView(new TableTopViewModel(response.LobbyKey, UserName, response.AdminKey));
                                await DependencyManager.PushNavigation(TableTopView.View);
                                break;
                            case (GameType.ChooseOne):
                                GameView.ReinstateView(new GameViewModel(response.LobbyKey, UserName, response.AdminKey));
                                await DependencyManager.PushNavigation(GameView.View);
                                break;
                            default:
                                TableTopView.ReinstateView(new TableTopViewModel(response.LobbyKey, UserName, response.AdminKey));
                                await DependencyManager.PushNavigation(TableTopView.View);
                                break;
                        }

                    });
                });

                await DependencyManager.Resolve<IHubClient>().JoinGame(response.LobbyKey, Password, UserName);

            }
        }
    }
}
