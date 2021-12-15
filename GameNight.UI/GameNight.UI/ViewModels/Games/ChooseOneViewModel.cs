using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GameNight.UI.ViewModels.Games
{
    public class ChooseOneViewModel : BaseViewModel
    {
        public ChooseOneViewModel(string lobbyCode, string userName, Guid? adminKey = null)
        {
            _lobbyCode = lobbyCode;
            _userName = userName;
            _adminKey = adminKey;

            SubmitLobbyPrompt = new Command(async () => await SubmitPromptToLobby(), () => CanExecute);
            SubmitJudgePrompt = new Command(async () => await SubmitPromptToJudge(), () => CanExecute);
            
            CanExecute = true;
            CanSubmit = true;
        }

        #region Properties

        protected override bool CanExecute
        {
            set
            {
                base.CanExecute = value;
                SubmitJudgePrompt.CanExecute(value);
                SubmitLobbyPrompt.CanExecute(value);
                RaisePropertyChange();
            }
        }

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

        private bool _canSubmit;
        public bool CanSubmit
        {
            get => _canSubmit;
            set
            {
                _canSubmit = value;
                RaisePropertyChange();
            }
        }

        private ICommand _submitLobbyPrompt;
        public ICommand SubmitLobbyPrompt
        {
            get => _submitLobbyPrompt;
            set
            {
                if (value == null) return;
                _submitLobbyPrompt = value;
                RaisePropertyChange();
            }
        }

        private ICommand _submitJudgePrompt;
        public ICommand SubmitJudgePrompt
        {
            get => _submitJudgePrompt;
            set
            {
                if (value == null) return;
                _submitJudgePrompt = value;
                RaisePropertyChange();
            }
        }

        private string _prompt;
        public string Prompt
        {
            get => _prompt;
            set
            {
                _prompt = value;
                RaisePropertyChange();
            }
        }

        private string _answerPrompt;
        public string AnswerPrompt
        {
            get => _answerPrompt;
            set
            {
                _answerPrompt = value;
                RaisePropertyChange();
            }
        }

        #endregion

        #region Private Methods

        private async Task SubmitPromptToLobby()
        {

        }

        private async Task SubmitPromptToJudge()
        {

        }

        #endregion
    }
}
