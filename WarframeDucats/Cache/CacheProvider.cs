using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarframeDucats.Services;

namespace WarframeDucats.Cache
{
	public static class CacheProvider
	{
		private static readonly Dictionary<string, ItemCacheModel> _items = new Dictionary<string, ItemCacheModel>(StringComparer.OrdinalIgnoreCase);

		public static bool IsInitialized { get; private set; }

		public static List<DucatCacheModel> Ducats { get; set; }

		public static IReadOnlyDictionary<string, ItemCacheModel> Items
		{
			get
			{
				if (!IsInitialized)
					throw new InvalidOperationException("Cache must be initialized");

				return _items;
			}
		}

		public static async Task Initialize(MarketProvider marketProvider)
		{
			if (IsInitialized)
				return;

			var items = await marketProvider.GetItemsInfos();
			var ducatInfos = await marketProvider.GetItemsDucatInfos();

			foreach (var ducatInfo in ducatInfos)
			{
				var item = items.First(x => x.Id.Equals(ducatInfo.ItemId));
				_items.Add(ducatInfo.ItemId, new ItemCacheModel
				{
					Ducats = ducatInfo.Ducats,
					Id = ducatInfo.ItemId,
					Name = item.Name,
					Slug = item.Slug
				});
			}

			IsInitialized = true;
		}
	}
}
