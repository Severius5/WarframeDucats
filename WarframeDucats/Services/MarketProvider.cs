using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WarframeDucats.Cache;
using WarframeDucats.DTO;
using WarframeDucats.Models;

namespace WarframeDucats.Services
{
	public class MarketProvider
	{
		private const string ItemsEndpoint = "items";
		private const string DucatsEndpoint = "tools/ducats";
		private const string ItemOrdersEndpointTemplate = "items/{0}/orders";
		private const string UserOrdersEndpointTemplate = "profile/{0}/orders";

		private readonly HttpClient _httpClient;

		public MarketProvider(HttpClient httpClient)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
		}

		public async Task<List<UserOrder>> GetUserOrders(string username)
		{
			var response = await _httpClient.GetAsync(string.Format(UserOrdersEndpointTemplate, username)).ConfigureAwait(false);
			var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var models = JsonConvert.DeserializeObject<BaseResponse<UserOrdersResponse>>(json);

			return models.Payload.Sell
				.Where(x =>
					x.Visible &&
					x.Region == Region.En &&
					x.Platform == Platform.Pc &&
					x.OrderType == OrderType.Sell &&
					CanExchangeToDucats(x.Item.ItemId))
				.Select(x => new UserOrder
				{
					Platinum = (int)x.Platinum,
					Quantity = x.Quantity,
					ItemName = x.Item.En.ItemName
				})
				.ToList();
		}

		public async Task<List<DTO.ItemOrder>> GetItemOrders(string itemSlug)
		{
			var response = await _httpClient.GetAsync(string.Format(ItemOrdersEndpointTemplate, itemSlug)).ConfigureAwait(false);
			var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var models = JsonConvert.DeserializeObject<BaseResponse<ItemOrdersResponse>>(json);

			return models.Payload.Orders
				.Where(x =>
					x.Visible &&
					x.Region == Region.En &&
					x.Platform == Platform.Pc &&
					x.OrderType == OrderType.Sell &&
					x.User?.Status == Status.InGame)
				.Select(x => new DTO.ItemOrder
				{
					Platinum = (int)x.Platinum,
					Quantity = x.Quantity,
					Username = x.User.Name
				})
				.ToList();
		}

		public async Task<List<ItemDucatInfo>> GetItemsDucatInfos()
		{
			var response = await _httpClient.GetAsync(DucatsEndpoint).ConfigureAwait(false);
			var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var models = JsonConvert.DeserializeObject<BaseResponse<DucatInfoResponse>>(json);

			return models.Payload.PreviousHour
				.Select(x => new ItemDucatInfo
				{
					Ducats = x.Ducats,
					DucatsPerPlat = x.DucatsPerPlat,
					ItemId = x.ItemId
				})
				.ToList();
		}

		public async Task<List<ItemInfo>> GetItemsInfos()
		{
			var response = await _httpClient.GetAsync(ItemsEndpoint).ConfigureAwait(false);
			var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var models = JsonConvert.DeserializeObject<BaseResponse<ItemsResponse>>(json);

			return models.Payload.Items
				.Select(x => new ItemInfo
				{
					Id = x.ItemId,
					Name = x.ItemName,
					Slug = x.UrlName
				})
				.ToList();
		}

		private bool CanExchangeToDucats(string itemId)
		{
			return CacheProvider.Items.TryGetValue(itemId, out _);
		}
	}
}
