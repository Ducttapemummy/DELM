using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace JsonAnalyzer
{
	class UI_helper: INotifyPropertyChanged
	{

		#region -- Notify --
		protected void SetAndNotify<T>(ref T var, T value, string additionalProperty = null, [CallerMemberName] string propertyName = "")
		{
			if (!EqualityComparer<T>.Default.Equals(var, value))
			{
				var = value;
				NotifyChange(propertyName);
				if (additionalProperty != null) NotifyChange(additionalProperty);
			}
		}
		protected void NotifyChange([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			CommandManager.InvalidateRequerySuggested();
		}
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
}
