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
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GameNight.UI.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        public GameViewModel(string lobbyCode, string userName, Guid? adminKey = null)
        {
            _lobbyCode = lobbyCode;
            _userName = userName;
            _adminKey = adminKey;

            TurnLogs = new ObservableCollection<TurnLog>();
            PopulateCurrentDice();

            DependencyManager.Resolve<IHubClient>().SetupHandlerForViewModel(conn =>
            {
                return () => conn.On<string, DiceResult>("SendDetails", (user, result) => AddTurnLog(user, result));
            });
            SubmitForRoll = new Command(async () => await RollDice(), () => CanExecute);

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

        private ObservableCollection<TurnLog> _turnLogs;
        public ObservableCollection<TurnLog> TurnLogs
        {
            get => _turnLogs;
            set
            {
                _turnLogs = value;
                RaisePropertyChange();
            }
        }

        private ObservableCollection<DiceType> _diceTypes;
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
        public string NumberOfDice {

            get => _numberOfDice;
            set
            {
                value = _regex.Replace(value, "");
                value = value.Replace(".", "").Replace("-", "");
                _numberOfDice = value;

                if(int.TryParse(value, out int val) && val > 99)
                {
                    _numberOfDice = "99";
                }
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

        private Action _scrollToBottom;
        public Action ScrollToBottom
        {
            get => _scrollToBottom;
            set
            {
                if (value == null) return;
                _scrollToBottom = value;
                RaisePropertyChange();
            }
        }
        #endregion

        #region Public Methods

        public void AddTurnLog(string user, DiceResult result)
        {
            List<TurnLog> turns = TurnLogs.ToList();

            turns.Add(new TurnLog
            {
                User = user,
                TurnResult = $"Rolled {result.RollCount} {result.Type.ToString()} for: {Environment.NewLine} {string.Join(", ", result.Rolls.Select(r => r.ToString()))}",
                IsPlayer = user.Equals(UserName, StringComparison.OrdinalIgnoreCase)
            });

            TurnLogs = new ObservableCollection<TurnLog>(turns);
            ScrollToBottom?.Invoke();
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
