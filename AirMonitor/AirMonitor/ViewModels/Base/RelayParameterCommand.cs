using System;
using System.Windows.Input;

namespace AirMonitor.ViewModels.Base
{
    public class RelayParameterCommand : ICommand

    {
        #region Private Members
        /// <summary>
        /// The action to run
        /// </summary>
        private Action<object> action;
        #endregion

        #region Public Events
        /// <summary>
        /// The event thats fired when the <see cref="CanExecute(object)"/> value has changed
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion


        #region Constructor
        /// <summary>
        /// Default constrcutor
        /// </summary>
        /// <param name="action"></param>
        public RelayParameterCommand(Action<object> action)
        {
            this.action = action;
        }
        #endregion

        #region Command Methods
        /// <summary>
        /// A relay command can always execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }
        /// <summary>
        /// Executes the commands action
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            action(parameter);
        }
        #endregion

    }
}
