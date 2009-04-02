using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienGame.Actors
{
	using Order = Func<Actor, Model, bool>;

	abstract class Mover : Actor
	{
		List<Order> orders = new List<Order>();

		public void CancelOrders() { orders.Clear(); }
		public void SetOrders(IEnumerable<Order> orders)
		{
			this.orders = orders.ToList();
		}

		public override void Tick(Model m)
		{
			while (orders.Count > 0 && orders[0](this, m))
				orders.RemoveAt(0);
		}
	}
}
