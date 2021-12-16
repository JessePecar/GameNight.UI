using GameNight.Core.Controllers;
using GameNight.Models.Dice;
using GameNight.Models.EnumUtils;
using GameNight.Models.LobbyLog;
using GameNight.UI.Core;
using GameNight.UI.HubClient.Interface;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using GameNight.UI.ViewModels.ContentViewModels;
using GameNight.UI.Views.ContentViews;

namespace GameNight.UI.ViewModels.Games
{
    public class TableTopViewModel : BaseViewModel
    {
        public TableTopViewModel(string lobbyCode, string userName, Guid? adminKey = null)
        {
            _lobbyCode = lobbyCode;
            _userName = userName;
            _adminKey = adminKey;

            PopulateCurrentDice();

            DependencyManager.Resolve<IHubClient>().SetupHandlerForViewModel(conn =>
            {
                conn.On<string, DiceResult>("SendDetails", (user, result) => AddTurnLog(user, result));
            });

            SubmitForRoll = new Command(async () => await RollDice(), () => CanExecute);

            ChatViewModel = new ChatViewModel();
            ChatLogView = new ChatView(ChatViewModel);

            CanExecute = true;
        }

        #region Properties

        private readonly Regex _regex = new Regex("/[0-9]/g");
        private readonly string _lobbyCode;
        public string LobbyCode
        {
            get => _lobbyCode;
        }

        private readonly string _userName;
        public string UserName
        {
            get => _userName;
        }

        private readonly Guid? _adminKey;
        public Guid? AdminKey
        {
            get => _adminKey;
        }

        private ObservableCollection<DiceType> _diceTypes = new ObservableCollection<DiceType>();
        public ObservableCollection<DiceType> DiceTypes
        {
            get => _diceTypes;
            set
            {
                if (value == null) return;
                _diceTypes = value;
                RaisePropertyChange();
            }
        }

        private DiceType _selectedDiceType = DiceType.D20;
        public DiceType SelectedDiceType
        {
            get => _selectedDiceType;
            set
            {
                _selectedDiceType = value;
                RaisePropertyChange();
            }
        }

        private ICommand _submitForRoll;
        public ICommand SubmitForRoll
        {
            get => _submitForRoll;
            set
            {
                _submitForRoll = value;
                RaisePropertyChange();
            }
        }

        private string _numberOfDice { get; set; } = "1";
        public string NumberOfDice
        {

            get => _numberOfDice;
            set
            {
                value = _regex.Replace(value, "");
                value = value.Replace(".", "").Replace("-", "");
                _numberOfDice = value;

                if (int.TryParse(value, out int val) && val > 99)
                {
                    _numberOfDice = "99";
                }
                RaisePropertyChange();
            }
        }

        private ChatViewModel _chatViewModel;
        public ChatViewModel ChatViewModel
        {
            get => _chatViewModel;
            set
            {
                if (value == null) return;
                _chatViewModel = value;
                RaisePropertyChange();
            }
        }

        private ContentView _chatLogView;
        public ContentView ChatLogView
        {
            get => _chatLogView;
            set
            {
                if (value == null) return;
                _chatLogView = value;
                RaisePropertyChange();
            }
        }
        protected override bool CanExecute
        {
            set
            {
                base.CanExecute = value;
                SubmitForRoll.CanExecute(value);
                RaisePropertyChange();
            }
        }

        #endregion

        #region Public Methods

        public void AddTurnLog(string user, DiceResult result)
        {
            ChatViewModel.AddTurnLog(
                user, 
                user.Equals(UserName, StringComparison.OrdinalIgnoreCase), 
                $"Rolled {result.RollCount} {result.Type.ToString()} for: {Environment.NewLine} {string.Join(", ", result.Rolls.Select(r => r.ToString()))}");

            RaisePropertyChange(nameof(ChatViewModel));
        }

        #endregion

        #region Private Methods

        private async Task RollDice()
        {
            DiceResult result = await DependencyManager.Resolve<DiceController>().DndRoll(int.Parse(NumberOfDice), SelectedDiceType);

            await DependencyManager.Resolve<IHubClient>().SendPlayersDetails(LobbyCode, UserName, result);

            AddTurnLog(UserName, result);
        }

        private void PopulateCurrentDice()
        {
            DiceTypes = new ObservableCollection<DiceType>(typeof(DiceType).ToList<DiceType>());
        }

        #endregion
    }
}
