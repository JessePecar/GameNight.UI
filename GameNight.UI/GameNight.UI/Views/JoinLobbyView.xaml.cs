using GameNight.UI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameNight.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JoinLobbyView : ContentPage
    {
        private JoinLobbyViewModel _viewModel;
        private static JoinLobbyView _view;
        public JoinLobbyView()
        {
            _view = this;
            _view.InitializeComponent();
            _view.SetViewModel(new JoinLobbyViewModel());

            _view.BindingContext = _viewModel;
        }
        public static void ReinstateView() => _view = new JoinLobbyView();
        public static JoinLobbyView View => _view;

        public void SetViewModel(JoinLobbyViewModel viewModel) => _viewModel = viewModel;
    }
}