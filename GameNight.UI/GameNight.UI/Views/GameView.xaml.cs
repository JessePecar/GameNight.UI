using GameNight.UI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

namespace GameNight.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameView : ContentPage
    {
        private GameViewModel _viewModel;
        private static GameView _view;
        public GameView(GameViewModel viewModel)
        {
            _view = this;
            _view.InitializeComponent();
            _view.SetViewModel(viewModel);
            viewModel.ScrollToBottom = () => ScrollToBottom();
            _view.BindingContext = _viewModel;
        }

        public static void ReinstateView(GameViewModel viewModel)
        {
            _view = new GameView(viewModel);
        }

        public void SetViewModel(GameViewModel viewModel) => _viewModel = viewModel;

        public static GameView View => _view;

        private void ScrollToBottom()
        {
            lv_ListView.ScrollTo(_viewModel.TurnLogs.LastOrDefault(), ScrollToPosition.End, true);
        }
    }
}