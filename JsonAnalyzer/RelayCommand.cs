using System;
using System.ComponentModel;
using System.Windows.Input;

namespace JsonAnalyzer
{
    public class RelayCommand : ICommand, INotifyPropertyChanged
    {
        /*Tinus zusammenkopierte Klasse für automatische Updates auf dem Gui
         * z.B. Soll ein Knopf nur bei auswahl einer Checkbox enabled sein, mit einem Relaycommand prüft das Gui selber ob der Knopf aktiv sein darf,
         * resp. ob die Checkbox aktiviert wurde(Beispiel AGB)
         * Oder wenn in einem Textfeld mindestens 8 chars stehen müssen bis die Bestätigung zur Verfügung steht, prüft das Gui selbstständig,
         * wie lange der Text in diesem Textfeld ist, und gibt ab einer Länge von >=8 die Bestätigung frei(Beispiel Passworteingabe)
         */

        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, String commandText, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            Text = commandText;
        }

        public bool CanExecute(object parameter)
        {
			return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute.Invoke(parameter);
        }

        private String text = String.Empty;
        public String Text
        {
            get { return text; }
            set
            {
                text = value;
                NotifyChange("Text");
            }
        }

        private void NotifyChange(params object[] properties)
        {
            if (PropertyChanged != null)
            {
                foreach (string p in properties) PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }
    }
}


