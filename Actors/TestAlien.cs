using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;

namespace AlienGame.Actors
{
	using Order = Func<Actor, Model, bool>;

	interface IOrderTarget
	{
		void AcceptOrder(Model m, Point targetSquare);
	}

	class TestAlien : Mover, IOrderTarget
	{
		public override void Draw(Graphics g)
		{
			DrawDirection(g);
			DrawBasicActor(g, Pens.Blue);
		}

		public void AcceptOrder(Model m, Point targetSquare)
		{
			var food = m.ActorsAt(targetSquare).OfType<Food>().FirstOrDefault();

			if (food == null)
				this.SetOrders(PlanPathTo(m, targetSquare));
			else
				this.SetOrders(PlanToEat(m, food, targetSquare));
		}

		public IEnumerable<Order> PlanToEat(Model m, Food f, Point to)
		{
			return PlanPathTo(m, to).Concat(new Order[] { Orders.Eat(f) });
		}

		public TestAlien() : base() { }
		public TestAlien(XmlElement e) : base(e) { }
	}
}
