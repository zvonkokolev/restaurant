using Restaurant.Core;
using System;
using System.Diagnostics;

namespace Restaurant.Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private const string _pfad = "Tasks.csv";

		public MainWindow()
		{
			InitializeComponent();
		}
		private void MetroWindow_Initialized(object sender, EventArgs e)
		{
			Title = $"restaurantsimulator, uhrzeit: {(FastClock.Instance.Time = Convert.ToDateTime("12:00")).ToShortTimeString()}";
			FastClock.Instance.IsRunning = true;
			FastClock.Instance.OneMinuteIsOver += OnOneMinuteIsOverFromFastClock;
			Waiter.Instance.NewOrder += OnNewOrderFromWaiter;
			Waiter.Instance.GetOrder(_pfad);
		}
		private void OnOneMinuteIsOverFromFastClock(object source, DateTime time)
		{
			Title = $"restaurantsimulator, uhrzeit: {time.ToShortTimeString()}";  // (FastClock.Instance.Time)
		}
		private void OnNewOrderFromWaiter(object source, Order order)
		{
			TextBlockLog.Inlines.Add(order.ToString());
			TextBlockLog.Inlines.Add("\n");
			Debug.WriteLine(order.ToString());
		}
	}
}
