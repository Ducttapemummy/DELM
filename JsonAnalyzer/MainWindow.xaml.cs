using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JsonAnalyzer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Model _model = new Model();
		public MainWindow()
		{
			this.DataContext = _model;
			Closing += _model.DoClosingStuff;
			InitializeComponent();
		}

		private void CharBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_model.CharSelectionChanged();
		}
    }
}
