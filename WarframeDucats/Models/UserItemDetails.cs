using Newtonsoft.Json;

namespace WarframeDucats.Models
{
	public class UserItemDetails
	{
		[JsonProperty("item_name")]
		public string ItemName { get; set; }
	}
}
