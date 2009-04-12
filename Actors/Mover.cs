using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AlienGame.Actors
{
	using Order = Func<Actor, Model, bool>;
	using System.Drawing;

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

		public Mover() : base() { }
		public Mover(XmlElement e) : base(e) { }

		public IEnumerable<Order> PlanPathTo(Model m, Point to)
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
