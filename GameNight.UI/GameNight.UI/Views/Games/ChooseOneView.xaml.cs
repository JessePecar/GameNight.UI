using GameNight.UI.ViewModels.Games;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameNight.UI.Views.Games
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseOneView : ContentPage
    {
        private ChooseOneViewModel _viewModel;
        private static ChooseOneView _view;
        public ChooseOneView()
        {
            _view = this;
            _view.InitializeComponent();
            _view.SetViewModel(new ChooseOneViewModel());
            _view.BindingContext = _viewModel;        
        }

        public void ReinstateView() => new ChooseOneView();
        public ChooseOneViewModel ViewModel => _viewModel;

        private void SetViewModel(ChooseOneViewModel viewModel) => _viewModel = viewModel;
    }
}