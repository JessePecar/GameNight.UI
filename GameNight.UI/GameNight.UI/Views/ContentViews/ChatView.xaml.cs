using GameNight.UI.ViewModels.ContentViewModels;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameNight.UI.Views.ContentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatView : ContentView
    {
        private ChatViewModel _viewModel;
        
        public ChatView(ChatViewModel viewModel)
        {
            InitializeComponent();
            SetViewModel(viewModel);

            _viewModel.ScrollToBottom = () => ScrollToBottom();

            BindingContext = _viewModel;
        }

        public void SetViewModel(ChatViewModel viewModel) => _viewModel = viewModel;

        private void ScrollToBottom()
        {
            lv_ListView.ScrollTo(_viewModel.TurnLogs.LastOrDefault(), ScrollToPosition.End, true);
        }
    }
}