using System.Collections.Generic;
using System.Linq;
using WarframeDucats.ViewModels;

namespace WarframeDucats.Services
{
	internal static class MsgGenerator
	{
		private const int _itemsLimit = 6;

		public static string Generate(IEnumerable<OrderRow> orders)
		{
			var context = new MessageContext();

			foreach (var order in orders)
				ProcessOrder(context, order);

			var msg = $"/w {orders.First().Username} Hello! I want to buy: {string.Join(", ", context.MessageParts)}";
			if (context.AvaiableSlots != _itemsLimit - 1) // when user want to buy more than one item
				msg += $" for a total of {context.TotalPrice} platinum";

			return msg;
		}

		public static string Generate(OrderRow order)
		{
			return Generate(new List<OrderRow>(1) { order });
		}

		private static void ProcessOrder(MessageContext context, OrderRow order)
		{
			if (context.AvaiableSlots <= 0)
				return;

			int quantity = order.Quantity;
			if (quantity > context.AvaiableSlots)
				quantity = context.AvaiableSlots;

			if (quantity == 1)
				context.MessageParts.Add($"{order.ItemName} ({order.Plat}p)");
			else
				context.MessageParts.Add($"{quantity}x {order.ItemName} ({order.Plat}p/ea)");

			context.AvaiableSlots -= quantity;
			context.TotalPrice += quantity * order.Plat;
		}

		private class MessageContext
		{
			public int TotalPrice { get; set; }

			public int AvaiableSlots { get; set; } = _itemsLimit;

			public List<string> MessageParts { get; } = new List<string>();
		}
	}
}
