using GameNight.UI.ViewModels.Games;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameNight.UI.Views.Games
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TableTopView : ContentPage
    {
        private TableTopViewModel _viewModel;
        private static TableTopView _view;

        public TableTopView(TableTopViewModel viewModel)
        {
            _view = this;
            _view.InitializeComponent();
            _view.SetViewModel(viewModel);

            _view.BindingContext = _viewModel;
        }

        public static void ReinstateView(TableTopViewModel viewModel) => _view = new TableTopView(viewModel);

        public void SetViewModel(TableTopViewModel viewModel) => _viewModel = viewModel;

        public static TableTopView View => _view;

    }
}