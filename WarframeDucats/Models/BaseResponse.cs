namespace WarframeDucats.Models
{
	public class BaseResponse<TModel>
		where TModel : class
	{
		public TModel Payload { get; set; }

		public string Error { get; set; }
	}
}
