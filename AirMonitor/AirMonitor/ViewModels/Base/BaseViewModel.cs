using System.ComponentModel;

namespace AirMonitor.ViewModels
{
    /// <summary>
    /// Base View Model that implements <see cref="INotifyPropertyChanged"/> for proper Data Binding.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged

        public new event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Invoked on a property changed in the View Models and Pages. Allows two way data binding.
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion PropertyChanged
    }
}