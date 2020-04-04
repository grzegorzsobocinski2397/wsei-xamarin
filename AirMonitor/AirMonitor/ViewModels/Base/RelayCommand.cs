using System;
using System.Windows.Input;

namespace AirMonitor.ViewModels.Base
{
    /// <summary>
    /// Base Command that implements <see cref="ICommand"/> for correct functionality.
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Method on the View Model.
        /// </summary>
        private readonly Action action;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        /// <summary>
        /// Base Command that implements <see cref="ICommand"/> for correct functionality.
        /// </summary>
        /// <param name="action">A method to invoke on a View Model.</param>
        public RelayCommand(Action action)
        {
            this.action = action;
        }

        /// <summary>
        /// Command can always execute.
        /// </summary>
        /// <param name="parameter">In this case it's always null.</param>
        /// <returns>Always true, because in our case it doesn't matter.</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Invoke the action (method) on the view model.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            action();
        }
    }
}