using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace AlienGame.Actors
{
	using System.Drawing;
	using Order = Func<Actor, bool>;

	abstract class Mover : Actor
	{
		List<Order> orders = new List<Order>();

		public void CancelOrders() { orders.Clear(); }
		public void SetOrders(IEnumerable<Order> orders)
		{
			this.orders = orders.ToList();
		}

		public void InterruptOrders(IEnumerable<Order> newOrders)
		{
			this.orders.InsertRange(0, newOrders);
		}

		public override void Tick()
		{
			while (orders.Count > 0 && orders[0](this))
				orders.RemoveAt(0);
		}

		protected Mover(Model m) : base(m) { }
		protected Mover(Model m, XmlElement e) : base(m, e) { }

		public IEnumerable<Order> PlanPathTo(Point to)
		{
			var from = Position.ToSquare();
			var pf = new Pathfinder(m);

			var path = pf.FindPath(from, to).ToList();
			for (int i = path.Count - 1; i >= 0; i--)
			{
				var walkTo = path[i].SquareToCenter();
				yield return Orders.Face(MakeDirection(from, path[i], 0), 1);
				yield return Orders.Walk(walkTo, 6);
				from = path[i];
			}
		}
	}
}
