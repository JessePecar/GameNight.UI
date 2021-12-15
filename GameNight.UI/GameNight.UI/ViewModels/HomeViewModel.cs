using GameNight.Core.Controllers;
using GameNight.Models.Game;
using GameNight.UI.Core;
using GameNight.UI.HubClient.Interface;
using GameNight.UI.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GameNight.UI.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            JoinClicked = new Command(async () =>
            {
                JoinLobbyView.ReinstateView();
                await DependencyManager.PushNavigation(JoinLobbyView.View);
            }, () => CanExecute);
            CreateClicked = new Command(async () =>
            {
                CreateLobbyView.ReinstateView();
                await DependencyManager.PushNavigation(CreateLobbyView.View);
            }, () => CanExecute);

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

        #endregion

    }
}
