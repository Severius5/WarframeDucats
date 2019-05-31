using Newtonsoft.Json;

namespace WarframeDucats.Models
{
	public class UserItem
	{
		[JsonProperty("id")]
		public string ItemId { get; set; }

		public UserItemDetails En { get; set; }
	}
}
