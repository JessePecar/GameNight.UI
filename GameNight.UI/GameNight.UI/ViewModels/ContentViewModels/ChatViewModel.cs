using GameNight.Models.Dice;
using GameNight.Models.LobbyLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GameNight.UI.ViewModels.ContentViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        public ChatViewModel()
        {
            TurnLogs = new ObservableCollection<ChatLog>();
        }

        #region Properties

        private ObservableCollection<ChatLog> _turnLogs = new ObservableCollection<ChatLog>();
        public ObservableCollection<ChatLog> TurnLogs
        {
            get => _turnLogs;
            set
            {
                _turnLogs = value;
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

        public void AddTurnLog(string user, bool isPlayer, string message)
        {
            List<ChatLog> turns = TurnLogs.ToList();

            turns.Add(new ChatLog
            {
                User = user,
                TurnResult = message,
                IsPlayer = isPlayer
            });

            TurnLogs = new ObservableCollection<ChatLog>(turns);
            ScrollToBottom?.Invoke();
        }

        #endregion
    }
}
