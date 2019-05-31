using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WarframeDucats.Converters;
using WarframeDucats.Services;
using WarframeDucats.ViewModels;

namespace WarframeDucats
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly DispatcherTimer _timer = new DispatcherTimer();
		private readonly CoreService _service;

		public MainWindow()
		{
			InitializeComponent();
			InitJsonSettings();
			InitTimer();

			_service = new CoreService(new MarketProvider(HttpClientProvider.Client));

			_ = UpdateOrders();
		}

		private void InitJsonSettings()
		{
			var settings = new JsonSerializerSettings();
			settings.Converters.Add(new TolerantEnumConverter());

			JsonConvert.DefaultSettings = () => settings;
		}

		private void InitTimer()
		{
			_timer.Tick += OnFetchOrders;
			_timer.Interval = TimeSpan.FromMinutes(5);
			_timer.Start();
		}

		private async void OnFetchOrders(object sender, EventArgs e)
		{
			await UpdateOrders().ConfigureAwait(false);
		}

		private async void RefreshBtn_Click(object sender, RoutedEventArgs e)
		{
			await UpdateOrders().ConfigureAwait(false);
		}

		private void GenerateSingleMsgBtn_Click(object sender, RoutedEventArgs e)
		{
			var order = (OrderRow)((Button)e.Source).DataContext;
			MsgTextBox.Text = MsgGenerator.Generate(order);
		}

		private void CopyBtn_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(MsgTextBox.Text);
		}

		private async void Orders_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (Orders.SelectedItem == null)
				return;

			if (!(Orders.SelectedItem is OrderRow order))
				return;

			var userOdersWindow = new UserOrdersWindow(order.Username);
			userOdersWindow.Show();
			await userOdersWindow.Init().ConfigureAwait(false);
		}

		private async Task UpdateOrders()
		{
			Loading.IsBusy = true;
			Orders.ItemsSource = await FetchOrders();
			Loading.IsBusy = false;
		}

		private async Task<List<OrderRow>> FetchOrders()
		{
			var orders = await _service.GetItemsOrders();

			return orders
				.Select(x => new OrderRow
				{
					Ducats = x.Ducats,
					ItemName = x.ItemName,
					Plat = x.Platinum,
					Quantity = x.Quantity,
					Username = x.Username
				})
				.OrderByDescending(x => x.Quantity)
				.ThenByDescending(x => x.Plat)
				.ToList();
		}
	}
}
