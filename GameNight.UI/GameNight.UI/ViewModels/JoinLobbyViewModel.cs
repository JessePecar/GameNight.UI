using GameNight.Models.Game;
using GameNight.UI.Core;
using GameNight.UI.HubClient.Interface;
using GameNight.UI.ViewModels.Games;
using GameNight.UI.Views;
using GameNight.UI.Views.Games;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GameNight.UI.ViewModels
{
    public class JoinLobbyViewModel : BaseViewModel
    {
        public JoinLobbyViewModel()
        {
            JoinClicked = new Command(async () => await ExecuteFunction(async () => await JoinGame()), () => CanExecute);
            CanExecute = true;
        }

        #region Commands

        private ICommand _joinClicked;
        public ICommand JoinClicked
        {
            get => _joinClicked;
            set
            {
                _joinClicked = value;
                RaisePropertyChange();
            }
        }

        #endregion

        #region Properties

        protected override bool CanExecute
        {
            set
            {
                base.CanExecute = value;
                JoinClicked.CanExecute(value);
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

        private string _lobbyKey;
        public string LobbyKey
        {
            get => _lobbyKey;
            set
            {
                _lobbyKey = value;
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

        private async Task JoinGame()
        {
            DependencyManager.Resolve<IHubClient>().SetupHandlerForViewModel((conn) =>
            {
                return () =>
                {
                    conn.On<GameType>("GameJoinedSuccessfully", async (gameType) =>
                    {
                        switch (response.GameType)
                        {
                            case (GameType.TableTopRPG):
                                TableTopView.ReinstateView(new TableTopViewModel(LobbyKey, UserName));
                                await DependencyManager.PushNavigation(GameView.View);
                                break;
                            case (GameType.ChooseOne):
                                ChooseOneView.ReinstateView(new ChooseOneViewModel(LobbyKey, UserName));
                                await DependencyManager.PushNavigation(ChooseOneView.View);
                                break;
                            default:
                                TableTopView.ReinstateView(new TableTopViewModel(LobbyKey, UserName));
                                await DependencyManager.PushNavigation(TableTopView.View);
                                break;
                        }
                    });
                };
            });

            GameView.ReinstateView(new GameViewModel(LobbyKey, UserName));
            await DependencyManager.Resolve<IHubClient>().JoinGame(LobbyKey, Password, UserName);
        }
    }
}
