using GameNight.UI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameNight.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateLobbyView : ContentPage
    {
        private CreateLobbyViewModel _viewModel;
        private static CreateLobbyView _view;
        public CreateLobbyView()
        {
            _view = this;
            _view.InitializeComponent();
            _view.SetViewModel(new CreateLobbyViewModel());
            
            _view.BindingContext = _viewModel;
        }

        public static void ReinstateView()
        {
            _view = new CreateLobbyView();
        }

        public static CreateLobbyView View => _view;

        public void SetViewModel(CreateLobbyViewModel viewModel) => _viewModel = viewModel;
    }
}