using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace GameNight.UI.ViewModels
{
    public class BaseViewModel : BindableObject
    {
        public void RaisePropertyChange([CallerMemberName] string property = "")
        {
            OnPropertyChanged(property);
        }

        private bool _canExecute;
        protected virtual bool CanExecute
        {
            get => _canExecute;
            set
            {
                _canExecute = value;
            }
        }
    }
}
