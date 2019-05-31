using Newtonsoft.Json;

namespace WarframeDucats.Models
{
	public class Item
	{
		[JsonProperty("id")]
		public string ItemId { get; set; }

		[JsonProperty("url_name")]
		public string UrlName { get; set; }

		[JsonProperty("item_name")]
		public string ItemName { get; set; }
	}
}
