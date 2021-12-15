using GameNight.UI.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameNight.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentPage
    {
        private HomeViewModel _viewModel;
        private static HomeView _view;
        public HomeView()
        {
            _view = this;
            _view.InitializeComponent();
            _view.SetViewModel(new HomeViewModel());

            _view.BindingContext = _viewModel;
        }

        public void ReinstateView() => _view = new HomeView();
        public static HomeView View => _view;
        public void SetViewModel(HomeViewModel viewModel) => _viewModel = viewModel;
    }
}