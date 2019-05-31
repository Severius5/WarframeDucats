using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarframeDucats.Cache;
using WarframeDucats.DTO;

namespace WarframeDucats.Services
{
	public class CoreService
	{
		private readonly MarketProvider _marketProvider;

		public CoreService(MarketProvider marketProvider)
		{
			_marketProvider = marketProvider ?? throw new ArgumentNullException(nameof(marketProvider));
		}

		public async Task RefreshDucatsCache()
		{
			if (!CacheProvider.IsInitialized)
				await CacheProvider.Initialize(_marketProvider);

			var ducatsInfos = await _marketProvider.GetItemsDucatInfos();
			CacheProvider.Ducats = ducatsInfos
				.OrderByDescending(x => x.DucatsPerPlat)
				.Take(15)
				.Select(x =>
				{
					var item = CacheProvider.Items[x.ItemId];
					return new DucatCacheModel
					{
						ItemId = x.ItemId,
						ItemName = item.Name,
						Slug = item.Slug,
						Ducats = x.Ducats,
						DucatsPerPlat = x.DucatsPerPlat
					};
				})
				.ToList();
		}

		public async Task<List<Order>> GetItemsOrders()
		{
			var ducats = CacheProvider.Ducats;
			if (ducats == null)
			{
				await RefreshDucatsCache();
				ducats = CacheProvider.Ducats;
			}

			var orders = new ConcurrentBag<Order>();
			var tasks = new List<Task>(ducats.Count);

			foreach (var ducat in ducats)
				tasks.Add(GetItemOrders(ducat)
					.ContinueWith(x => x.Result.ForEach(orders.Add)));

			await Task.WhenAll(tasks);

			return orders.ToList();
		}

		public async Task<List<Order>> GetUserOrders(string username)
		{
			if (!CacheProvider.IsInitialized)
				await CacheProvider.Initialize(_marketProvider);

			var orders = new List<Order>();
			var userOrders = await _marketProvider.GetUserOrders(username);

			foreach (var userOrder in userOrders)
			{
				var item = CacheProvider.Items.First(x => x.Value.Name.Equals(userOrder.ItemName, StringComparison.OrdinalIgnoreCase)).Value;
				if (userOrder.Platinum > GetMaxPlatPerDucats(item.Ducats))
					continue;

				orders.Add(new Order
				{
					Ducats = item.Ducats,
					ItemName = userOrder.ItemName,
					Platinum = userOrder.Platinum,
					Quantity = userOrder.Quantity,
					Username = username
				});
			}

			return orders;
		}

		private async Task<List<Order>> GetItemOrders(DucatCacheModel ducat)
		{
			var maxPlat = GetMaxPlatPerDucats(ducat.Ducats);
			var itemOrders = await _marketProvider.GetItemOrders(ducat.Slug);

			return itemOrders
				.Where(x => x.Platinum <= maxPlat)
				.Select(x => new Order
				{
					Ducats = ducat.Ducats,
					ItemName = ducat.ItemName,
					Platinum = x.Platinum,
					Quantity = x.Quantity,
					Username = x.Username
				})
				.ToList();
		}

		private int GetMaxPlatPerDucats(int ducats)
		{
			switch (ducats)
			{
				case 45: return 3;
				case 100: return 6;
				default: return -1;
			}
		}
	}
}
