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
        public ChooseOneView(ChooseOneViewModel viewModel)
        {
            _view = this;
            _view.InitializeComponent();
            _view.SetViewModel(viewModel);
            _view.BindingContext = _viewModel;        
        }

        public static void ReinstateView(ChooseOneViewModel viewModel) => new ChooseOneView(viewModel);
        public ChooseOneViewModel ViewModel => _viewModel;

        public static ChooseOneView View => _view;
        private void SetViewModel(ChooseOneViewModel viewModel) => _viewModel = viewModel;
    }
}