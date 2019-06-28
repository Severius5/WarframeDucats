using Newtonsoft.Json;

namespace WarframeDucats.Models
{
	public class ItemOrder
	{
		[JsonProperty("order_type")]
		public OrderType OrderType { get; set; }

		public Region Region { get; set; }

		public Platform Platform { get; set; }

		public User User { get; set; }

		public bool Visible { get; set; }

		public int Quantity { get; set; }

		public double Platinum { get; set; }
	}
}
