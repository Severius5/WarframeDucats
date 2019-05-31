using Newtonsoft.Json;

namespace WarframeDucats.Models
{
	public class DucatInfo
	{
		public int Ducats { get; set; }

		[JsonProperty("ducats_per_platinum_wa")]
		public float DucatsPerPlat { get; set; }

		[JsonProperty("item")]
		public string ItemId { get; set; }
	}
}
