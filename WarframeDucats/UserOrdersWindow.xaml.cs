using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WarframeDucats.Services;
using WarframeDucats.ViewModels;

namespace WarframeDucats
{
	/// <summary>
	/// Interaction logic for UserOrdersWindow.xaml
	/// </summary>
	public partial class UserOrdersWindow : Window
	{
		private readonly CoreService _service;
		private readonly string _username;

		public UserOrdersWindow(string username)
		{
			_username = username;

			InitializeComponent();
			UserLabel.Content = username;

			_service = new CoreService(new MarketProvider(HttpClientProvider.Client));
		}

		public async Task Init()
		{
			Orders.ItemsSource = await FetchOrders();
			Loading.IsBusy = false;
		}

		private void CopyBtn_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(MsgTextBox.Text);
		}

		private void OnCheckBox(object sender, RoutedEventArgs e)
		{
			var orders = (List<OrderRow>)Orders.ItemsSource;
			var msg = MsgGenerator.Generate(orders.Where(x => x.Buy));

			MsgTextBox.Text = msg;
		}

		private async Task<List<OrderRow>> FetchOrders()
		{
			var orders = await _service.GetUserOrders(_username);

			return orders
				.Select(x => new OrderRow
				{
					Ducats = x.Ducats,
					ItemName = x.ItemName,
					Plat = x.Platinum,
					Quantity = x.Quantity,
					Username = _username
				})
				.OrderByDescending(x => x.Quantity)
				.ThenByDescending(x => x.Plat)
				.ToList();
		}
	}
}
