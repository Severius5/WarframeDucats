using Newtonsoft.Json;

namespace WarframeDucats.Models
{
	public class UserItemOrder
	{
		public Region Region { get; set; }

		public Platform Platform { get; set; }

		[JsonProperty("order_type")]
		public OrderType OrderType { get; set; }

		public int Quantity { get; set; }

		public bool Visible { get; set; }

		public int Platinum { get; set; }

		public UserItem Item { get; set; }
	}
}
